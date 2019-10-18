namespace Dappery.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Data;
    using Dapper;
    using Domain.Entities;

    public class BreweryRepository : IBreweryRepository
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly IDbConnection _dbConnection;
        private readonly string _rowInsertRetrievalQuery;

        public BreweryRepository(IDbTransaction dbTransaction, string rowInsertRetrievalQuery)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = _dbTransaction.Connection;
            _rowInsertRetrievalQuery = rowInsertRetrievalQuery;
        }

        public async Task<Brewery> GetBreweryById(int id)
        {
            var beersFromBrewery = (await _dbConnection.QueryAsync<Beer>(
                @"SELECT * FROM Beers WHERE BreweryId = @Id",
                new {Id = id},
                _dbTransaction)).ToList();
            
            return (await _dbConnection.QueryAsync<Brewery, Address, Brewery>(
                @"SELECT br.*, a.* FROM Breweries br INNER JOIN Addresses a ON a.BreweryId = br.Id WHERE br.Id = @Id",
                (brewery, address) =>
                {
                    // Since breweries have a one-to-one relation with address, we can initialize that mapping here
                    brewery.Address = address;
                    
                    // Add each beer from the previous query into the list of beers for the brewery
                    if (beersFromBrewery.Any())
                    {
                        foreach (var beer in beersFromBrewery)
                        {
                            brewery.Beers.Add(beer);
                        }
                    }
                    
                    return brewery;
                },
                new { Id = id },
                _dbTransaction)).FirstOrDefault();
        }

        public async Task<IEnumerable<Brewery>> GetAllBreweries()
        {
            // Grab a reference to all beers so we can map them to there corresponding breweries
            var beers = (await _dbConnection.QueryAsync<Beer>(
                "SELECT * FROM Beers",
                transaction: _dbTransaction)).ToList();
            
            return await _dbConnection.QueryAsync<Brewery, Address, Brewery>(
                // We join with addresses as there's a one-to-one relation with breweries, making the query a little less intensive
                "SELECT * FROM Breweries br INNER JOIN Addresses a ON a.BreweryId = br.Id",
                (brewery, address) =>
                {
                    // Map the address to the brewery
                    brewery.Address = address;
                    
                    // Map each beer to the beer collection for the brewery during iteration over our result set 
                    if (beers.Any(b => b.BreweryId == brewery.Id))
                    {
                        foreach (var beer in beers.Where(b => b.BreweryId == brewery.Id))
                        {
                            brewery.Beers.Add(beer);
                        }
                    }
                    
                    return brewery;
                },
                transaction: _dbTransaction);
        }
        
        public async Task<Brewery> CreateBrewery(Brewery brewery)
        {
            // Grab a reference to the address
            var address = brewery.Address;
            var breweryInsertSql =
                new StringBuilder(@"INSERT INTO Breweries (Name, CreatedAt, UpdatedAt) VALUES (@Name, @CreatedAt, @UpdatedAt);");
            
            // Let's add the brewery
            var breweryId = await _dbConnection.ExecuteScalarAsync<int>(
                breweryInsertSql.Append(_rowInsertRetrievalQuery).ToString(),
                new { brewery.Name, brewery.CreatedAt, brewery.UpdatedAt },
                _dbTransaction);

            // One of our business rules is that a brewery must have an associated address
            await _dbConnection.ExecuteAsync(
                @"INSERT INTO Addresses (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
                        VALUES (@StreetAddress, @City, @State, @ZipCode, @CreatedAt, @UpdatedAt, @BreweryId)",
                new
                {
                    address.StreetAddress, 
                    address.City,
                    address.State,
                    address.ZipCode,
                    address.CreatedAt,
                    address.UpdatedAt,
                    BreweryId = breweryId
                }, 
                _dbTransaction);
            
            return await GetBreweryById(breweryId);
        }

        public async Task<Brewery> UpdateBrewery(Brewery brewery, bool updateAddress)
        {
            // Again, we'll assume the brewery details are being validated and mapped properly in the application layer
            await _dbConnection.ExecuteAsync(
                @"UPDATE Breweries SET Name = @Name, UpdatedAt = @UpdatedAt WHERE Id = @Id",
                new
                {
                    brewery.Name,
                    brewery.UpdatedAt,
                    brewery.Id
                },
                _dbTransaction);

            if (brewery.Address != null && updateAddress)
            {
                // Again, we'll assume the brewery details are being validated and mapped properly in the application layer
                // For now, we won't allow users to swap breweries address to another address
                await _dbConnection.ExecuteAsync(
                    @"UPDATE Addresses SET StreetAddress = @StreetAddress, City = @City, ZipCode = @ZipCode, State = @State, UpdatedAt = @UpdatedAt WHERE Id = @Id",
                    new
                    {
                        brewery.Address.StreetAddress,
                        brewery.Address.City,
                        brewery.Address.ZipCode,
                        brewery.Address.State,
                        brewery.Address.UpdatedAt,
                        brewery.Address.Id
                    },
                    _dbTransaction);
            }
            
            return await GetBreweryById(brewery.Id);
        }

        public async Task<int> DeleteBrewery(int breweryId)
        {
            // Because we setup out database providers to cascade delete on parent entity removal, we won't have to
            // worry about individually removing all the associated beers and address
            // NOTE: Because we don't directly expose CRUD operations on the address table, we'll validate the cascade
            // remove directly in the database for now
            return await _dbConnection.ExecuteAsync(
                    @"DELETE FROM Breweries WHERE Id = @Id",
                new {Id = breweryId},
                _dbTransaction);
        }
    }
}