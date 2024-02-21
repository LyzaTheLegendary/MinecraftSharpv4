namespace MinecraftSharp.Classes.Cache
{
    public class PlayerCache
    {
        private readonly string m_path;
        private List<string> m_uidList;
        public PlayerCache(string path)
        {
            m_path = path;
            if(!Directory.Exists(m_path))
                Directory.CreateDirectory(m_path);
        }
        public void AddCache(PlayerProfile info)
        {
            
        }
    }
}
