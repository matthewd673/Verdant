using System;
using System.IO;
using System.Text.Json.Serialization;

namespace IsoEngine.Networking
{
    [Serializable]
    public class NetworkEntity : Entity
    {

        public string NetId { get; }
        [JsonIgnore]
        public bool Managed { get; internal set; } = true;
        public string EntityType { get; }

        public NetworkEntity(string entityType, RenderObject sprite, Vec2 position, int width = -1, int height = -1) : base(sprite, position, width, height)
        {
            NetId = Guid.NewGuid().ToString();
            EntityType = entityType;
        }

        [JsonConstructor]
        public NetworkEntity(string netId, string entityType, Vec2 position, int width = -1, int height = -1) : base(null, position, width, height)
        {
            NetId = netId;
            EntityType = entityType;
        }

        protected static byte[] CombineByteArrays(params byte[][] arrays)
        {
            byte[] combined = new byte[arrays.Length * 8];
            for (int i = 0; i < combined.Length; i++)
            {
                int arrIndex = (int)System.Math.Floor((double)i / 8.0);
                combined[i] = arrays[arrIndex][arrIndex + (i % 8)];
            }

            return combined;
        }

        public override void Update()
        {
            if (Managed)
            {
                //float oldX = Position.X;
                //float oldY = Position.Y;
                base.Update();

                //transmit changes over network (if managed)
                ((NetworkManager)Manager).SendNetworkEntityPosition(NetId, Position);
            }
        }

        public void ApplyChanges()
        {

        }

    }
}
