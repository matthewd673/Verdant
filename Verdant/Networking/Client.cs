using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

namespace Verdant.Networking
{
    internal class Client
    {

        NetworkManager manager;

        string serverIp;
        int serverPort = 30508;

        internal int BytesRecieved { get; private set; }
        internal int BytesSent { get; private set; }
        internal bool ConnectionConfirmed { get; private set; }

        UdpClient client;

        public Client(NetworkManager manager)
        {
            this.manager = manager;
        }

        public void Connect(string ip)
        {
            serverIp = ip;
            client = new UdpClient(30509);
            Thread joinThread = new Thread(new ThreadStart(Join));
            joinThread.Start();
        }

        void Join()
        {
            //client = new UdpClient(30509);
            client.Connect(serverIp, serverPort);

            byte[] joinData = Server.BuildMessageBytes(NetworkManager.MessageType.Join);
            client.Send(joinData, joinData.Length);
            BytesSent += joinData.Length;

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];

            while (true)
            {
                data = client.Receive(ref sender);
                BytesRecieved += data.Length;

                Server.Message message = Server.ParseIncomingMessage(data);

                //special case, since data may not even have 4 bytes
                if (message.Type == NetworkManager.MessageType.Ping)
                {
                    //todo
                    continue;
                }
                byte[] inData = message.Data.Skip<byte>(4).ToArray();

                switch (message.Type)
                {
                    case NetworkManager.MessageType.JoinConfirm:
                        ConnectionConfirmed = true;
                        manager.OnClientConnectionConfirmed();
                        break;
                    case NetworkManager.MessageType.CreateEntity:
                        NetworkEntity createdEntity = manager.DeserializeNetworkEntity(inData);
                        createdEntity.Managed = false;
                        manager.AddEntity(createdEntity);
                        break;
                    case NetworkManager.MessageType.RemoveEntity:
                        //todo
                        break;
                    default:
                        //todo
                        break;
                }
            }
        }

        public void Send(Server.Message message)
        {
            byte[] outData = Server.BuildMessageBytes(message.Type, message.Data);
            client.Send(outData, outData.Length);
            BytesSent += outData.Length;
        }

    }
}
