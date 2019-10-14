namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBreweryRepository
    {
        Task<IEnumerable<Brewery>> GetAllBreweries();
        
        Task<Brewery> GetBreweryById(int id);

        Task<Brewery> GetAddressFromBreweryId(int id);
    }
}