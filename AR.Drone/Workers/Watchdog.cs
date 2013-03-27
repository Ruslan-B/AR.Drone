using System.Threading;
using AR.Drone.Common;

namespace AR.Drone.Workers
{
    public class Watchdog : WorkerBase
    {
        private readonly CommandQueue _commandQueue;
        private readonly NavdataAcquisition _navdataAcquisition;
        private readonly VideoAcquisition _videoAcquisition;
        private readonly VideoDecoder _videoDecoder;
        private readonly NetworkWorker _networkWorker;

        public Watchdog(NetworkWorker networkWorker, NavdataAcquisition navdataAcquisition, CommandQueue commandQueue, VideoAcquisition videoAcquisition,
                        VideoDecoder videoDecoder)
        {
            _networkWorker = networkWorker;
            _navdataAcquisition = navdataAcquisition;
            _commandQueue = commandQueue;
            _videoAcquisition = videoAcquisition;
            _videoDecoder = videoDecoder;
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
                    if (_navdataAcquisition.IsAlive == false || _commandQueue.IsAlive == false)
                    {
                        if (_commandQueue.IsAlive) _commandQueue.Stop();
                        if (_navdataAcquisition.IsAlive) _navdataAcquisition.Stop();

                        _commandQueue.Start();
                        _navdataAcquisition.Start();
                    }
                    if (_videoAcquisition.IsAlive == false || _videoDecoder.IsAlive == false)
                    {
                        if (_videoAcquisition.IsAlive) _videoAcquisition.Stop();
                        if (_videoDecoder.IsAlive) _videoDecoder.Stop();

                        _videoAcquisition.Start();
                        _videoDecoder.Start();
                    }
                }
                else
                {
                    // todo think - shall we stop rest of workers 
                }
                Thread.Sleep(100);
            }
            _networkWorker.Stop();
            _navdataAcquisition.Stop();
            _commandQueue.Stop();
            _videoAcquisition.Stop();
            _videoDecoder.Stop();
        }
    }
}