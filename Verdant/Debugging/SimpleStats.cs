using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Debugging
{
    public static class SimpleStats
    {

        static int lineCt = 0;

        public static void Render(Scene scene, SpriteBatch spriteBatch, SpriteFont font)
        {
            lineCt = 0;
            WriteToScreen("Entities: " + scene.EntityManager.EntityCount, spriteBatch, font);
            WriteToScreen("Updated (last tick): " + scene.EntityManager.EntityUpdateCount, spriteBatch, font);
        }

        static void WriteToScreen(string text, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, text, new Vector2(10, 10 + (20 * lineCt)), Color.White);
            lineCt++;
        }

    }
}
