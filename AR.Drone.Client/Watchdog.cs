using System.Threading;
using AR.Drone.Client.Acquisition;
using AR.Drone.Infrastructure;

namespace AR.Drone.Client
{
    public class Watchdog : WorkerBase
    {
        private readonly CommandSender _commandSender;
        private readonly NavdataAcquisition _navdataAcquisition;
        private readonly VideoAcquisition _videoAcquisition;

        public Watchdog(NavdataAcquisition navdataAcquisition,
                        CommandSender commandSender,
                        VideoAcquisition videoAcquisition)
        {
            _navdataAcquisition = navdataAcquisition;
            _commandSender = commandSender;
            _videoAcquisition = videoAcquisition;
        }

        protected override void Loop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                if (_navdataAcquisition.IsAlive == false)
                {
                    _navdataAcquisition.Start();
                }
                else if (_navdataAcquisition.IsAcquiring)
                {
                    if (_commandSender.IsAlive == false) _commandSender.Start();
                    if (_videoAcquisition.IsAlive == false) _videoAcquisition.Start();
                }
                Thread.Sleep(100);
            }

            _navdataAcquisition.Stop();
            _commandSender.Stop();
            _videoAcquisition.Stop();
        }
    }
}