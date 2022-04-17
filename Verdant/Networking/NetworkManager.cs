using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;

namespace Verdant.Networking
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

        public Type[] NetworkEntityTypes { get; set; } = new Type[0];
        Dictionary<string, NetworkEntity> networkEntitySet = new Dictionary<string, NetworkEntity>();

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

        static JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { IncludeFields = true };

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

        public void AddNetworkEntity<TNetworkEntity>(TNetworkEntity e) where TNetworkEntity : NetworkEntity
        {
            AddEntity(e); //add entity to normal manager
            networkEntitySet.Add(e.NetId, e); //and to special manager

            if (e.Managed) //broadcast change
                SendMessage(new Server.Message(MessageType.CreateEntity, SerializeNetworkEntity(e), incoming: false));
        }

        void SetNetworkEntityType(NetworkEntity e)
        {
            if (e.GetType() == typeof(NetworkEntity))
                e.NetworkEntityType = 0;
            else
            {
                int i = 0;
                foreach (Type t in NetworkEntityTypes)
                {
                    if (e.GetType() == t)
                        e.NetworkEntityType = i + 1;
                    i++;
                }
            }
        }

        Type GetNetworkEntityType(NetworkEntity e)
        {
            if (e.NetworkEntityType == 0)
                return typeof(NetworkEntity);

            if (e.NetworkEntityType == -1)
                return null;
            if (e.NetworkEntityType > NetworkEntityTypes.Length + 1)
                return null;

            return NetworkEntityTypes[e.NetworkEntityType - 1];
        }

        void SendMessage(Server.Message message)
        {
            if (!HasConnection)
                return;

            if (server != null)
                server.Broadcast(message);
            if (client != null)
                client.Send(message);
        }

        public byte[] SerializeNetworkEntity(NetworkEntity e)
        {
            SetNetworkEntityType(e);
            string json = JsonSerializer.Serialize<object>(e, serializerOptions);
            return Encoding.ASCII.GetBytes(json);

        }

        public NetworkEntity DeserializeNetworkEntity(byte[] array)
        {
            string json = Encoding.ASCII.GetString(array);
            NetworkEntity genericEntity = (NetworkEntity) JsonSerializer.Deserialize(json, typeof(NetworkEntity));
            Type entityType = GetNetworkEntityType(genericEntity);
            if (entityType == null)
                return null;
            NetworkEntity e = (NetworkEntity) JsonSerializer.Deserialize(json, entityType);
            e.NetId = genericEntity.NetId;
            return e;
        }

        internal void RecieveNetworkEntityPosition(Server.EntityPositionInfo info)
        {
            if (!networkEntitySet.ContainsKey(info.NetId))
                return;

            NetworkEntity e = networkEntitySet[info.NetId];
            if (e == null)
                return;
            
            e.Position.X = info.Position.X;
            e.Position.Y = info.Position.Y;
            e.Velocity.X = info.Velocity.X;
            e.Velocity.Y = info.Velocity.Y;
        }

        public void SendNetworkEntityPosition(string netId, Vec2 pos, Vec2 vel)
        {
            //it goes without saying that this is temporary
            SendMessage(new Server.Message(
                MessageType.EntityPosition,
                Encoding.ASCII.GetBytes(
                    JsonSerializer.Serialize(
                        new Server.EntityPositionInfo(
                            netId,
                            pos,
                            vel
                            ),
                        typeof(Server.EntityPositionInfo)
                        )
                    ),
                incoming: false
                )
            );
        }

        public virtual NetworkEntity ProcessIncomingEntity(NetworkEntity e)
        {
            return e;
        }

        protected override void UpdateList(List<Entity> updateList)
        {
            EntityUpdateCount = 0;

            //update all
            foreach (Entity e in updateList)
            {
                if (e.GetType() == typeof(NetworkEntity) || e.GetType().IsSubclassOf(typeof(NetworkEntity)))
                {
                    NetworkEntity n = (NetworkEntity)e;
                    if (n.Managed)
                    {
                        n.Update();
                        EntityUpdateCount++;
                    }
                    else
                    {
                        n.UnmanagedUpdate();
                        EntityUpdateCount++;
                    }
                }
                else
                {
                    e.Update();
                    EntityUpdateCount++; //keep track
                }

                //remove marked entities
                if (e.ForRemoval)
                {
                    RemoveEntity(e);
                    continue; //don't bother with anything else if being removed
                }
                if (!e.Key.Equals(e.PreviousKey))
                    MoveEntityCell(e);
            }

            ApplyQueues();
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
