
using System.Security.Cryptography;

namespace MinecraftSharp.Classes.Network
{
    public class MinecraftConnection
    {
        private readonly ConnectionTag m_tag;
        private readonly Aes m_aes;
        public MinecraftConnection(ConnectionTag tag, Aes aes)
        {
            m_tag = tag;
            m_aes = aes; 
        }
    }
}
