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
         */

        static void Main(string[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("IsoEngine Debugging Log Console");
            Console.ResetColor();

            StartServer();
        }

        static void StartServer()
        {

            UdpClient server = new UdpClient(8085);

            Console.WriteLine("[CON] Listening on port 8085");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[1024];
            data = server.Receive(ref sender);

            //recieve first message (handshake)
            if (Encoding.ASCII.GetString(data, 0, data.Length).Equals("__mdebug__")) //correct
            {
                Console.WriteLine("[CON] Debugging session started");
            }
            else
            {
                Console.WriteLine("[WRN] Unexpected client connected");
            }

            while (true)
            {
                data = server.Receive(ref sender);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                server.Send(data, data.Length, sender);
            }
        }
    }
}
