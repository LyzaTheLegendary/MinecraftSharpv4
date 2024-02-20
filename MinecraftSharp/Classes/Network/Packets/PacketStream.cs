using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class PacketStream : Stream
    {
        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => data.LongCount();

        public override long Position { get => m_position; set => m_position = value; }
        private long m_position;

        private List<byte> data = new List<byte>();

        public override void Flush()
        {
            int size = data.Count;
            List<byte> content = data;
            data = new List<byte>(size);
            this.WriteLeb32(size);

            data.AddRange(content);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            data.AddRange(buffer);
        }
        public byte[] GetData() => data.ToArray();
    }
}
