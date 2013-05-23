using System;
using System.IO;
using AI.Core.System;
using AR.Drone.Client.Packets;

namespace AR.Drone.Media
{
    public class PacketWriter : DisposableBase
    {
        private readonly FileStream _stream;
        private readonly BinaryWriter _writer;

        public PacketWriter(string path)
        {
            _stream = new FileStream(path, FileMode.Create);
            _writer = new BinaryWriter(_stream);
        }

        public void Write(object packet)
        {
            if (packet == null) throw new NullReferenceException();

            if (packet is NavigationPacket)
            {
                _writer.Write((byte) PacketType.Navigation);

                var navigationPacket = (NavigationPacket) packet;
                BinaryHelper.WriteNavigationPacket(_writer, navigationPacket);
            }
            else if (packet is VideoPacket)
            {
                _writer.Write((byte) PacketType.Video);

                var videoPacket = (VideoPacket) packet;
                BinaryHelper.WriteVideoPacket(_writer, videoPacket);
            }
            else
            {
                string message = string.Format("Not supported packet type - {0}.", packet.GetType().Name);
                throw new NotSupportedException(message);
            }
        }

        protected override void DisposeOverride()
        {
            _writer.Flush();
            _writer.Dispose();
            _stream.Dispose();
        }
    }
}