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
    }
}