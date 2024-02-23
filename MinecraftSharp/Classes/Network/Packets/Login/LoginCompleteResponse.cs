using Minecraft.Binary;
using MinecraftSharp.Classes.Utils;

namespace MinecraftSharp.Classes.Network.Packets.Login
{
    public class LoginCompleteResponse : PacketStream
    {
        public LoginCompleteResponse(string username, uint id)
        {
            this.WriteLeb32(2);
            this.WriteLebPrefix(id.ToUuid());
            this.WriteLeb32(0);
            Flush();
        }
    }
}
