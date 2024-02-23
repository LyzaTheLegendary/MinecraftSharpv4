
using Minecraft.Binary;
using MinecraftSharp.Classes.Network.Packets;
using MinecraftSharp.Classes.Utils;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace MinecraftSharp.Classes.Network
{
    public class McClient : Identifiable
    {
        private readonly mAddr m_address;
        private readonly Socket m_socket;
        private readonly Aes m_aes;
        private readonly CryptoStream m_rStream;
        public McClient(ConnectionTag tag, Aes aes) : base(tag.Id)
        {
            m_address = tag.GetAddr();
            m_socket = tag.GetSock();
            m_aes    = aes;
            m_rStream = new CryptoStream(new NetworkStream(m_socket, true), m_aes.CreateDecryptor(), CryptoStreamMode.Read);
        }
        public Stream GetReadingStream() 
            => m_rStream;
        
        public void Disconnect(string reason)
            => Write(new KickResponse(reason, 0).GetData());
        
        public void Write(byte[] data) {
            using (var cs = new CryptoStream(new NetworkStream(m_socket, false), m_aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data);
                cs.FlushFinalBlock();
            }
        }
        public Packet GetPacket()
        {
            return m_rStream.GetPacket();

        }
    }
}
