using System;
using System.Net.NetworkInformation;
using System.Threading;
using AI.Core.System;

namespace AR.Drone.Client.Workers
{
    public class NetworkWorker : WorkerBase
    {
        private readonly ARDroneConfig _config;
        private readonly Action<bool> _connectionChanged;
        private bool _isConnected;

        public NetworkWorker(ARDroneConfig config, Action<bool> connectionChanged)
        {
            _config = config;
            _connectionChanged = connectionChanged;
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
                                _connectionChanged(true);
                            }
                            else
                            {
                                // todo add timeout?
                                _isConnected = false;
                                _connectionChanged(false);
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}