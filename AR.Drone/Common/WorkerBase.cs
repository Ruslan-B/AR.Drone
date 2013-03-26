using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AR.Drone.Common
{
    public abstract class WorkerBase : DisposableBase
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;

        public bool IsAlive
        {
            get { return _task != null; }
        }

        public void Start()
        {
            if (IsAlive) return;
            lock (this)
            {
                if (IsAlive) return;

                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = _cancellationTokenSource.Token;
                _task = new Task(() => RunLoop(token), token);
                _task.Start();
            }
        }

        public void Stop()
        {
            Task task = _task;
            CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;
            if (task != null && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                task.Wait();
            }
        }

        private void RunLoop(CancellationToken token)
        {
            try
            {
                Loop(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                OnException(exception);
            }
            finally
            {
                CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;
                if (cancellationTokenSource != null)
                {
                    _cancellationTokenSource = null;
                    cancellationTokenSource.Dispose();
                }
                _task = null;
            }
        }

        protected abstract void Loop(CancellationToken token);

        protected virtual void OnException(Exception exception)
        {
            Trace.TraceError("{0} - Exception: {1}", GetType(), exception.Message);
        }

        protected override void DisposeOverride()
        {
            Stop();
        }
    }
}