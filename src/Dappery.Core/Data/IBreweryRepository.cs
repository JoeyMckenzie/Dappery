namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBreweryRepository
    {
        Task<IEnumerable<Brewery>> GetAllBreweries();
        
        Task<Brewery> GetBreweryById(int id);

        Task<int> CreateBrewery(Brewery brewery);

        Task UpdateBrewery(Brewery brewery, bool updateAddress = false);

        Task<int> DeleteBrewery(int breweryId);
    }
}