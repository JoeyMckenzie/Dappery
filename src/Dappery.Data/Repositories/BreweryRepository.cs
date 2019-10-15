namespace Dappery.Data.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Data;
    using Dapper;
    using Domain.Entities;

    public class BreweryRepository : IBreweryRepository
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly IDbConnection _dbConnection;

        public BreweryRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = _dbTransaction.Connection;
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
            return await _dbConnection.QueryAsync<Brewery, Address, Brewery>(
                "SELECT * FROM Breweries br INNER JOIN Addresses a ON a.BreweryId = br.Id",
                (brewery, address) =>
                {
                    brewery.Address = address;
                    return brewery;
                },
                transaction: _dbTransaction);
        }
    }
}