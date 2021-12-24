using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.Debugging
{
    public static class SimpleStats
    {

        static int lineCt = 0;

        public static void Render(Scene scene, SpriteBatch spriteBatch, SpriteFont font)
        {
            lineCt = 0;
            WriteToScreen("Entities: " + scene.EntityManager.EntityCount, spriteBatch, font);
            WriteToScreen("Updated (last tick): " + scene.EntityManager.EntityUpdateCount, spriteBatch, font);
            
            if (scene.GetType() == typeof(Networking.NetworkScene) || scene.GetType().IsSubclassOf(typeof(Networking.NetworkScene)))
            {
                Networking.NetworkScene nScene = (Networking.NetworkScene)scene;
                if (nScene.Host)
                {
                    WriteToScreen("Host", spriteBatch, font);
                    WriteToScreen("Connected Clients: " + nScene.NetworkManager.ConnectedClients.ToString(), spriteBatch, font);
                }
                else
                {
                    WriteToScreen("Client", spriteBatch, font);
                    WriteToScreen("Has Connection: " + nScene.NetworkManager.HasConnection.ToString(), spriteBatch, font);
                }
                WriteToScreen("Bytes Sent/Recieved: " + nScene.NetworkManager.BytesSent + "/" + nScene.NetworkManager.BytesRecieved, spriteBatch, font);
            }
        }

        static void WriteToScreen(string text, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, text, new Vector2(10, 10 + (20 * lineCt)), Color.White);
            lineCt++;
        }

    }
}
