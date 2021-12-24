using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;
using IsoEngine.Debugging;

namespace IsoEngine.Networking
{
    internal class Server
    {

        NetworkManager manager;

        internal List<IPEndPoint> Connected { get; private set; } = new List<IPEndPoint>();
        internal int BytesRecieved { get; private set; }
        internal int BytesSent { get; private set; }

        UdpClient client;

        public Server(NetworkManager manager)
        {
            this.manager = manager;
        }

        public void Start()
        {
            client = new UdpClient(30508);
            Thread listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        void Listen()
        {
            //UdpClient client = new UdpClient(30508);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];

            while (true)
            {
                data = client.Receive(ref sender);
                BytesRecieved += data.Length;

                Message message = ParseIncomingMessage(data);
                //special case, since data may not even have 4 bytes
                if (message.Type == NetworkManager.MessageType.Ping)
                {
                    //todo
                    continue;
                }
                byte[] inData = message.Data.Skip<byte>(4).ToArray();

                //Log.WriteLine(message.Type.ToString());

                switch (message.Type)
                {
                    case NetworkManager.MessageType.Join:
                        Connected.Add(sender);
                        manager.OnServerClientConnected();
                        SendToOne(new Message(NetworkManager.MessageType.JoinConfirm, incoming: false), sender);
                        break;
                    case NetworkManager.MessageType.Leave:
                        Connected.Remove(sender);
                        break;
                    case NetworkManager.MessageType.CreateEntity:
                        NetworkEntity createdEntity = NetworkManager.DeserializeNetworkEntity(inData);
                        createdEntity.Managed = false;
                        manager.AddNetworkEntity(manager.ProcessIncomingEntity(createdEntity));
                        break;
                    case NetworkManager.MessageType.RemoveEntity:
                        //todo
                        break;
                    case NetworkManager.MessageType.EntityPosition:
                        string entityPositionJson = Encoding.ASCII.GetString(inData);
                        EntityPositionInfo entityPosition = (EntityPositionInfo) System.Text.Json.JsonSerializer.Deserialize(entityPositionJson, typeof(EntityPositionInfo));
                        manager.RecieveNetworkEntityPosition(entityPosition);
                        break;
                    default:
                        //todo
                        break;
                }
            }
        }

        public void Broadcast(Message message)
        {
            if (client == null || message.Incoming)
                return;

            byte[] outData = BuildMessageBytes(message.Type, message.Data);

            //send data to all connected clients
            foreach (IPEndPoint ip in Connected)
            {
                client.Send(outData, outData.Length, ip);
                BytesSent += outData.Length;
            }
        }

        public void SendToOne(Message message, IPEndPoint ip)
        {
            if (client == null || message.Incoming)
                return;

            byte[] outData = BuildMessageBytes(message.Type, message.Data);

            //send data to specific client
            client.Send(outData, outData.Length, ip);
            BytesSent += outData.Length;
        }

        public static Message ParseIncomingMessage(byte[] bytes)
        {
            return new Message(GetMessageType(bytes), bytes, incoming: true);
        }

        public static NetworkManager.MessageType GetMessageType(byte[] bytes)
        {
            if (bytes.Length < 4)
                return NetworkManager.MessageType.Ping;

            if (ByteArrayStartsWith(bytes, NetworkManager.JoinCode))
                return NetworkManager.MessageType.Join;
            if (ByteArrayStartsWith(bytes, NetworkManager.LeaveCode))
                return NetworkManager.MessageType.Leave;
            if (ByteArrayStartsWith(bytes, NetworkManager.JoinConfirmCode))
                return NetworkManager.MessageType.JoinConfirm;
            if (ByteArrayStartsWith(bytes, NetworkManager.CreateEntityCode))
                return NetworkManager.MessageType.CreateEntity;
            if (ByteArrayStartsWith(bytes, NetworkManager.RemoveEntityCode))
                return NetworkManager.MessageType.RemoveEntity;
            if (ByteArrayStartsWith(bytes, NetworkManager.EntityPositionCode))
                return NetworkManager.MessageType.EntityPosition;

            return NetworkManager.MessageType.Invalid;
        }

        public static byte[] MessageTypeToBytes(NetworkManager.MessageType type)
        {
            switch(type)
            {
                case NetworkManager.MessageType.Ping:
                    return new byte[1] { 0x0 };
                case NetworkManager.MessageType.Join:
                    return NetworkManager.JoinCode;
                case NetworkManager.MessageType.Leave:
                    return NetworkManager.LeaveCode;
                case NetworkManager.MessageType.JoinConfirm:
                    return NetworkManager.JoinConfirmCode;
                case NetworkManager.MessageType.CreateEntity:
                    return NetworkManager.CreateEntityCode;
                case NetworkManager.MessageType.RemoveEntity:
                    return NetworkManager.RemoveEntityCode;
                case NetworkManager.MessageType.EntityPosition:
                    return NetworkManager.EntityPositionCode;
            }
            return NetworkManager.InvalidCode;
        }

        public static byte[] BuildMessageBytes(NetworkManager.MessageType type, byte[] data)
        {
            byte[] newBytes = new byte[data.Length + 4];

            byte[] prefix = MessageTypeToBytes(type);

            for (int i = 0; i < data.Length + prefix.Length; i++)
            {
                if (i < prefix.Length)
                    newBytes[i] = prefix[i];
                else
                    newBytes[i] = data[i - prefix.Length];
            }

            return newBytes;
        }
        public static byte[] BuildMessageBytes(NetworkManager.MessageType type)
        {
            return BuildMessageBytes(type, new byte[0]);
        }

        public static bool ByteArrayStartsWith(byte[] arr, byte[] prefix)
        {
            if (arr.Length < prefix.Length)
                return false;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (arr[i] != prefix[i])
                    return false;
            }

            return true;
        }

        public struct Message
        {
            public NetworkManager.MessageType Type { get; set; }
            public byte[] Data { get; set; }
            public bool Incoming { get; set; } //incoming messages have their type included in the data, outgoing do not

            public Message(NetworkManager.MessageType type, byte[] data, bool incoming)
            {
                Type = type;
                Data = data;
                Incoming = incoming;
            }
            public Message(NetworkManager.MessageType type, bool incoming)
            {
                Type = type;
                Data = new byte[0];
                Incoming = incoming;
            }
        }

        public struct EntityPositionInfo
        {
            public string NetId { get; set; }
            public Vec2 Position { get; set; }

            public EntityPositionInfo(string netId, Vec2 position)
            {
                NetId = netId;
                Position = position;
            }
        }
    }
}
