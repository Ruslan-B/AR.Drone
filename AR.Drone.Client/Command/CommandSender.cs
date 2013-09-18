using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AR.Drone.Infrastructure;

namespace AR.Drone.Client.Command
{
    public class CommandSender : WorkerBase
    {
        public const int CommandPort = 5556;
        public const int KeepAliveTimeout = 20;

        private readonly ConcurrentQueue<AtCommand> _commandQueue;
        private readonly NetworkConfiguration _configuration;

        public CommandSender(NetworkConfiguration configuration, ConcurrentQueue<AtCommand> commandQueue)
        {
            _configuration = configuration;
            _commandQueue = commandQueue;
        }

        protected override void Loop(CancellationToken token)
        {
            int sequenceNumber = 1;

            using (var udpClient = new UdpClient(CommandPort))
            {
                udpClient.Connect(_configuration.DroneHostname, CommandPort);

                byte[] firstMessage = BitConverter.GetBytes(1);
                udpClient.Send(firstMessage, firstMessage.Length);

                _commandQueue.Enqueue(ComWdgCommand.Default);
                Stopwatch swKeepAlive = Stopwatch.StartNew();

                while (token.IsCancellationRequested == false)
                {
                    if (swKeepAlive.ElapsedMilliseconds > KeepAliveTimeout)
                    {
                        _commandQueue.Enqueue(ComWdgCommand.Default);
                        swKeepAlive.Restart();
                    }

                    if (_commandQueue.Count > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            AtCommand command;
                            while (_commandQueue.TryDequeue(out command))
                            {
                                byte[] payload = command.CreatePayload(sequenceNumber);
                                Trace.WriteIf((command is ComWdgCommand) == false, Encoding.ASCII.GetString(payload));
                                ms.Write(payload, 0, payload.Length);
                                sequenceNumber++;
                            }

                            byte[] fullPayload = ms.ToArray();
                            udpClient.Send(fullPayload, fullPayload.Length);
                        }
                    }

                    Thread.Sleep(5);
                }
            }
        }
    }
}