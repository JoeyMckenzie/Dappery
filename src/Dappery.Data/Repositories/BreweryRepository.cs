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
            return (await _dbConnection.QueryAsync(
                @"SELECT * FROM Dappery.dbo.Breweries WHERE Id = @Id",
                new { Id = id },
                _dbTransaction)).FirstOrDefault();
        }

        public async Task<IEnumerable<Brewery>> GetAllBreweries()
        {
            return await _dbConnection.QueryAsync<Brewery>(
                "SELECT * FROM Dappery.dbo.Breweries",
                transaction: _dbTransaction);
        }

        public async Task<Brewery> GetAddressFromBreweryId(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return (await _dbConnection.QueryAsync<Brewery>(
                "GetAddressByBreweryId",
                parameters,
                commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
    }
}