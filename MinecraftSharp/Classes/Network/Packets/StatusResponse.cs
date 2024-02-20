using Json.ServerInfo;
using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{

    public class StatusResponse : PacketStream
    {
        public StatusResponse()
        {
            this.WriteLeb32(0); // Packet id
            ServerInfo serverInfo = new ServerInfo
            {
                version = new VersionInfo
                {
                    name = "1.20.4",
                    protocol = 765
                },
                players = new PlayersInfo
                {
                    max = 100,
                    online = 5,
                    sample = new List<PlayerSampleInfo>
                {
                    new PlayerSampleInfo
                    {
                        name = "Nathalie",
                        id = "4566e69f-c907-48ee-8d71-d7ba5aa00d20"
                    }
                }
                },
                description = new DescriptionInfo
                {
                    text = "Sharpcraft server"
                },
                favicon = "data:image/png;base64,<data>",
                enforcesSecureChat = false,
                previewsChat = true
            };
            this.WriteLebString(serverInfo.GetJson());
        }
    }
}
