using System.Collections.Concurrent;
using static System.Net.Mime.MediaTypeNames;

namespace MinecraftSharp.Classes.Display
{
    public class Logging
    {
        private static BlockingCollection<string> m_logs = new BlockingCollection<string>();
        private static string m_path = "logs.txt";
        private static object m_lock = new object();
        public static void Initialize()
        {
            Task.Factory.StartNew(() =>
            {
                foreach(string text in m_logs.GetConsumingEnumerable())
                    lock(m_lock)
                        File.AppendAllText(m_path, text);
                
            });
        }
        public static void Write(string text) => m_logs.Add(text);
        
    }
}
