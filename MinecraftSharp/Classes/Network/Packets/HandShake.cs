using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{
    public readonly struct HandShake
    {
        public readonly int protocol;
        public readonly string address;
        public readonly ushort port;
        public readonly int nextState;
        public HandShake(Stream stream)
        {
            protocol = stream.ReadLEB32();
            address = stream.ReadLEBStr();
            port = stream.ReadLEB16();
            nextState = stream.ReadByte();
        }
    }
}
