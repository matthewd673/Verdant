using System;
using IsoEngine.UI;

namespace IsoEngine.Networking
{
    public class NetworkScene : Scene
    {

        public bool Host { get; protected set; }

        public NetworkManager NetworkManager
        {
            get { return (NetworkManager) EntityManager; }
        }

        public NetworkScene(int id, bool host) : base(id)
        {
            Host = host;
        }

        public override void Initialize()
        {
            EntityManager = new NetworkManager();
            UIManager = new UIManager();

            Start();
        }

        public void Start()
        {
            if (Host)
                NetworkManager.StartServer();
            else
                NetworkManager.StartClient("127.0.0.1"); //temp
        }

    }
}
