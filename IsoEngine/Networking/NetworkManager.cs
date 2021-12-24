using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;

namespace IsoEngine.Networking
{
    public class NetworkManager : EntityManager
    {

        public enum MessageType
        {
            Invalid,
            Ping,
            Join,
            Leave,
            JoinConfirm,
            CreateEntity,
            RemoveEntity,
            EntityPosition,
        }

        public static readonly byte[] InvalidCode = Encoding.ASCII.GetBytes("____");
        public static readonly byte[] JoinCode = Encoding.ASCII.GetBytes("join");
        public static readonly byte[] LeaveCode = Encoding.ASCII.GetBytes("leav");
        public static readonly byte[] JoinConfirmCode = Encoding.ASCII.GetBytes("jncn");
        public static readonly byte[] CreateEntityCode = Encoding.ASCII.GetBytes("crte");
        public static readonly byte[] RemoveEntityCode = Encoding.ASCII.GetBytes("rmve");
        public static readonly byte[] EntityPositionCode = Encoding.ASCII.GetBytes("epos");

        Server server;
        Client client;

        Dictionary<string, NetworkEntity> networkEntitySet = new Dictionary<string, NetworkEntity>();
        int networkEntityCt = 0;

        public int ConnectedClients
        {
            get
            {
                if (server == null)
                    return -1;
                else
                    return server.Connected.Count;
            }
        }
        public int BytesRecieved
        {
            get
            {
                if (server == null)
                    return client.BytesRecieved;
                else
                    return server.BytesRecieved;
            }
        }
        public int BytesSent
        {
            get
            {
                if (server == null)
                    return client.BytesSent;
                else
                    return server.BytesSent;
            }
        }
        public bool HasConnection
        {
            get
            {
                if (server == null)
                    return client.ConnectionConfirmed;
                else
                    return server.Connected.Count > 0;
            }
        }

        public void StartServer()
        {
            server = new Server(this);
            server.Start();
        }

        public void StartClient(string ip)
        {
            client = new Client(this);
            client.Connect(ip);
        }

        public void AddNetworkEntity(NetworkEntity e)
        {
            AddEntity(e); //add entity to normal manager
            networkEntitySet.Add(e.NetId, e); //and to special manager

            if (e.Managed)
            {
                //broadcast change
                SendMessage(new Server.Message(MessageType.CreateEntity, SerializeNetworkEntity(e), incoming: false));
            }
            else
            {
                //incoming entity
            }
        }

        void SendMessage(Server.Message message)
        {
            if (server != null)
                server.Broadcast(message);
            if (client != null)
                client.Send(message);
        }

        public static byte[] SerializeNetworkEntity<TValue>(TValue e) where TValue : NetworkEntity
        {
            //SurrogateSelector surrogateSelector = new SurrogateSelector();
            //surrogateSelector.AddSurrogate(
            //    e.GetType(),
            //    new StreamingContext(StreamingContextStates.All),
            //    new NetworkEntitySerializationSurrogate()
            //);

            //MemoryStream stream = new MemoryStream();
            //IFormatter serializer = new BinaryFormatter();
            //serializer.SurrogateSelector = surrogateSelector;
            //serializer.Serialize(stream, e);

            //return stream.ToArray();

            string json = JsonSerializer.Serialize<TValue>(e);
            System.IO.File.WriteAllText("C:\\Users\\Matthew\\Desktop\\isomultiplayer.txt", json); //TEMP!
            return Encoding.ASCII.GetBytes(json);

        }

        public static NetworkEntity DeserializeNetworkEntity(byte[] array)
        {
            //MemoryStream stream = new MemoryStream(array);
            //NetworkEntity e = (NetworkEntity)new BinaryFormatter().Deserialize(stream);
            //stream.Close();
            //return e;
            string json = Encoding.ASCII.GetString(array);
            NetworkEntity genericEntity = (NetworkEntity) JsonSerializer.Deserialize(json, typeof(NetworkEntity));
            return genericEntity;
        }

        internal void RecieveNetworkEntityPosition(Server.EntityPositionInfo info)
        {
            NetworkEntity e = networkEntitySet[info.NetId];
            if (e == null)
                return;
            e.Position.X = info.Position.X;
            e.Position.Y = info.Position.Y;
        }

        public void SendNetworkEntityPosition(string netId, Vec2 pos)
        {
            //it goes without saying that this is temporary
            SendMessage(new Server.Message(
                MessageType.EntityPosition,
                Encoding.ASCII.GetBytes(
                    JsonSerializer.Serialize(
                        new Server.EntityPositionInfo(
                            netId,
                            pos
                            ),
                        typeof(Server.EntityPositionInfo)
                        )
                    ),
                incoming: false
                )
            );
        }

        //weirdo strategy
        public virtual NetworkEntity ProcessIncomingEntity(NetworkEntity e)
        {
            return e;
        }

        //EVENTS

        public event EventHandler ServerClientConnected;
        public virtual void OnServerClientConnected()
        {
            EventHandler serverClientConnectedEvent = ServerClientConnected;
            if (serverClientConnectedEvent != null)
                serverClientConnectedEvent(this, EventArgs.Empty); //todo, better event args
        }

        public event EventHandler ClientConnectionConfirmed;
        public virtual void OnClientConnectionConfirmed()
        {
            EventHandler clientConnectionConfirmedEvent = ClientConnectionConfirmed;
            if (clientConnectionConfirmedEvent != null)
                clientConnectionConfirmedEvent(this, EventArgs.Empty); //todo, better event args
        }

    }
}
