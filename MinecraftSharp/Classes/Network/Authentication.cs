using MinecraftSharp.Classes.Utils;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace MinecraftSharp.Classes.Network
{
    public class Authentication
    {
        private readonly BlockingCollection<TcpClient> m_queue = new BlockingCollection<TcpClient>();
        private readonly TaskPool m_taskPool;
        private readonly Task m_listener;
        public Authentication(int threadCount)
        {
            m_taskPool = new TaskPool(threadCount);
            m_listener = Task.Factory.StartNew(ListenLoop, TaskCreationOptions.LongRunning);
        }
        private void ListenLoop()
        {
            foreach(TcpClient client in m_queue.GetConsumingEnumerable())
                m_taskPool.PendAction(() => Authenticate(client));
        }
        public void Authenticate(TcpClient client)
        {
            using( NetworkStream netStream = client.GetStream() )
            {

            }

        }
    }
}
