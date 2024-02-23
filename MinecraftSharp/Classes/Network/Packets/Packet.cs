using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class Packet : MemoryStream
    {
        public readonly int m_id;
        public Packet(byte[] buffer) : base(buffer)
        {
            m_id = this.ReadLEB32();
        }
    }
}
