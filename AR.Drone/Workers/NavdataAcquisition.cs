using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AR.Drone.Common;
using AR.Drone.NativeApi;

namespace AR.Drone.Workers
{
    public class NavdataAcquisition : WorkerBase
    {
        public const int NavdataPort = 5554;
        public const int KeepAliveTimeout = 200;
        public const int NavdataTimeout = 2000;
        
        private readonly DroneConfig _config;
        private readonly Action<RawNavdata> _navdataAcquired;
        private readonly UdpClient _udpClient;

        public NavdataAcquisition(DroneConfig config, Action<RawNavdata> navdataAcquired)
        {
            _config = config;
            _navdataAcquired = navdataAcquired;
            _udpClient = new UdpClient(NavdataPort);
        }

        protected override void Loop(CancellationToken token)
        {
            _udpClient.Connect(_config.Hostname, NavdataPort);

            SendKeepAliveSignal();

            var droneEp = new IPEndPoint(IPAddress.Any, NavdataPort);
            Stopwatch swKeepAlive = Stopwatch.StartNew();
            Stopwatch swNavdataTimeout = Stopwatch.StartNew();
            while (token.IsCancellationRequested == false &&
                   swNavdataTimeout.ElapsedMilliseconds < NavdataTimeout)
            {
                if (_udpClient.Available > 0)
                {
                    byte[] data = _udpClient.Receive(ref droneEp);

                    RawNavdata rawNavdata;
                    if (NavdataParser.TryParse(data, out rawNavdata))
                    {
                        _navdataAcquired(rawNavdata);

                        swNavdataTimeout.Restart();
                    }
                }

                if (swKeepAlive.ElapsedMilliseconds > KeepAliveTimeout)
                {
                    SendKeepAliveSignal();
                    swKeepAlive.Restart();
                }
                Thread.Sleep(5);
            }
        }

        private void SendKeepAliveSignal()
        {
            byte[] payload = BitConverter.GetBytes(1);
            _udpClient.Send(payload, payload.Length);
        }
    }
}