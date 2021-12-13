using System;
using System.Net.Sockets;
using System.Text;

namespace IsoEngine.Debugging
{
    public static class Log
    {

        /*
         * TODO
         * - add option to auto-begin on first WriteLine
         * - add option to warn when connecting (so a game doesn't accidentally ship with the logger still going)
         */

        static TcpClient client;
        static NetworkStream stream;

        public static void Begin(int port = 8085)
        {
            client = new TcpClient("127.0.0.1", port);
            stream = client.GetStream();
        }

        public static void WriteLine(string message)
        {
            if (stream == null)
                return;

            stream.Write(
                Encoding.ASCII.GetBytes(message),
                0,
                message.Length
                );
        }

    }
}
