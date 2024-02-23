using Cryptography;
using MinecraftSharp.Classes.Utils;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace MinecraftSharp.Classes.Cache
{
    public class PlayerCache
    {
        private readonly string m_path;
        private List<string> m_uidList = new();
        public PlayerCache(string path)
        {
            m_path = path;
            if(!Directory.Exists(m_path))
                Directory.CreateDirectory(m_path);
            foreach (string uuid in Directory.GetFileSystemEntries(m_path))
                m_uidList.Add(uuid);
        }
        public PlayerProfile GetCache(string uuid, string username, string serverId)
        {
            if(m_uidList.Contains(uuid))
                return JsonConvert.DeserializeObject<PlayerProfile>(File.ReadAllText(Path.Combine(m_path,uuid)));
            // getrequest
            username = HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(username));
            serverId = HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(serverId));

            return HttpHelper.Get<PlayerProfile>($"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={username}&serverId={serverId}");
        }
        public void AddCache(PlayerProfile info)
        {
            lock (m_uidList)
            {
                File.WriteAllText(Path.Combine(m_path, info.Id), JsonConvert.SerializeObject(info));
                m_uidList.Add(info.Id);
            }
        }
    }
}
