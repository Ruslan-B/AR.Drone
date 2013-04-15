using System;
using System.Net.NetworkInformation;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Configuration;

namespace AR.Drone.Client.Workers
{
    public class NetworkWorker : WorkerBase
    {
        private readonly INetworkConfiguration _configuration;
        private readonly Action<bool> _connectionChanged;
        private bool _isConnected;

        public NetworkWorker(INetworkConfiguration configuration, Action<bool> connectionChanged)
        {
            _configuration = configuration;
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
                            PingReply result = ping.Send(_configuration.DroneHostname);
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