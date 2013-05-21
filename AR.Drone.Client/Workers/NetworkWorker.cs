// TODO: remove
//using System;
//using System.Net.NetworkInformation;
//using System.Net.Sockets;
//using System.Threading;
//using AI.Core.System;
//using AR.Drone.Client.Configuration;
//using System.Net;

//namespace AR.Drone.Client.Workers
//{
//    public class NetworkWorker : WorkerBase
//    {
//        private const int TelnetPort = 23;
//        private const int TelnetConnectTimeout = 1000;
//        private readonly INetworkConfiguration _configuration;
//        private readonly Action<bool> _connectionChanged;
//        private bool _isConnected;

//        public NetworkWorker(INetworkConfiguration configuration, Action<bool> connectionChanged)
//        {
//            _configuration = configuration;
//            _connectionChanged = connectionChanged;
//        }

//        public bool IsConnected
//        {
//            get { return IsAlive && _isConnected; }
//        }

//        protected override void Loop(CancellationToken token)
//        {
//            _isConnected = false;

//            while (token.IsCancellationRequested == false)
//            {
//                bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
//                if (isNetworkAvailable)
//                {
                   
//                    bool connected = false;
//                    using (var client = new TcpClient())
//                        try
//                        {
//                            IPAddress[] addresses = Dns.GetHostAddresses(_configuration.DroneHostname);
//                            foreach (var address in addresses)
//                            {
//                                IAsyncResult result = client.BeginConnect(address, TelnetPort, null, null);
//                                connected = result.AsyncWaitHandle.WaitOne(TelnetConnectTimeout,  false);
//                            }
//                        }
//                        catch (SocketException)
//                        {
//                        }

//                    if (connected)
//                    {
//                        _isConnected = true;
//                        _connectionChanged(true);
//                    }
//                    else
//                    {
//                        _isConnected = false;
//                        _connectionChanged(false);
//                    }
//                }
//                Thread.Sleep(1000);
//            }
//        }
//    }
//}