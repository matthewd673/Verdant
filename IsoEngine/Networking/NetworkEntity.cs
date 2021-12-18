using System;

namespace IsoEngine.Networking
{
    [Serializable]
    public class NetworkEntity : Entity
    {

        public byte[] NetId { get; }
        public bool Managed { get; }

        public NetworkEntity(RenderObject sprite, Vec2 pos, bool managed = true, int w = -1, int h = -1) : base(sprite, pos, w, h)
        {
            NetId = Guid.NewGuid().ToByteArray();
            Managed = managed;
        }

        public void StartServer()
        {
            Server server = new Server();
            server.Start();
        }

        public void StartClient(string ip)
        {
            Client client = new Client();
            client.Connect(ip, 30508);

        }

        public override void Update()
        {
            if (Managed)
            {
                base.Update();

                //transmit changes over network (if managed)
            }
        }

        public void ApplyChanges()
        {

        }

    }
}
