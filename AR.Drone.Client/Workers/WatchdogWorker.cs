using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Workers.Acquisition;

namespace AR.Drone.Client.Workers
{
    public class WatchdogWorker : WorkerBase
    {
        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;

        public WatchdogWorker(NavdataAcquisitionWorker navdataAcquisitionWorker,
                              CommandQueueWorker commandQueueWorker,
                              VideoAcquisitionWorker videoAcquisitionWorker)
        {
            _navdataAcquisitionWorker = navdataAcquisitionWorker;
            _commandQueueWorker = commandQueueWorker;
            _videoAcquisitionWorker = videoAcquisitionWorker;
        }

        protected override void Loop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                if (_navdataAcquisitionWorker.IsAlive == false)
                {
                    _navdataAcquisitionWorker.Start();
                }
                else if (_navdataAcquisitionWorker.IsAcquiring)
                {
                    if (_commandQueueWorker.IsAlive == false) _commandQueueWorker.Start();
                    if (_videoAcquisitionWorker.IsAlive == false) _videoAcquisitionWorker.Start();
                }
                Thread.Sleep(100);
            }

            _navdataAcquisitionWorker.Stop();
            _commandQueueWorker.Stop();
            _videoAcquisitionWorker.Stop();
        }
    }
}