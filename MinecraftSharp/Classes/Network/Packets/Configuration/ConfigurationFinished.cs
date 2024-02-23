using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets.Configuration
{
    public struct ConfigurationFinished : IPacket
    {
        public ConfigurationFinished(Packet packet)
        {
            Id = packet.m_id;
        }
        public int Id { get; }
    }
}
