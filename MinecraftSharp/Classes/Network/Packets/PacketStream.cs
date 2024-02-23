using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class PacketStream : Stream
    {
        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => m_data.LongCount();

        public override long Position { get => m_position; set => m_position = value; }
        private long m_position = 0;

        protected List<byte> m_data = new();

        public override void Flush()
        {
            int size = m_data.Count;
            List<byte> content = m_data;
            m_data = new List<byte>(size);
            this.WriteLeb32(size);

            m_data.AddRange(content);
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
            m_data.AddRange(buffer);
        }
        public virtual byte[] GetData() => m_data.ToArray();
    }
}
