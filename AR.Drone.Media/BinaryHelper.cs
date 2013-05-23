using System.IO;
using AR.Drone.Data;

namespace AR.Drone.Media
{
    public static class BinaryHelper
    {
        public static void WriteNavigationPacket(BinaryWriter writer, NavigationPacket packet)
        {
            writer.Write(packet.Timestamp);
            writer.Write(packet.Data.Length);
            writer.Write(packet.Data);
        }

        public static NavigationPacket ReadNavigationPacket(BinaryReader reader)
        {
            var packet = new NavigationPacket();
            packet.Timestamp = reader.ReadInt64();
            int dataSize = reader.ReadInt32();
            packet.Data = reader.ReadBytes(dataSize);
            return packet;
        }
        
        public static void WriteVideoPacket(BinaryWriter writer, VideoPacket packet)
        {
            writer.Write(packet.Timestamp);
            writer.Write(packet.FrameNumber);
            writer.Write(packet.Height);
            writer.Write(packet.Width);
            writer.Write((byte) packet.FrameType);
            writer.Write(packet.Data.Length);
            writer.Write(packet.Data);
        }

        public static VideoPacket ReadVideoPacket(BinaryReader reader)
        {
            var packet = new VideoPacket();
            packet.Timestamp = reader.ReadInt64();
            packet.FrameNumber = reader.ReadUInt32();
            packet.Height = reader.ReadUInt16();
            packet.Width = reader.ReadUInt16();
            packet.FrameType = (VideoFrameType) reader.ReadByte();
            int dataSize = reader.ReadInt32();
            packet.Data = reader.ReadBytes(dataSize);
            return packet;
        }
    }
}