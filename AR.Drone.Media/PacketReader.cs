using System;
using System.IO;
using AR.Drone.Data;

namespace AR.Drone.Media
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(Stream stream) : base(stream)
        {
        }

        public PacketType ReadPacketType()
        {
            return (PacketType) ReadByte();
        }

        public NavigationPacket ReadNavigationPacket()
        {
            var packet = new NavigationPacket();
            packet.Timestamp = ReadInt64();
            int dataSize = ReadInt32();
            packet.Data = ReadBytes(dataSize);
            return packet;
        }

        public VideoPacket ReadVideoPacket()
        {
            var packet = new VideoPacket();
            packet.Timestamp = ReadInt64();
            packet.FrameNumber = ReadUInt32();
            packet.Height = ReadUInt16();
            packet.Width = ReadUInt16();
            packet.FrameType = (VideoFrameType) ReadByte();
            int dataSize = ReadInt32();
            packet.Data = ReadBytes(dataSize);
            return packet;
        }

        public object ReadPacket()
        {
            try
            {
                PacketType packetType = ReadPacketType();
                switch (packetType)
                {
                    case PacketType.Navigation:
                        return ReadNavigationPacket();
                    case PacketType.Video:
                        return ReadVideoPacket();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (EndOfStreamException)
            {
                return null;
            }
        }
    }
}