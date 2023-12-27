using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Debugging
{
    /// <summary>
    /// Display metrics about the current Scene and custom state information.
    /// SimpleStats is not a UIElement, it renders independently.
    /// </summary>
    public static class SimpleStats
    {

        private static int stackHeight = 0;
        private static Dictionary<string, string> customFields = new();

        // The text color stats will be written in.
        public static Color TextColor { get; set; } = Color.White;
        // The color of the background behind stats text.
        public static Color BackgroundColor { get; set; } = new Color(Color.Black, 125);

        // Determines if SimpleStats should be rendered.
        public static bool Show { get; set; } = true;

        // The top-left position of the output.
        public static Vec2Int Position { get; set; } = new();

        /// <summary>
        /// Draw the SimpleStats output.
        /// </summary>
        /// <param name="scene">The current scene.</param>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="font">The font to render with.</param>
        public static void Draw(Scene scene, SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!Show) return;

            stackHeight = 0;
            WriteToScreen($"FPS: {1000 / scene.DeltaTime} ({scene.DeltaTime}ms)", spriteBatch, font);
            WriteToScreen($"Frame Duration: {scene.EntityManager.UpdateDuration + Renderer.RenderDuration}ms (U={scene.EntityManager.UpdateDuration} + R={Renderer.RenderDuration})", spriteBatch, font);
            WriteToScreen($"Scene Entities: {scene.EntityManager.EntityCount}", spriteBatch, font);
            WriteToScreen($"Entity Updates: {scene.EntityManager.EntityUpdateCount}", spriteBatch, font);
            WriteToScreen($"PhysicsEntity Updates: {scene.EntityManager.PhysicsEntityUpdateCount}", spriteBatch, font);
            WriteToScreen($"Entity Draw Calls: {Renderer.EntityDrawCalls}", spriteBatch, font);
            WriteToScreen($"Camera Position: {scene.Camera.Position}", spriteBatch, font);

            foreach (string f in customFields.Keys)
            {
                WriteToScreen($"{f}: {customFields[f]}", spriteBatch, font);
            }
        }

        private static void WriteToScreen(string text, SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 stringDim = font.MeasureString(text);

            // draw the background
            spriteBatch.Draw(Renderer.Pixel,
                             new Rectangle(Position.X,
                                           Position.Y + stackHeight,
                                           (int)stringDim.X,
                                           (int)stringDim.Y),
                             BackgroundColor
                             );
            // draw the text
            spriteBatch.DrawString(font,
                                   text,
                                   new Vector2(Position.X,
                                               Position.Y + stackHeight
                                              ),
                                   TextColor
                                   );

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
