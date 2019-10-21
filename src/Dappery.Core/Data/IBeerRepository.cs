namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>> GetAllBeers();
        
        Task<Beer> GetBeerById(int id);

        Task<int> CreateBeer(Beer beer);

        Task UpdateBeer(Beer beer);

        Task<int> DeleteBeer(int beer);
    }
}