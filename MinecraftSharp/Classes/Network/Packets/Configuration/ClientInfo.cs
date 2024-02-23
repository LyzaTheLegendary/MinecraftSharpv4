using Minecraft.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftSharp.Classes.Network.Packets.Configuration
{
    public struct ClientInfo : IPacket
    {
        public readonly string m_locale;
        public readonly int m_renderDist;
        public readonly int m_chatmode; // 0 enabled, 1 commands only and 2 hidden
        public readonly byte m_chatcolors;
        public readonly byte m_skinBitmap;
        public readonly int m_mainHand;
        public readonly byte m_textFiltering;
        public readonly byte m_serverListed;
        public ClientInfo(Packet packet)
        {
            Id = packet.m_id;
            m_locale = packet.ReadLEBStr();
            m_renderDist = packet.ReadByte() * 16;
            m_chatmode = packet.ReadLEB32();
            m_chatcolors = (byte)packet.ReadByte();
            m_skinBitmap = (byte)packet.ReadByte();
            m_mainHand = packet.ReadLEB32();
            m_textFiltering = (byte)packet.ReadByte();
            m_serverListed = (byte)packet.ReadByte();
        }

        public int Id { get; }
    }
}
