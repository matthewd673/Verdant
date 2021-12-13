using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LogConsole
{
    class Program
    {

        /*
         * TODO
         * - settings file to customize port
         * - option to save output when session ends
         * - option to auto-reset every time a new client connects
         *      (presumably, this will only happen when the game is re-run)
         * - add simple handshake to beginning so anything other than the Log class is discouraged from connecting
         */

        static void Main(string[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("IsoEngine Debugging Log Console");
            Console.ResetColor();

            StartServer();
        }

        static void StartServer(int port = 8085)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            server.Start();
            Console.WriteLine("[CON] Listening on port {0}", port);

            Byte[] bytes = new byte[256];
            string data = null;

            //listen loop
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("[CON] Client connected");

                data = null;

                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("[LOG] {0}", data);
                }

                client.Close();
                Console.WriteLine("[CON] Client disconnected");
            }
        }
    }
}
