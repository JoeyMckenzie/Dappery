namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBreweryRepository
    {
        Task<IEnumerable<Brewery>> GetAllBreweries(CancellationToken cancellationToken);
        
        Task<Brewery> GetBreweryById(int id, CancellationToken cancellationToken);

        Task<int> CreateBrewery(Brewery brewery, CancellationToken cancellationToken);

        Task UpdateBrewery(Brewery brewery, CancellationToken cancellationToken, bool updateAddress = false);

        Task<int> DeleteBrewery(int breweryId, CancellationToken cancellationToken);
    }
}