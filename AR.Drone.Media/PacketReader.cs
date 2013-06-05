using System;
using System.IO;
using AR.Drone.Infrastructure;

namespace AR.Drone.Media
{
    public class PacketReader : DisposableBase
    {
        private readonly BinaryReader _reader;

        public PacketReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        public object Read()
        {
            try
            {
                var packetType = (PacketType) _reader.ReadByte();

                switch (packetType)
                {
                    case PacketType.Navigation:
                        return BinaryHelper.ReadNavigationPacket(_reader);
                    case PacketType.Video:
                        return BinaryHelper.ReadVideoPacket(_reader);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (EndOfStreamException)
            {
                return null;
            }
        }

        protected override void DisposeOverride()
        {
            _reader.Dispose();
        }
    }
}