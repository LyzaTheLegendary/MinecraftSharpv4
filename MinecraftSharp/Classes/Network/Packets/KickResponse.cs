using Minecraft.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class KickResponse : PacketStream
    {
        public KickResponse(string reason, int id = 0) {
            this.WriteLeb32(id);
            this.WriteLebString($"{{\r\n    \"text\": \"{reason}\"\r\n}}");
            this.Flush();
        }
    }
}
