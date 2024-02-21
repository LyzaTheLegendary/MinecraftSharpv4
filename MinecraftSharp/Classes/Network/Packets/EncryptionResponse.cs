using Minecraft.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class EncryptionResponse : PacketStream
    {
        public EncryptionResponse(byte[] key, byte[] token) {
            this.WriteLeb32(1);
            this.WriteLebString("");
            this.WriteLebPrefix(key);
            this.WriteLebPrefix(token);
            this.Flush();

        }
    }
}
