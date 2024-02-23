using Cryptography;
using Minecraft.Binary;
using MinecraftSharp.Classes.Cache;
using MinecraftSharp.Classes.Display;
using MinecraftSharp.Classes.Json;
using MinecraftSharp.Classes.Network.Packets;
using MinecraftSharp.Classes.Network.Packets.Configuration;
using MinecraftSharp.Classes.Network.Packets.Login;
using MinecraftSharp.Classes.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MinecraftSharp.Classes.Network
{
    public class Authentication
    {
        private readonly BlockingCollection<ConnectionTag> m_queue = new BlockingCollection<ConnectionTag>();
        private readonly PlayerCache m_playerCache;
        private readonly TaskPool m_taskPool;
        private readonly Task m_listener;
        public Authentication(int threadCount)
        {
            m_taskPool = new TaskPool(threadCount);
            m_playerCache = new PlayerCache("cache/players");
            m_listener = Task.Factory.StartNew(ListenLoop, TaskCreationOptions.LongRunning);
        }
        private void ListenLoop()
        {
            foreach(ConnectionTag conn in m_queue.GetConsumingEnumerable())
                m_taskPool.PendAction(() => Authenticate(conn));
        }
        public void PendAuthentication(ConnectionTag conn)
            => m_taskPool.PendAction(() => Authenticate(conn));

        private void Authenticate(ConnectionTag conn)
        {
            using( Stream netStream = conn.GetStream() )
            {
                
                _ = netStream.ReadLEB32(); // size
                _ = netStream.ReadLEB32(); // Id

                string username = netStream.ReadLEBStr();

                byte[] uuidBytes = new byte[16];
                netStream.Read(uuidBytes, 0, 16);
                
                if(!ValidateUuid(username, uuidBytes))
                {
                    netStream.Write(new KickResponse("Reauthenticate, Invalid credentials!").GetData());
                    Thread.Sleep(100);
                    conn.Drop();
                    return;
                }



                //_ = netStream.ReadLEB32();// size
                //_ = netStream.ReadLEB32();// id

                //netStream.WriteLeb32(1);
                //netStream.WriteLeb32(0);
                //return;
                netStream.Write(new LoginCompleteResponse(username, (uint)conn.Id).GetData());

                netStream.ReadByte();
                netStream.ReadByte();

                PluginMessage pluginMsg = new(netStream.GetPacket());
                ClientInfo clientInfo = new(netStream.GetPacket());

                netStream.Write(new FinishConfigResponse().GetData());

                ConfigurationFinished signal = new(netStream.GetPacket());

                return; // Use bouncy castle for encryption ( I hate cryptography! )
                //(byte[]? aesKey, byte[] secret) = GetKeyAndPrefix(netStream);

                //if(aesKey == null) // if null the RSA failed
                //{
                //    ConsoleHelper.WriteInfo($"Rsa test failed Autentication.GetKeyAndPrefx()");
                //    conn.Drop();
                //    return;
                //}
               
                ////TODO do cool shit like player caching but can't make request to mojang?

                //Aes aes = Aes.Create();
                //#region AES settings
                //aes.Padding = PaddingMode.None;
                //aes.Mode = CipherMode.CFB;
                //aes.KeySize = 128;
                //aes.FeedbackSize = 8;
                //aes.Key = aesKey;
                //aes.IV = (byte[])aesKey.Clone();
                //#endregion

                ////TODO: set compression request before encryption

                //McClient l_conn = new(conn, aes); // send to server
                //l_conn.Write(new LoginCompleteResponse(username, (uint)conn.Id).GetData());

                //Stream stream = l_conn.GetReadingStream();

                //stream.ReadByte();
                //stream.ReadByte();


                //Packet packet = l_conn.GetPacket();
                //string channel = packet.ReadLEBStr();
                //channel += packet.ReadLEBStr();
                //byte[] data = packet.ReadLebPrefix();
                ////string str1 = stream.ReadLEBStr();
                ////string str2 = stream.ReadLEBStr();

                //Packet packet2 = l_conn.GetPacket();
                


                //l_conn.Write(new FinishConfigResponse().GetData());

                
                //int integer2 = stream.ReadLEB32();


            }

        }
        private bool ValidateUuid(string username, byte[] uuidBytes)
        {
            ApiResonse response = HttpHelper.Get<ApiResonse>("https://api.mojang.com/users/profiles/minecraft/" + username);
            string uuid = Convert.ToHexString(uuidBytes).ToLower();

            return uuid == response.id;
        }
        private (byte[]?, byte[]) GetKeyAndPrefix(Stream netStream)
        {
            RSA rsa = RSA.Create();
            byte[] token = new byte[4];
            new Random().NextBytes(token);

            netStream.Write(new EncryptionResponse(rsa.ExportSubjectPublicKeyInfo(), token).GetData());

            _ = netStream.ReadLEB32(); // size
            _ = netStream.ReadLEB32(); // Id

            byte[] aesKey = rsa.Decrypt(netStream.ReadLebPrefix(), RSAEncryptionPadding.Pkcs1);
            byte[] decryptedToken = rsa.Decrypt(netStream.ReadLebPrefix(), RSAEncryptionPadding.Pkcs1);

            if(Convert.ToHexString(token) != Convert.ToHexString(decryptedToken))
                return (null,decryptedToken);
            

            return (aesKey, decryptedToken);
        }

    }
}
