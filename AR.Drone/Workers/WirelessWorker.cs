using System.Net.NetworkInformation;
using System.Threading;
using AR.Drone.Common;

namespace AR.Drone
{
    public class WirelessWorker : WorkerBase
    {
        private readonly DroneConfig _config;
        private bool _isConnected;

        public WirelessWorker(DroneConfig config)
        {
            _config = config;
        }

        public bool IsConnected
        {
            get { return IsAlive && _isConnected; }
        }

        protected override void Loop(CancellationToken token)
        {
            _isConnected = false;

            while (token.IsCancellationRequested == false)
            {
                bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
                if (isNetworkAvailable)
                {
                    NetworkInterface[] ifs = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface @if in ifs)
                    {
                        // check for wireless and up
                        if (@if.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && @if.OperationalStatus == OperationalStatus.Up)
                        {
                            var ping = new Ping();
                            PingReply result = ping.Send(_config.Hostname);
                            if (result != null && result.Status == IPStatus.Success)
                            {
                                _isConnected = true;
                            }
                            else
                            {
                                _isConnected = false;
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}