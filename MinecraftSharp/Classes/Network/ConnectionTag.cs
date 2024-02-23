using System.Net.Sockets;
using MinecraftSharp.Classes.Utils;

namespace MinecraftSharp.Classes.Network
{
    public class ConnectionTag : Identifiable
    {
        private Socket m_socket;
        private NetworkStream m_netStream;
        private mAddr m_address;
        public ConnectionTag(Socket socket, Id id) : base(id)
        {
            m_socket = socket;
            m_netStream = new NetworkStream(m_socket, false);
            m_address = new mAddr(socket.RemoteEndPoint!.ToString()!); // Is not empty as it's incoming
        }
        public mAddr GetAddr() => m_address;
        public Stream GetStream() => m_netStream;
        public Socket GetSock() => m_socket;
        public void Drop()
        {
            m_socket.Close();
            Id.Dispose();
        }
    }
}
