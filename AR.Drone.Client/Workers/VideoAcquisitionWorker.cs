using System;
using System.Net.Sockets;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.NativeApi.Video;
using AR.Drone.Client.Video;

namespace AR.Drone.Client.Workers
{
    public class VideoAcquisitionWorker : WorkerBase
    {
        public const int VideoPort = 5555;
        public const int FrameBufferSize = 0x100000;
        public const int NetworkStreamReadSize = 0x10000;

        private readonly ARDroneConfig _config;
        private readonly Action<VideoPacket> _frameAcquired;
        private TcpClient _tcpClient;
        private NetworkStream _videoStream;

        public VideoAcquisitionWorker(ARDroneConfig config, Action<VideoPacket> frameAcquired)
        {
            _config = config;
            _frameAcquired = frameAcquired;
        }

        protected override unsafe void Loop(CancellationToken token)
        {
            using (_tcpClient = new TcpClient(_config.Hostname, VideoPort))
            using (_videoStream = _tcpClient.GetStream())
            {
                VideoPacket packet = new VideoPacket();
                byte[] packetData = null;
                int offset = 0;
                int frameStart = 0;
                int frameEnd = 0;
                var buffer = new byte[FrameBufferSize];
                fixed (byte* pBuffer = &buffer[0])
                    while (token.IsCancellationRequested == false)
                    {
                        offset += _videoStream.Read(buffer, offset, NetworkStreamReadSize);
                        if (packetData == null)
                        {
                            // lookup for a frame start
                            int maxSearchIndex = offset - sizeof (parrot_video_encapsulation_t);
                            for (int i = 0; i < maxSearchIndex; i++)
                            {
                                if (buffer[i] == 'P' &&
                                    buffer[i + 1] == 'a' &&
                                    buffer[i + 2] == 'V' &&
                                    buffer[i + 3] == 'E')
                                {
                                    parrot_video_encapsulation_t pve = *(parrot_video_encapsulation_t*) (pBuffer + i);
                                    packetData = new byte[pve.payload_size];
                                    packet = new VideoPacket
                                        {
                                            Timestamp = DateTime.UtcNow.Ticks,
                                            FrameNumber = pve.frame_number,
                                            Width = pve.display_width,
                                            Height = pve.display_height,
                                            FrameType = Convert(pve.frame_type),
                                            Data = packetData
                                        };
                                    frameStart = i + pve.header_size;
                                    frameEnd = frameStart + packet.Data.Length;
                                    break;
                                }
                            }
                            if (packetData == null)
                            {
                                // frame is not detected
                                offset -= maxSearchIndex;
                                Array.Copy(buffer, maxSearchIndex, buffer, 0, offset);
                            }
                        }
                        if (packetData != null && offset >= frameEnd)
                        {
                            // frame acquired
                            Array.Copy(buffer, frameStart, packetData, 0, packetData.Length);
                            _frameAcquired(packet);

                            // clean up acquired frame
                            packetData = null;
                            offset -= frameEnd;
                            Array.Copy(buffer, frameEnd, buffer, 0, offset);
                        }
                        Thread.Sleep(10);
                    }
            }
        }

        private FrameType Convert(byte frame_type)
        {
            var frameType = (parrot_video_encapsulation_frametypes_t) frame_type;
            switch (frameType)
            {
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_IDR_FRAME:
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_I_FRAME:
                    return FrameType.I;
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_P_FRAME:
                    return FrameType.I;
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_UNKNNOWN:
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_HEADERS:
                    return FrameType.Unknnown;
                default:
                    throw new ArgumentOutOfRangeException("frame_type");
            }
        }
    }
}