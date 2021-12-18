using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using IsoEngine.Debugging;

namespace IsoEngine.Networking
{
    internal class Server
    {

        public static byte[] JoinBytes { get; } = Encoding.ASCII.GetBytes("join");

        List<IPAddress> connected = new List<IPAddress>();

        public void Start()
        {
            Thread listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        void Listen()
        {
            UdpClient client = new UdpClient(30508);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];

            while (true)
            {
                data = client.Receive(ref sender);

                if (data.Equals(JoinBytes))
                    connected.Add(sender.Address);
                else
                    Log.WriteLine("SERVER GOT \"" + Encoding.ASCII.GetString(data) + "\"");
            }
        }

    }
}
