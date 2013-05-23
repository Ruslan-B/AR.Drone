using System;
using System.IO;
using AI.Core.System;

namespace AR.Drone.Media
{
    public class PacketReader : DisposableBase
    {
        private readonly BinaryReader _reader;
        private readonly FileStream _stream;

        public PacketReader(string path)
        {
            _stream = new FileStream(path, FileMode.Open);
            _reader = new BinaryReader(_stream);
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
            _stream.Dispose();
        }
    }
}