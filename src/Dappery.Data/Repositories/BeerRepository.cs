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

    public class BeerRepository : IBeerRepository
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly IDbConnection _dbConnection;
        private readonly string _insertRowRetrievalQuery;
    
        public BeerRepository(IDbTransaction dbTransaction, string insertRowRetrievalQuery)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = _dbTransaction.Connection;
            _insertRowRetrievalQuery = insertRowRetrievalQuery;
        }
        
        public async Task<IEnumerable<Beer>> GetAllBeers()
        {
            // Retrieve the addresses, as this is a nested mapping
            var addresses = (await _dbConnection.QueryAsync<Address>(
                "SELECT * FROM Addresses",
                transaction: _dbTransaction)).ToList();
            
            return await _dbConnection.QueryAsync<Beer, Brewery, Beer>(
                @"SELECT b.*, br.* FROM Beers b INNER JOIN Breweries br ON br.Id = b.BreweryId",
                (beer, brewery) =>
                {
                    // Map the brewery that Dapper returns for us to the beer 
                    brewery.Address = addresses.FirstOrDefault(a => a.BreweryId == brewery.Id);
                    beer.Brewery = brewery;
                    return beer;
                },
                transaction: _dbTransaction
            );
        }
        
        public async Task<Beer> GetBeerById(int id)
        {
            // Retrieve the beer from the database
            var beerFromId = (await _dbConnection.QueryAsync<Beer, Brewery, Beer>(
                @"SELECT b.*, br.* FROM Beers b 
                INNER JOIN Breweries br ON br.Id = b.BreweryId
                WHERE b.Id = @Id",
                (beer, brewery) =>
                {
                    beer.Brewery = brewery;
                    return beer;
                },
                new { Id = id },
                _dbTransaction)).FirstOrDefault();

            // Return back to the caller if no beer is found, let the business logic decide what to do if we can't the specified beer
            if (beerFromId == null)
            {
                return null;
            }

            // Map the address to the beer's brewery
            var address = await _dbConnection.QueryFirstOrDefaultAsync<Address>(
                @"SELECT * FROM Addresses WHERE BreweryId = @BreweryId",
                new { BreweryId = beerFromId.Brewery?.Id },
                _dbTransaction);

            // Set the address found in the previous query to the beer's brewery address, if we have a brewery
            if (beerFromId.Brewery != null)
            {
                beerFromId.Brewery.Address = address;
            }
            
            // Let's add all the beers to our brewery attached to this beer
            var beersFromBrewery = await _dbConnection.QueryAsync<Beer>(
                @"SELECT * FROM Beers WHERE BreweryId = @BreweryId",
                new { beerFromId.BreweryId },
                _dbTransaction);

            // Lastly, let's add all the beers to the entity model
            foreach (var beer in beersFromBrewery)
            {
                beerFromId.Brewery?.Beers.Add(beer);
            }

            return beerFromId;
        }

        public async Task<Beer> CreateBeer(Beer beer)
        {
            // From our business we defined, we'll assume the brewery ID is always attached to the beer
            var beerToInsertSql = new StringBuilder(@"INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                                        VALUES (@Name, @BeerStyle, @CreatedAt, @UpdatedAt, @BreweryId);");
            
            // Let's insert the beer and grab its ID
            var beerId = await _dbConnection.ExecuteScalarAsync<int>(
                beerToInsertSql.Append(_insertRowRetrievalQuery).ToString(),
                new
                {
                    beer.Name,
                    beer.BeerStyle,
                    beer.CreatedAt,
                    beer.UpdatedAt,
                    beer.BreweryId
                },
                _dbTransaction);
            
            // Finally, we'll return the newly inserted beer
            return await GetBeerById(beerId);
        }

        public async Task<Beer> UpdateBeer(Beer beer)
        {
            // Our application layer will be in charge of mapping the new properties to the entity layer,
            // as well as validating that the beer exists, so the data layer will only be responsible for
            // inserting the values into the database; separation of concerns!
            await _dbConnection.ExecuteAsync(
                @"UPDATE Beers SET Name = @Name, BeerStyle = @BeerStyle, UpdatedAt = @UpdatedAt, BreweryId = @BreweryId WHERE Id = @Id",
                new
                {
                    beer.Name,
                    beer.BeerStyle,
                    beer.UpdatedAt,
                    beer.BreweryId,
                    beer.Id
                },
                _dbTransaction);
            
            // Finally, we'll return the newly inserted beer
            return await GetBeerById(beer.Id);
        }

        public async Task<int> DeleteBeer(int beerId)
        {
            // Our simplest command, just remove the beer directly from the database
            // Validation that the beer actually exists in the database will left to the application layer
            return await _dbConnection.ExecuteAsync(
                @"DELETE FROM Beers WHERE Id = @Id",
                new { Id = beerId },
                _dbTransaction);
        }
    }
}