namespace MinecraftSharp.Classes.Utils
{
    public static class UuidHelper
    {
        public static byte[] ToUuid(this uint id){
            byte[] idBytes = BitConverter.GetBytes(id);
            byte[] data = new byte[12];

            return idBytes.Concat<byte>(data).ToArray();
        }
    }
}
