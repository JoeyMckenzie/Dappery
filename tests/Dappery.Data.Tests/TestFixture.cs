namespace Dappery.Data.Tests
{
    using System;
    using Core.Data;

    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            UnitOfWork = new UnitOfWork(null);
        }
        
        protected IUnitOfWork UnitOfWork { get; }
        
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