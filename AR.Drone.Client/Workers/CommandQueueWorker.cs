using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Helpers;

namespace AR.Drone.Client.Workers
{
    public class CommandQueueWorker : WorkerBase
    {
        public const int CommandPort = 5556;
        public const int KeepAliveTimeout = 50;

        private readonly ConcurrentQueue<ATCommand> _commandQueue;
        private readonly ARDroneConfig _config;
        private readonly UdpClient _udpClient;

        public CommandQueueWorker(ARDroneConfig config, ConcurrentQueue<ATCommand> commandQueue)
        {
            _config = config;
            _commandQueue = commandQueue;
            _udpClient = new UdpClient(CommandPort);
        }

        protected override void Loop(CancellationToken token)
        {
            int sequenceNumber = 1;
            ConcurrentQueueHelper.Flush(_commandQueue);

            _udpClient.Connect(_config.Hostname, CommandPort);

            byte[] firstMessage = BitConverter.GetBytes(1);
            _udpClient.Send(firstMessage, firstMessage.Length);

            Stopwatch swKeepAlive = Stopwatch.StartNew();
            while (token.IsCancellationRequested == false)
            {
                ATCommand command;
                if (_commandQueue.TryDequeue(out command))
                {
                    byte[] payload = command.CreatePayload(sequenceNumber);

                    Trace.WriteIf((command is COMWDGCommand) == false, Encoding.ASCII.GetString(payload));

                    _udpClient.Send(payload, payload.Length);
                    sequenceNumber++;
                    swKeepAlive.Restart();
                }
                else if (swKeepAlive.ElapsedMilliseconds > KeepAliveTimeout)
                {
                    _commandQueue.Enqueue(new COMWDGCommand());
                }
                Thread.Sleep(1);
            }
        }

        internal class COMWDGCommand : ATCommand
        {
            protected override string ToAt(int sequenceNumber)
            {
                return string.Format("AT*COMWDG={0}\r", sequenceNumber);
            }
        }
    }
}