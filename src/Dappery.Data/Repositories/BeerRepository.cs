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
        private readonly bool _useSqlite;
    
        public BeerRepository(IDbTransaction dbTransaction, bool useSqlite)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = _dbTransaction.Connection;
            _useSqlite = useSqlite;
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

            if (address != null && beerFromId.Brewery != null)
            {
                beerFromId.Brewery.Address = address;
            }
            
            // Finally, let's add all the beers to our brewery attached to this beer
            var beersFromBrewery = await _dbConnection.QueryAsync<Beer>(
                @"SELECT * FROM Beers WHERE BreweryId = @BreweryId",
                new { beerFromId.BreweryId },
                _dbTransaction);

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
            
            // Based on our database implementation, we'll need a reference to the last row inserted
            var lastRowIdSql = _useSqlite ? "SELECT last_insert_rowid();" : "SELECT CAST(SCOPE_IDENTITY() as int);";

            // Let's insert the beer and grab its ID
            var beerId = await _dbConnection.ExecuteScalarAsync<int>(
                beerToInsertSql.Append(lastRowIdSql).ToString(),
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
    }
}