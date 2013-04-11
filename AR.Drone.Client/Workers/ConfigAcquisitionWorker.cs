using System;
using System.Net.Sockets;
using System.Threading;
using AI.Core.System;

namespace AR.Drone.Client.Workers
{
    public class ConfigAcquisitionWorker : WorkerBase
    {
        private const int ControlPort = 5559;
        private const int NetworkBufferSize = 0x10000;
        private readonly ARDroneConfig _config;
        private readonly Action<ConfigurationPacket> _configurationAcquired;

        public ConfigAcquisitionWorker(ARDroneConfig config, Action<ConfigurationPacket> configurationAcquired)
        {
            _config = config;
            _configurationAcquired = configurationAcquired;
        }

        protected override void Loop(CancellationToken token)
        {
            using (var tcpClient = new TcpClient(_config.Hostname, ControlPort))
            using (NetworkStream stream = tcpClient.GetStream())
            {
                var buffer = new byte[NetworkBufferSize];
                while (token.IsCancellationRequested == false)
                {
                    int offset = 0;
                    if (tcpClient.Available > 0)
                    {
                        offset += stream.Read(buffer, offset, buffer.Length);
                        var packet = new ConfigurationPacket
                            {
                                Timestamp = DateTime.UtcNow.Ticks,
                                Data = buffer
                            };
                        _configurationAcquired(packet);
                    }
                    Thread.Sleep(10);
                }
            }
        }
    }

    public struct ConfigurationPacket
    {
        public byte[] Data;
        public long Timestamp;
    }
}