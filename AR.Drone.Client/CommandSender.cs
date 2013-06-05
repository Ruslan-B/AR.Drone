using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AR.Drone.Infrastructure;
using AR.Drone.Client.Commands;
using AR.Drone.Client.Configuration;

namespace AR.Drone.Client
{
    public class CommandSender : WorkerBase
    {
        public const int CommandPort = 5556;
        public const int KeepAliveTimeout = 50;
        private readonly ConcurrentQueue<ATCommand> _commandQueue;
        private readonly INetworkConfiguration _configuration;

        public CommandSender(INetworkConfiguration configuration, ConcurrentQueue<ATCommand> commandQueue)
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

                Stopwatch swKeepAlive = Stopwatch.StartNew();
                while (token.IsCancellationRequested == false)
                {
                    ATCommand command;
                    if (_commandQueue.TryDequeue(out command))
                    {
                        byte[] payload = command.CreatePayload(sequenceNumber);

                        Trace.WriteIf((command is ComWdgCommand) == false, Encoding.ASCII.GetString(payload));

                        udpClient.Send(payload, payload.Length);
                        sequenceNumber++;
                        swKeepAlive.Restart();
                    }
                    else if (swKeepAlive.ElapsedMilliseconds > KeepAliveTimeout)
                    {
                        _commandQueue.Enqueue(new ComWdgCommand());
                    }
                    Thread.Sleep(10);
                }
            }
        }
    }
}