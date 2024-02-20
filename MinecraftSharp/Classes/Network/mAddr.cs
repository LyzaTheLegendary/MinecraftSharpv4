using System.Net;

namespace MinecraftSharp.Classes.Network
{
    public struct mAddr
    {
        private readonly string m_address;
        private readonly int m_port;
        public mAddr(string host)
        {
            if (!host.Contains(":"))
                throw new ArgumentException("Invalid host address, Does not contain the seperator for the ip and port!");

            string[] addrInfo = host.Split(':');

            m_address = addrInfo[0];
            m_port = int.Parse(addrInfo[1]);
        }
        public IPAddress Convert() => IPAddress.Parse(m_address);
        public string Address { get { return m_address; } }
        public int Port { get { return m_port; } }
        public override string ToString() => $"{m_address}:{m_port}";
    }
}
