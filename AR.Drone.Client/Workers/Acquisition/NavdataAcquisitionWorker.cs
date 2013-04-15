using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Configuration;
using AR.Drone.Client.Packets;

namespace AR.Drone.Client.Workers.Acquisition
{
    public class NavdataAcquisitionWorker : WorkerBase
    {
        public const int NavdataPort = 5554;
        public const int KeepAliveTimeout = 200;
        public const int NavdataTimeout = 2000;

        private readonly INetworkConfiguration _configuration;
        private readonly Action<NavigationPacket> _navigationPacketAcquired;
        private readonly UdpClient _udpClient;

        public NavdataAcquisitionWorker(INetworkConfiguration configuration, Action<NavigationPacket> navigationPacketAcquired)
        {
            _configuration = configuration;
            _navigationPacketAcquired = navigationPacketAcquired;
            _udpClient = new UdpClient(NavdataPort);
        }

        protected override void Loop(CancellationToken token)
        {
            _udpClient.Connect(_configuration.DroneHostname, NavdataPort);

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
                    var packet = new NavigationPacket
                        {
                            Timestamp = DateTime.UtcNow.Ticks,
                            Data = data
                        };
                    _navigationPacketAcquired(packet);
                    swNavdataTimeout.Restart();
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