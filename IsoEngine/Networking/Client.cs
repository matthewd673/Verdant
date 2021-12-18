using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace IsoEngine.Networking
{
    internal class Client
    {

        string serverIp;
        int serverPort;

        public void Connect(string ip, int port)
        {
            serverIp = ip;
            serverPort = port;
            Thread joinThread = new Thread(new ThreadStart(Join));
            joinThread.Start();
        }

        void Join()
        {
            UdpClient client = new UdpClient(30509);
            client.Connect(serverIp, serverPort);

            client.Send(Server.JoinBytes, Server.JoinBytes.Length);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];

            while (true)
            {
                data = client.Receive(ref sender);

                //todo
            }
        }

    }
}
