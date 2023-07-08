using System;
using System.Net.Sockets;
using System.Text;

namespace Verdant.Debugging
{
    /// <summary>
    /// Send messages over UDP to the Verdant LogConsole debug tool.
    /// </summary>
    public static class Log
    {

        /*
         * TODO
         * - add option to warn when connecting (so a game doesn't accidentally ship with the logger still going)
         */

        private static UdpClient client;
        // If the logger fails to establish a WebSockets connection, it will stop attempting to send messages and this value will be true.
        public static bool ConnectionFailed { get; private set; } = false;

        // The port of the game's UdpClient. Should not be changed after the first message is sent.
        public static int ClientPort { get; set; } = 40425;
        // The port of the LogConsole instance. Should not be changed after the first message is sent.
        public static int ConsolePort { get; set; } = 8085;

        /// <summary>
        /// Send a string to the LogConsole. If the game is not currently connected to the LogConsole, it will attempt to connect (but only once).
        /// NOTE: Log is not intended for production use. WriteLine will not do anything unless the game is running in a debug build.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(params object[] values)
        {
#if DEBUG
            if (ConnectionFailed) { return; } //only try to connect once

            if (client == null)
            {
                client = new UdpClient(ClientPort);
                try
                {
                    client.Connect("127.0.0.1", ConsolePort);
                    //first message
                    byte[] firstBytes = Encoding.ASCII.GetBytes("__mdebug__");
                    client.Send(firstBytes, firstBytes.Length);
                }
                catch (Exception)
                {
                    //fail silently for now
                    ConnectionFailed = true;
                    return;
                }
            }

            // hanlde printing null values
            string s = ObjectsToString(values);
            byte[] messageBytes = Encoding.ASCII.GetBytes(s);

            // fail silently if message doesn't send
            try
            {
                client.Send(messageBytes, messageBytes.Length);
            }
            catch (Exception e)
            {
                ConnectionFailed = true;
            }
#endif
        }

        private static string ObjectsToString(object[] objects)
        {
            if (objects == null)
                return "null.";

            StringBuilder output = new();
            for (int i = 0; i < objects.Length; i++)
            {
                string objStr = objects[i].ToString();
                if (objStr == null)
                    output.Append("null");
                else
                    output.Append(objStr);

                output.Append(' ');
            }
            return output.ToString();
        }

    }
}
