using System;
using System.Net.Sockets;
using System.Text;

namespace IsoEngine.Debugging
{
    public static class Log
    {

        /*
         * TODO
         * - add option to warn when connecting (so a game doesn't accidentally ship with the logger still going)
         */

        static UdpClient client;
        static bool connectionFailed = false;

        /// <summary>
        /// Send a string to the LogConsole. If the game is not currently connected to the LogConsole, it will attempt to connect (but only once).d
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {

            if (connectionFailed) { return; } //only try to connect once

            if (client == null)
            {
                client = new UdpClient(40425);
                try
                {
                    client.Connect("127.0.0.1", 8085);
                    //first message
                    byte[] firstBytes = Encoding.ASCII.GetBytes("__mdebug__");
                    client.Send(firstBytes, firstBytes.Length);
                }
                catch (Exception e)
                {
                    //fail silently for now
                    connectionFailed = true;
                    return;
                }
            }

            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            client.Send(messageBytes, messageBytes.Length);
        }

    }
}
