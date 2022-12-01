using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Debugging
{
    public static class SimpleStats
    {

        private static int stackHeight = 0;
        private static Dictionary<string, string> customFields = new();

        public static Color TextColor { get; set; } = Color.White;
        public static Color BackgroundColor { get; set; } = new Color(Color.Black, 125);

        public static void Render(Scene scene, SpriteBatch spriteBatch, SpriteFont font)
        {
            stackHeight = 0;
            WriteToScreen($"FPS: {1000/scene.DeltaTime} ({scene.DeltaTime}ms)", spriteBatch, font);
            WriteToScreen($"Entities: {scene.EntityManager.EntityCount}", spriteBatch, font);
            WriteToScreen($"Total updates (last tick): {scene.EntityManager.EntityUpdateCount}", spriteBatch, font);
            WriteToScreen($"Physics updates (last tick): {scene.EntityManager.PhysicsEntityUpdateCount}", spriteBatch, font);
        
            foreach (string f in customFields.Keys)
            {
                WriteToScreen($"{f}: {customFields[f]}", spriteBatch, font);
            }
        }

        static void WriteToScreen(string text, SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 stringDim = font.MeasureString(text);
            spriteBatch.Draw(Renderer.GetPixel(), new Rectangle(10, 10 + stackHeight, (int)stringDim.X, (int)stringDim.Y), BackgroundColor);
            spriteBatch.DrawString(font, text, new Vector2(10, 10 + stackHeight), TextColor);
            stackHeight += (int)stringDim.Y;
        }

        public static void UpdateField(string fieldName, object value)
        {
            if (!customFields.TryAdd(fieldName, value.ToString()))
                customFields[fieldName] = value.ToString();
        }

    }
}
