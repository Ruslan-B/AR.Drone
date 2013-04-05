using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AR.Drone.Command;
using AR.Drone.Common;
using AR.Drone.Helpers;

namespace AR.Drone.Workers
{
    public class CommandQueueWorker : WorkerBase
    {
        public const int CommandPort = 5556;
        public const int KeepAliveTimeout = 50;

        private readonly DroneConfig _config;
        private readonly ConcurrentQueue<ATCommand> _queue;
        private readonly UdpClient _udpClient;

        public CommandQueueWorker(DroneConfig config)
        {
            _config = config;
            _queue = new ConcurrentQueue<ATCommand>();
            _udpClient = new UdpClient(CommandPort);
        }

        public void Flush()
        {
            ConcurrentQueueHelper.Flush(_queue);
        }

        public void Enqueue(ATCommand command)
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
                ATCommand command;
                if (_queue.TryDequeue(out command))
                {
                    byte[] payload = command.CreatePayload(sequenceNumber);

                    Trace.WriteIf((command is COMWDGCommand) == false, Encoding.ASCII.GetString(payload));

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