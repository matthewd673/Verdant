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

        // The text color stats will be written in.
        public static Color TextColor { get; set; } = Color.White;
        // The color of the background behind stats text.
        public static Color BackgroundColor { get; set; } = new Color(Color.Black, 125);

        /// <summary>
        /// Draw the SimpleStats output
        /// </summary>
        /// <param name="scene">The current scene.</param>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="font">The font to render with.</param>
        public static void Render(Scene scene, SpriteBatch spriteBatch, SpriteFont font)
        {
            stackHeight = 0;
            WriteToScreen($"FPS: {1000/scene.DeltaTime} ({scene.DeltaTime}ms)", spriteBatch, font);
            WriteToScreen($"Frame Duration: {scene.EntityManager.UpdateDuration + Renderer.RenderDuration}ms (U={scene.EntityManager.UpdateDuration} + R={Renderer.RenderDuration})", spriteBatch, font);
            WriteToScreen($"Entities: {scene.EntityManager.EntityCount}", spriteBatch, font);
            WriteToScreen($"Total updates (last tick): {scene.EntityManager.EntityUpdateCount}", spriteBatch, font);
            WriteToScreen($"Physics updates (last tick): {scene.EntityManager.PhysicsEntityUpdateCount}", spriteBatch, font);
        
            foreach (string f in customFields.Keys)
            {
                WriteToScreen($"{f}: {customFields[f]}", spriteBatch, font);
            }
        }

        private static void WriteToScreen(string text, SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 stringDim = font.MeasureString(text);
            spriteBatch.Draw(Renderer.GetPixel(), new Rectangle(10, 10 + stackHeight, (int)stringDim.X, (int)stringDim.Y), BackgroundColor);
            spriteBatch.DrawString(font, text, new Vector2(10, 10 + stackHeight), TextColor);
            stackHeight += (int)stringDim.Y;
        }

        /// <summary>
        /// Update the value of a custom field on the SimpleStats output. If the field has not been referenced before, it will be created.
        /// </summary>
        /// <param name="fieldName">The name of the field. Updating a field of the same name will overwrite the old value.</param>
        /// <param name="value">The value of the field to be output.</param>
        public static void UpdateField(string fieldName, object value)
        {
            if (!customFields.TryAdd(fieldName, value.ToString()))
                customFields[fieldName] = value.ToString();
        }

    }
}
