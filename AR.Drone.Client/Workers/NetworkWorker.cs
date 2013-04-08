using System;
using System.Net.NetworkInformation;
using System.Threading;
using AI.Core.System;

namespace AR.Drone.Client.Workers
{
    public class NetworkWorker : WorkerBase
    {
        private readonly ARDroneConfig _config;
        private readonly Action<bool> _connectionStateChanged;
        private bool _isConnected;

        public NetworkWorker(ARDroneConfig config, Action<bool> connectionStateChanged)
        {
            _config = config;
            _connectionStateChanged = connectionStateChanged;
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
                                _connectionStateChanged(true);
                            }
                            else
                            {
                                // todo add timeout?
                                _isConnected = false;
                                _connectionStateChanged(false);
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}