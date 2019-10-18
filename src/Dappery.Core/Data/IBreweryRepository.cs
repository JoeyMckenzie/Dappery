namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBreweryRepository
    {
        Task<IEnumerable<Brewery>> GetAllBreweries();
        
        Task<Brewery> GetBreweryById(int id);

        Task<Brewery> CreateBrewery(Brewery brewery);

        Task<Brewery> UpdateBrewery(Brewery brewery, bool updateAddress = false);

        Task<int> DeleteBrewery(int breweryId);
    }
}