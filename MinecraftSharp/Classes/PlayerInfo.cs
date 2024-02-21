using MinecraftSharp.Classes.Utils;
using Newtonsoft.Json;

namespace MinecraftSharp.Classes
{
    public struct Property
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public struct PlayerProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public Property[] Properties { get; set; }
    }
}
