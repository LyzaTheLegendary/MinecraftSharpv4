using Minecraft.Binary;

namespace MinecraftSharp.Classes.Network.Packets
{
    public class StatusResponse : PacketStream
    {
        public StatusResponse()
        {
            this.WriteLeb32(0); // Packet id
            this.WriteLebString("{\r\n    \"version\": {\r\n        \"name\": \"SharpCraft\",\r\n        \"protocol\": 765\r\n    },\r\n    \"players\": {\r\n        \"max\": 100,\r\n        \"online\": 5,\r\n        \"sample\": [\r\n            {\r\n                \"name\": \"thinkofdeath\",\r\n                \"id\": \"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"\r\n            }\r\n        ]\r\n    },\r\n    \"description\": {\r\n        \"text\": \"Hello world\"\r\n    },\r\n    \"favicon\": \"data:image/png;base64,<data>\",\r\n    \"enforcesSecureChat\": true,\r\n    \"previewsChat\": true\r\n}");
        }
    }
}
