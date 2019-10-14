namespace Dappery.Core.Data
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IBeerRepository BeerRepository { get; }
        
        IBreweryRepository BreweryRepository { get; }
        
        void Commit();
    }
}