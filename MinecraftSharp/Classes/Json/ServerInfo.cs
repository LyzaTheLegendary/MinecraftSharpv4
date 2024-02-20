using Newtonsoft.Json;
namespace Json.ServerInfo
{
    public struct ServerInfo
    {
        public VersionInfo version;
        public PlayersInfo players;
        public DescriptionInfo description;
        public string favicon;
        public bool enforcesSecureChat;
        public bool previewsChat;

        public string GetJson()
            => JsonConvert.SerializeObject(this);
    }

    public struct VersionInfo
    {
        public string name;
        public int protocol;
    }

    public struct PlayersInfo
    {
        public int max;
        public int online;
        public List<PlayerSampleInfo> sample;
    }

    public struct PlayerSampleInfo
    {
        public string name;
        public string id;
    }

    public struct DescriptionInfo
    {
        public string text;
    }
}