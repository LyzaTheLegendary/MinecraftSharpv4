using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets.Login
{
    public class EncryptionResponse : PacketStream
    {
        public EncryptionResponse(byte[] key, byte[] token)
        {
            this.WriteLeb32(1);
            this.WriteLebString("");
            this.WriteLebPrefix(key);
            this.WriteLebPrefix(token);
            Flush();

        }
    }
}
