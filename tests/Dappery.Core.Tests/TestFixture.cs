namespace Dappery.Core.Tests
{
    using System;
    using System.Threading;
    using Dappery.Data;
    using Data;

    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            // Initialize our test database with our seed data
            UnitOfWork = new UnitOfWork(null);
        }
        
        protected IUnitOfWork UnitOfWork { get; }
        
        protected static CancellationToken CancellationTestToken => CancellationToken.None;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
        }
    }
}