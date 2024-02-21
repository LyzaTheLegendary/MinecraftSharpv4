using System.IO;
using System.Text;

namespace Minecraft.Binary
{
    public static class MinecraftEncoding
    {
        public static int ReadLEB32(this Stream stream)
        {
            int value = 0, position = 0, currentByte;

            while (((currentByte = stream.ReadByte()) & 128) != 0)
            {
                if (currentByte < 0) throw new EndOfStreamException();
                value |= (currentByte & 127) << position;
                position += 7;
            }

            return value |= (currentByte & 127) << position;
        }
        public static ushort ReadLEB16(this Stream stream)
        {
            byte[] uint16Bytes = new byte[sizeof(ushort)];
            stream.ReadExactly(uint16Bytes, 0, sizeof(ushort));
            return (ushort)(uint16Bytes[1] | uint16Bytes[0] << 8);
        }
        public static string ReadLEBStr(this Stream stream)
        {
            int len = stream.ReadLEB32();
            byte[] buff = new byte[len];
            stream.ReadExactly(buff, 0, len);

            return Encoding.UTF8.GetString(buff);
        }
        public static byte[] ReadLebPrefix(this Stream stream)
        {
            int len = stream.ReadLEB32();
            byte[] bytes = new byte[len];
            stream.Read(bytes, 0, len);

            return bytes;
        }
        public static void WriteLeb16(this Stream stream, ushort value)
        {
            byte[] data = BitConverter.GetBytes(value);
            stream.Write(BitConverter.GetBytes((ushort)(data[1] | data[0] << 8)), 0, data.Length);
        }
        public static void WriteLeb32(this Stream stream, int value)
        {
            do
            {
                byte currentByte = (byte)(value & 0x7F);  //LSB, 127
                value >>= 7;                              //VLQ, 7

                if (value > 0)
                    currentByte |= 0x80;                    //MSB, 128

                stream.WriteByte(currentByte);
            } while (value > 0);
        }
        public static void WriteLebString(this Stream stream, string value)
        {
            stream.WriteLeb32(value.Length);
            byte[] strBytes = Encoding.UTF8.GetBytes(value);
            stream.Write(strBytes, 0, strBytes.Length); // We check the size of the array as it's utf8
        }
        public static void WriteLebPrefix(this Stream stream, byte[] data)
        {
            stream.WriteLeb32(data.Length);
            stream.Write(data, 0, data.Length);
        }
    }
}