using System;
using System.IO;
using AR.Drone.Data;

namespace AR.Drone.Media
{
    public class PacketWriter : BinaryWriter
    {
        public PacketWriter(Stream stream) : base(stream)
        {
        }

        public void WritePacket(object packet)
        {
            if (packet == null) throw new NullReferenceException();

            if (packet is NavigationPacket)
            {
                Write(PacketType.Navigation);

                var navigationPacket = (NavigationPacket) packet;
                Write(navigationPacket);
            }
            else if (packet is VideoPacket)
            {
                Write((byte) PacketType.Video);

                var videoPacket = (VideoPacket) packet;
                Write(videoPacket);
            }
            else
            {
                string message = string.Format("Not supported packet type - {0}.", packet.GetType().Name);
                throw new NotSupportedException(message);
            }
        }

        private void Write(PacketType packetType)
        {
            Write((byte) packetType);
        }

        public void Write(NavigationPacket packet)
        {
            Write(packet.Timestamp);
            Write(packet.Data.Length);
            Write(packet.Data);
        }

        public void Write(VideoPacket packet)
        {
            Write(packet.Timestamp);
            Write(packet.FrameNumber);
            Write(packet.Height);
            Write(packet.Width);
            Write((byte) packet.FrameType);
            Write(packet.Data.Length);
            Write(packet.Data);
        }
    }
}