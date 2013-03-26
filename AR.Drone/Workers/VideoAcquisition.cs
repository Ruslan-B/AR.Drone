using System;
using System.Net.Sockets;
using System.Threading;
using AR.Drone.Helpers;
using AR.Drone.Api.Video;
using AR.Drone.Common;
using AR.Drone.NativeApi.Video;

namespace AR.Drone.Workers
{
    public class VideoAcquisition : WorkerBase
    {
        public const int VideoPort = 5555;
        public const int FrameBufferSize = 0x100000;
        public const int NetworkStreamReadSize = 0x10000;

        private readonly DroneConfig _config;
        private readonly Action<VideoPacket> _frameAcquired;
        private TcpClient _tcpClient;
        private NetworkStream _videoStream;

        public VideoAcquisition(DroneConfig config, Action<VideoPacket> frameAcquired)
        {
            _config = config;
            _frameAcquired = frameAcquired;
        }

        protected override unsafe void Loop(CancellationToken token)
        {
            using (_tcpClient = new TcpClient(_config.Hostname, VideoPort))
            using (_videoStream = _tcpClient.GetStream())
            {
                VideoPacket packet = null;
                int offset = 0;
                int frameStart = 0;
                int frameEnd = 0;
                var buffer = new byte[FrameBufferSize];
                fixed (byte* pBuffer = &buffer[0])
                    while (token.IsCancellationRequested == false)
                    {
                        offset += _videoStream.Read(buffer, offset, NetworkStreamReadSize);
                        if (packet == null)
                        {
                            // lookup for a frame start
                            int maxSearchIndex = offset - sizeof (VideoEncapsulation);
                            for (int i = 0; i < maxSearchIndex; i++)
                            {
                                if (buffer[i] == 'P' &&
                                    buffer[i + 1] == 'a' &&
                                    buffer[i + 2] == 'V' &&
                                    buffer[i + 3] == 'E')
                                {
                                    VideoEncapsulation pve = *(VideoEncapsulation*) (pBuffer + i);
                                    packet = new VideoPacket
                                        {
                                            Timestamp = DateTime.UtcNow,
                                            Width = pve.display_width,
                                            Height = pve.display_height,
                                            FrameType = VideoHelper.Convert(pve.frame_type),
                                            Data = new byte[pve.payload_size]
                                        };
                                    frameStart = i + pve.header_size;
                                    frameEnd = frameStart + packet.Data.Length;
                                    break;
                                }
                            }
                            if (packet == null)
                            {
                                // frame is not detected
                                offset -= maxSearchIndex;
                                Array.Copy(buffer, maxSearchIndex, buffer, 0, offset);
                            }
                        }
                        if (packet != null && offset >= frameEnd)
                        {
                            // frame acquired
                            Array.Copy(buffer, frameStart, packet.Data, 0, packet.Data.Length);
                            _frameAcquired(packet);

                            // clean up acquired frame
                            packet = null;
                            offset -= frameEnd;
                            Array.Copy(buffer, frameEnd, buffer, 0, offset);
                        }
                        Thread.Sleep(10);
                    }
            }
        }
    }
}