using System;

namespace AR.Drone.Common
{
    public abstract class DisposableBase : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing == false)
            {
                DisposeOverride();
            }

            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeOverride();
    }
}