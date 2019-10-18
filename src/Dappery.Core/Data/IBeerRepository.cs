namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>> GetAllBeers();
        
        Task<Beer> GetBeerById(int id);

        Task<Beer> CreateBeer(Beer beer);

        Task<Beer> UpdateBeer(Beer beer);

        Task<int> DeleteBeer(int beer);
    }
}