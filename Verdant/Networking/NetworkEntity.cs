using System;
using System.Text.Json.Serialization;

namespace Verdant.Networking
{
    [Serializable]
    public class NetworkEntity : Entity
    {

        public new NetworkManager Manager
        {
            get { return (NetworkManager)base.Manager; }
            set { base.Manager = value; }
        }

        public string NetId { get; internal set; }
        [JsonIgnore]
        public bool Managed { get; internal set; } = true;
        public int NetworkEntityType { get; internal set; } = -1;

        int _networkUpdateFrequency = 5;
        public int NetworkUpdateFrequency
        {
            get { return _networkUpdateFrequency; }
            protected set
            {
                _networkUpdateFrequency = value;
                networkUpdateTimer = new Timer(60 / _networkUpdateFrequency);
            }
        }

        Timer networkUpdateTimer;

        public NetworkEntity(RenderObject sprite, Vec2 position, int width = -1, int height = -1, float mass = 1) : base(sprite, position, width, height, mass)
        {
            NetId = Guid.NewGuid().ToString();
            networkUpdateTimer = new Timer(60 / NetworkUpdateFrequency);
        }

        [JsonConstructor]
        public NetworkEntity(string netId, int networkEntityType, Vec2 position, int width = -1, int height = -1) : base(null, position, width, height)
        {
            NetId = netId;
            NetworkEntityType = networkEntityType;
            networkUpdateTimer = new Timer(60 / NetworkUpdateFrequency);
        }

        protected static byte[] CombineByteArrays(params byte[][] arrays)
        {
            byte[] combined = new byte[arrays.Length * 8];
            for (int i = 0; i < combined.Length; i++)
            {
                int arrIndex = (int)Math.Floor(i / 8.0);
                combined[i] = arrays[arrIndex][arrIndex + (i % 8)];
            }

            return combined;
        }

        /// <summary>
        /// Perform's the Entity's base update loop and transmits changes over the network at the appropriate frequency.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // transmit changes over network (very basic for now)
            networkUpdateTimer.Tick();
            if (networkUpdateTimer.Consume())
                Manager.SendNetworkEntityPosition(NetId, Position, Velocity);
        }

        /// <summary>
        /// Perform updates without attempting to update the state across the network.
        /// Changes to networked properties (like Entity position) will still be propogated the next time <c>Update()</c> 
        /// </summary>
        public virtual void UnmanagedUpdate()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }

    }
}
