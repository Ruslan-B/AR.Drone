using System.Threading;
using AI.Core.System;

namespace AR.Drone.Client.Workers
{
    public class WatchdogWorker : WorkerBase
    {
        private readonly CommandQueueWorker _commandQueueWorker;
        private readonly NavdataAcquisitionWorker _navdataAcquisitionWorker;
        private readonly NetworkWorker _networkWorker;
        private readonly VideoAcquisitionWorker _videoAcquisitionWorker;

        public WatchdogWorker(NetworkWorker networkWorker,
                              NavdataAcquisitionWorker navdataAcquisitionWorker,
                              CommandQueueWorker commandQueueWorker,
                              VideoAcquisitionWorker videoAcquisitionWorker)
        {
            _networkWorker = networkWorker;
            _navdataAcquisitionWorker = navdataAcquisitionWorker;
            _commandQueueWorker = commandQueueWorker;
            _videoAcquisitionWorker = videoAcquisitionWorker;
        }

        protected override void Loop(CancellationToken token)
        {
            _networkWorker.Start();

            while (token.IsCancellationRequested == false)
            {
                if (_networkWorker.IsAlive == false)
                {
                    _networkWorker.Start();
                }
                else if (_networkWorker.IsConnected)
                {
                    if (_navdataAcquisitionWorker.IsAlive == false || _commandQueueWorker.IsAlive == false)
                    {
                        if (_commandQueueWorker.IsAlive) _commandQueueWorker.Stop();
                        if (_navdataAcquisitionWorker.IsAlive) _navdataAcquisitionWorker.Stop();

                        _commandQueueWorker.Start();
                        _navdataAcquisitionWorker.Start();
                    }
                    if (_videoAcquisitionWorker.IsAlive == false)
                    {
                        if (_videoAcquisitionWorker.IsAlive) _videoAcquisitionWorker.Stop();

                        _videoAcquisitionWorker.Start();
                    }
                }
                else
                {
                    // todo if network is not connected - think - shall we stop working workers 
                }
                Thread.Sleep(100);
            }
            _networkWorker.Stop();
            _navdataAcquisitionWorker.Stop();
            _commandQueueWorker.Stop();
            _videoAcquisitionWorker.Stop();
        }
    }
}