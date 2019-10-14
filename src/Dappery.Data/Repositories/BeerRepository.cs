namespace Dappery.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Data;
    using Dapper;
    using Domain.Entities;

    public class BeerRepository : IBeerRepository
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly IDbConnection _dbConnection;
    
        public BeerRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = _dbTransaction.Connection;
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
//                splitOn: "Id",
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
//                splitOn: "Id",
                param: new { Id = id },
                transaction: _dbTransaction)).FirstOrDefault();

            // Return back to the caller if no beer is found, let the business logic decide what to do if we can't the specified beer
            if (beerFromId == null)
            {
                return null;
            }
            
            // Map the address to the beer's brewery
            var address = await _dbConnection.QueryFirstOrDefaultAsync<Address>(
                @"SELECT * FROM Addresses WHERE BreweryId = @BreweryId",
                param: new { BreweryId = beerFromId?.Brewery?.Id },
                transaction: _dbTransaction);

            if (address != null && beerFromId.Brewery != null)
            {
                beerFromId.Brewery.Address = address;
            }

            return beerFromId;
        }
    }
}