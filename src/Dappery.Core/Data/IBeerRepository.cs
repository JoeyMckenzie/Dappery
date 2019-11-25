namespace Dappery.Core.Data
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>?> GetAllBeersAsync(CancellationToken cancellationToken);
        
        Task<Beer> GetBeerByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<int> CreateBeerAsync(Beer beer, CancellationToken cancellationToken);

        Task UpdateBeerAsync(Beer beer, CancellationToken cancellationToken);

        Task DeleteBeerAsync(int beer, CancellationToken cancellationToken);
    }
}