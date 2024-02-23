using Minecraft.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets.Login
{
    public class FinishConfigResponse : PacketStream
    {
        public FinishConfigResponse()
        {
            this.WriteLeb32(2);
            Flush();
        }
    }
}
