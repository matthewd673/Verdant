using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace IsoEngine.Networking
{
    public class NetworkManager : EntityManager
    {

        Dictionary<string, NetworkEntity> networkEntitySet = new Dictionary<string, NetworkEntity>();

        public void StartServer()
        {
            Server server = new Server();
        }

    }
}
