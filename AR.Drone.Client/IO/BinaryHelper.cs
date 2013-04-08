using System.IO;
using AR.Drone.Client.Navigation;
using AR.Drone.Client.Video;

namespace AR.Drone.Client.IO
{
    public static class BinaryHelper
    {
        public static void WritePacket(this BinaryWriter writer, NavigationPacket packet)
        {
            writer.Write(packet.Timestamp);
            writer.Write(packet.Data.Length);
            writer.Write(packet.Data);
        }

        public static void WritePacket(this BinaryWriter writer, VideoPacket packet)
        {
            writer.Write(packet.Timestamp);
            writer.Write(packet.FrameNumber);
            writer.Write(packet.Height);
            writer.Write(packet.Width);
            writer.Write((byte) packet.FrameType);
            writer.Write(packet.Data.Length);
            writer.Write(packet.Data);
        }

        public static NavigationPacket ReadNavigationPacket(this BinaryReader reader)
        {
            var packet = new NavigationPacket();
            packet.Timestamp = reader.ReadInt64();
            int dataSize = reader.ReadInt32();
            packet.Data = reader.ReadBytes(dataSize);
            return packet;
        }

        public static VideoPacket ReadVideoPacket(this BinaryReader reader)
        {
            var packet = new VideoPacket();
            packet.Timestamp = reader.ReadInt64();
            packet.FrameNumber = reader.ReadUInt32();
            packet.Height = reader.ReadUInt16();
            packet.Width = reader.ReadUInt16();
            packet.FrameType = (FrameType) reader.ReadByte();
            int dataSize = reader.ReadInt32();
            packet.Data = reader.ReadBytes(dataSize);
            return packet;
        }
    }
}