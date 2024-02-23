using Minecraft.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets.Configuration
{
    public struct PluginMessage : IPacket
    {
        public readonly string m_namespace;
        public readonly string m_value;
        public PluginMessage(Packet packet)
        {
            m_namespace = packet.ReadLEBStr();
            m_value = packet.ReadLEBStr();
        }
        public int Id { get; }
    }
}
