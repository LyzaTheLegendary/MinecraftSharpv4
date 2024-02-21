using Minecraft.Binary;
using MinecraftSharp.Classes.Display;
using MinecraftSharp.Classes.Network.Packets;
using MinecraftSharp.Classes.Utils;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace MinecraftSharp.Classes.Network
{
    public static class IOManager
    {
        private static Socket m_listener;
        private static mAddr m_addr;

        private static Task m_listenerTask;
        private static Authentication auth;

        private static IdPool m_pool = new();
        //private static IdList m_connections = new();
        private static ConcurrentDictionary<uint, ConnectionTag> m_connections = new();
        private static CancellationTokenSource m_cts = new();
        private static bool m_accepting = true;

        public static void Initialize(mAddr host, int threadCount)
        {
            m_addr = host;
            auth = new Authentication(threadCount);
            //m_taskPool = new TaskPool(threadCount);
            IPEndPoint ep = new IPEndPoint(host.Convert(), host.Port);

            m_listener = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(ep);
            m_listener.Listen(threadCount);

            ConsoleHelper.WriteInfo($"Starting network with {threadCount} threads!");
            m_listenerTask = Task.Factory.StartNew(() =>
            {
                ConsoleHelper.WriteInfo($"Accepting connections on: {m_addr}");
                while (m_accepting)
                {
                    Socket remoteSock = m_listener.Accept();
                    int maxPing = (int)Settings.GetValue("server.maxping");

                    remoteSock.ReceiveTimeout = maxPing;
                    remoteSock.SendTimeout = maxPing;

                    //  instead of using a taskpool to acccept sockets we could use it to authenticate users
                    Accept(remoteSock);
                    //m_taskPool.PendAction(() => Accept(remoteSock)); 
                }
            }, m_cts.Token);

        }
        public static void AddConnection(ConnectionTag conn) 
            => m_connections[(uint)conn.Id] = conn;
        public static void DelConnection(uint id)
            => m_connections.Remove(id, out _);
        private static void Accept(Socket remoteSock)
        {
            using (NetworkStream netStream = new NetworkStream(remoteSock, false))
            {

                //  We don't care about all this as we handle the client first in it's own Async context
                _ = netStream.ReadLEB32(); // Size
                _ = netStream.ReadLEB32(); // Id

                HandShake packet = new(netStream);

                if (packet.nextState == 1) // status update
                {
                    //  MC protocol surely isn't ill
                    netStream.ReadByte();
                    netStream.ReadByte();

                    netStream.Write(new StatusResponse().GetData());

                    byte[] bytes = new byte[10];
                    netStream.Read(bytes);
                    netStream.Write(bytes);

                    remoteSock.Close();
                    return;
                }

                else if (packet.nextState == 2) // login
                {
                    if (packet.protocol != (int)Settings.GetValue("server.protocol"))
                    {
                        netStream.Write(new KickResponse("Not implemented yet!").GetData());
                        Thread.Sleep(100);
                        remoteSock.Close();
                        return;
                    }
                    
                }

                auth.PendAuthentication(new ConnectionTag(remoteSock, m_pool.GetId()));
            }

        }
    }
}
