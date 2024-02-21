using Cryptography;
using Minecraft.Binary;
using MinecraftSharp.Classes.Cache;
using MinecraftSharp.Classes.Display;
using MinecraftSharp.Classes.Json;
using MinecraftSharp.Classes.Network.Packets;
using MinecraftSharp.Classes.Utils;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Collections.Concurrent;
using System.Data;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

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

                byte[]? aesKey = GetKeyAndPrefix(netStream);

                if(aesKey == null) // if null the RSA failed
                {
                    ConsoleHelper.WriteInfo($"Rsa test failed Autentication.GetKeyAndPrefx()");
                    conn.Drop();
                    return;
                }

                Aes aes = Aes.Create();
                #region AES settings
                aes.Padding = PaddingMode.None;
                aes.Mode = CipherMode.CFB;
                aes.KeySize = 128;
                aes.FeedbackSize = 8;
                aes.Key = aesKey;
                aes.IV = (byte[])aesKey.Clone();
                #endregion

                //TODO: set compression request before encryption

                MinecraftConnection l_conn = new(conn, aes); // send to server


                //AuthenticationInfo info = new() // Can't be bothered to implement this
                //{
                //    accessToken = Encoding.ASCII.GetString(aesKey),
                //    selectedProfile = Convert.ToHexString(uuid).ToLower(),//Encoding.UTF8.GetString(uuid).Replace("-", string.Empty),
                //    serverId = MinecraftSha.MinecraftShaDigest("")
                //};
                //string data = JsonConvert.SerializeObject(info);
                //ErrorResponse response = HttpHelper.Post<AuthenticationInfo>("https://sessionserver.mojang.com/session/minecraft/join", info);

                //m_playerList[(uint)client.Id] = pinfo;
            }

        }
        private bool ValidateUuid(string username, byte[] uuidBytes)
        {
            ApiResonse response = HttpHelper.Get<ApiResonse>("https://api.mojang.com/users/profiles/minecraft/" + username);
            string uuid = Convert.ToHexString(uuidBytes).ToLower();

            return uuid == response.id;
        }
        private byte[]? GetKeyAndPrefix(Stream netStream)
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
                return null;
            

            return aesKey;
        }

    }
}
