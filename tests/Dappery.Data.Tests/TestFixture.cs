namespace Dappery.Data.Tests
{
    using System;
    using System.Threading;
    using Core.Data;

    public class TestFixture : IDisposable
    {
        protected TestFixture()
        {
            UnitOfWork = new UnitOfWork(null);
        }
        
        protected IUnitOfWork UnitOfWork { get; }

        protected CancellationToken CancellationTestToken => CancellationToken.None;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
        }
    }
}