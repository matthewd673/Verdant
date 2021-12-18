using System;
using IsoEngine.UI;

namespace IsoEngine.Networking
{
    public class NetworkScene : Scene
    {

        public new EntityManager EntityManager
        {
            get { return NetworkManager; }
        }
        public NetworkManager NetworkManager { get; protected set; }

        public NetworkScene(int id) : base(id) { }

        public virtual void Initialize()
        {
            NetworkManager = new NetworkManager();
            UIManager = new UIManager();
        }
    }
}
