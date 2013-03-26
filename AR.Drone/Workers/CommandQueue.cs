using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AR.Drone.Api;
using AR.Drone.Api.Commands;
using AR.Drone.Helpers;
using AR.Drone.Common;

namespace AR.Drone.Workers
{
    public class CommandQueue : WorkerBase
    {
        public const int CommandPort = 5556;
        public const int KeepAliveTimeout = 50;

        private readonly ConcurrentQueue<IATCommand> _queue;
        private readonly DroneConfig _config;
        private readonly UdpClient _udpClient;

        public CommandQueue(DroneConfig config)
        {
            _config = config;
            _queue = new ConcurrentQueue<IATCommand>();
            _udpClient = new UdpClient(CommandPort);
        }

        public void Flush()
        {
            ConcurrentQueueHelper.Clear(_queue);
        }

        public void Enqueue(IATCommand command)
        {
            _queue.Enqueue(command);
        }

        protected override void Loop(CancellationToken token)
        {
            int sequenceNumber = 1;
            Flush();

            _udpClient.Connect(_config.Hostname, CommandPort);

            byte[] firstMessage = BitConverter.GetBytes(1);
            _udpClient.Send(firstMessage, firstMessage.Length);

            Stopwatch swKeepAlive = Stopwatch.StartNew();
            while (token.IsCancellationRequested == false)
            {
                IATCommand command;
                if (_queue.TryDequeue(out command))
                {
                    string at = command.ToAt(sequenceNumber);
                    Trace.WriteIf((command is COMWDGCommand) == false, at);
                    byte[] payload = Encoding.ASCII.GetBytes(at);
                    _udpClient.Send(payload, payload.Length);
                    sequenceNumber++;
                    swKeepAlive.Restart();
                }
                else if (swKeepAlive.ElapsedMilliseconds > KeepAliveTimeout)
                {
                    _queue.Enqueue(new COMWDGCommand());
                }
                Thread.Sleep(1);
            }
        }
    }
}