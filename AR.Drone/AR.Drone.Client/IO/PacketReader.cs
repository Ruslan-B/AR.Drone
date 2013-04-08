using System;
using System.IO;
using AR.Drone.Common;

namespace AR.Drone.IO
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
                        return _reader.ReadNavigationPacket();
                    case PacketType.Video:
                        return _reader.ReadVideoPacket();
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