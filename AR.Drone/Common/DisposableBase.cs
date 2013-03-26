using System;

namespace AR.Drone.Common
{
    public abstract class DisposableBase : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed == false)
            {
                DisposeOverride();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeOverride();
    }
}