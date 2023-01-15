using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Verdant.UI;

namespace Verdant
{
    /// <summary>
    /// Manages the rendering of the current Scene and stores relevant information about the game window's graphics.
    /// </summary>
    public static class Renderer
    {

        // The width of the game window.
        public static int ScreenWidth { get; private set; }
        // The height of the game window.
        public static int ScreenHeight { get; private set; }
        // The scale at which everything will be rendered.
        public static int Scale { get; private set; }

        private static Sprite pixel;

        // A custom cursor Sprite, which will be rendered if ShowCursor is true.
        public static Sprite Cursor { get; set; } = null;
        // Determines if the custom cursor Sprite should be rendered.
        public static bool ShowCursor { get; set; } = true;

        // Determines if Entities should be sorted according to z-index before rendering.
        public static bool SortEntities { get; set; } = true;

        // Determines if UIElements should be sorted according to z-index before rendering.
        public static bool SortUIElements { get; set; } = true;

        // The window's GraphicsDevice.
        public static GraphicsDevice GraphicsDevice { get; private set; }

        private static Stopwatch renderPerformanceTimer = new Stopwatch();
        // The duration (in milliseconds) of the last Render call.
        public static float RenderDuration { get; private set; }

        /// <summary>
        /// Get a Texture2D containing a single white pixel.
        /// </summary>
        /// <returns>A Texture2D pixel.</returns>
        public static Texture2D GetPixel() { return pixel.Draw(); }
        /// <summary>
        /// Get a Sprite containing a single white pixel.
        /// </summary>
        /// <returns>A Sprite pixel.</returns>
        public static Sprite GetPixelSprite() { return pixel; }

        /// <summary>
        /// Initialize the Renderer. Create a Camera, establish a consistent render scale, etc.
        /// </summary>
        /// <param name="graphicsDevice">A GraphicsDevice, used to build the automatic pixel texture.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="scale">The render scale.</param>
        public static void Initialize(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight, int scale)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            GraphicsDevice = graphicsDevice;
            Scale = scale;

            //build pixel texture
            Texture2D texturePixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] data = new Color[1] { Color.White };
            texturePixel.SetData(data);
            pixel = texturePixel;
        }

        /// <summary>
        /// Render the current frame containing the elements in the provided managers.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="scene">The Scene to render.</param>
        /// <param name="visualizeBodies">For debugging. Determine if Entities should be drawn with their colliders visible.</param>
        /// <param name="visualizeUIBounds>For debugging. Determine if UIElements should be drawn with their bounds visible.</param>
        public static void Render(SpriteBatch spriteBatch, Scene scene, bool visualizeBodies = false, bool visualizeUIBounds = false)
        {
            renderPerformanceTimer.Start();

            // render entities
            IEnumerable<Entity> entities;
            if (SortEntities)
                entities = scene.EntityManager.GetEntitiesInBounds(scene.Camera.Position, scene.Camera.Width, scene.Camera.Height).OrderBy(n => n.ZIndex);
            else
                entities = scene.EntityManager.GetEntitiesInBounds(scene.Camera.Position, scene.Camera.Width, scene.Camera.Height);

            foreach (Entity e in entities)
            {
                e.Draw(spriteBatch);
            }

            if (visualizeBodies)
            {
                foreach (Entity e in entities)
                {
                    if (e.IsType(typeof(Physics.PhysicsEntity)))
                        ((Physics.PhysicsEntity)e).DrawBody(spriteBatch);
                }
            }

            // render ui elements
            IEnumerable<UIElement> uiElements;
            if (SortUIElements)
                uiElements = scene.UIManager.GetElements().OrderBy(n => n.ZIndex);
            else
                uiElements = scene.UIManager.GetElements();

            foreach (UIElement e in scene.UIManager.GetElements())
            {
                e.Draw(spriteBatch);
            }

            if (visualizeUIBounds)
            {
                foreach (UIElement e in scene.UIManager.GetElements())
                {
                    e.DrawBounds(spriteBatch);
                }
            }

            // render cursor
            if (ShowCursor && Cursor != null)
            {
                spriteBatch.Draw(Cursor.Draw(),
                    new Rectangle(InputHandler.MouseX - (Cursor.Width / 2 * Scale),
                        InputHandler.MouseY - (Cursor.Height / 2 * Scale),
                        Cursor.Width * Scale,
                        Cursor.Height * Scale
                    ),
                    Color.White);
            }

            renderPerformanceTimer.Stop();
            RenderDuration = renderPerformanceTimer.ElapsedMilliseconds;
            renderPerformanceTimer.Reset();
        }

        /// <summary>
        /// Draw a line on the screen in screen space.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="start">The start point of the line.</param>
        /// <param name="end">The end point of the line.</param>
        /// <param name="color">The color of the line.</param>
        public static void DrawLine(SpriteBatch spriteBatch, Vec2 start, Vec2 end, Color color)
        {
            Vec2 diff = end - start;
            float angle = (float)Math.Atan2(diff.Y, diff.X);

            spriteBatch.Draw(GetPixel(),
                new Rectangle((int)start.X, (int)start.Y, (int)diff.Magnitude(), 1),
                null,
                color,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0
                );
        }

        /// <summary>
        /// Draw a line on the screen in world space.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="camera">The Camera to render through.</param>
        /// <param name="start">The start point of the line.</param>
        /// <param name="end">The end point of the line.</param>
        /// <param name="color">The color of the line.</param>
        public static void DrawLine(SpriteBatch spriteBatch, Camera camera, Vec2 start, Vec2 end, Color color)
        {
            DrawLine(spriteBatch,
                camera.WorldToScreenPos(start),
                camera.WorldToScreenPos(end),
                color
                );
        }

        /// <summary>
        /// Draw an axis-aligned rectangle in world space.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="camera">The Camera to render through.</param>
        /// <param name="topLeft">The coordinates of the top left corner of the Rectangle.</param>
        /// <param name="bottomRight">The coordinates of the bottom right corner of the Rectangle.</param>
        /// <param name="color">The color of the Rectangle.</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, Camera camera, Vec2 topLeft, Vec2 bottomRight, Color color)
        {
            DrawLine(spriteBatch, camera, topLeft, new Vec2(bottomRight.X, topLeft.Y), color);
            DrawLine(spriteBatch, camera, new Vec2(bottomRight.X, topLeft.Y), bottomRight, color);
            DrawLine(spriteBatch, camera, bottomRight, new Vec2(topLeft.X, bottomRight.Y), color);
            DrawLine(spriteBatch, camera, new Vec2(topLeft.X, bottomRight.Y), topLeft, color);
        }

        /// <summary>
        /// Draw an axis-aligned rectangle in screen space.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="topLeft">The coordinates of the top left corner of the Rectangle.</param>
        /// <param name="bottomRight">The coordinates of the bottom right corner of the Rectangle.</param>
        /// <param name="color">The color of the Rectangle.</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, Vec2 topLeft, Vec2 bottomRight, Color color)
        {
            DrawLine(spriteBatch, topLeft, new Vec2(bottomRight.X, topLeft.Y), color);
            DrawLine(spriteBatch, new Vec2(bottomRight.X, topLeft.Y), bottomRight, color);
            DrawLine(spriteBatch, bottomRight, new Vec2(topLeft.X, bottomRight.Y), color);
            DrawLine(spriteBatch, new Vec2(topLeft.X, bottomRight.Y), topLeft, color);
        }

        /// <summary>
        /// Generate a (non-filled) circle Sprite with a given radius and color.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <returns>A Sprite with the circle raster.</returns>
        public static Sprite GenerateCircleSprite(int radius, Color color)
        {
            int diam = radius * 2;

            Texture2D circleTex = new Texture2D(GraphicsDevice, diam+1, diam+1);
            Color[] colorData = new Color[(diam+1) * (diam+1)];

            // Bresenham's
            int x = 0;
            int y = radius;
            int d = 3 - diam;

            DrawCircleSpritePoint(x, y, radius, diam, colorData, color);
            while (y >= x)
            {
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }

                DrawCircleSpritePoint(x, y, radius, diam, colorData, color);
            }

            circleTex.SetData(colorData);
            return new Sprite(circleTex);
        }

        private static void DrawCircleSpritePoint(int x, int y, int rad, int diam, Color[] colorData, Color color)
        {
            colorData[(rad+x) * (diam+1) + (rad+y)] = color;
            colorData[(rad-x) * (diam+1) + (rad+y)] = color;
            colorData[(rad+x) * (diam+1) + (rad-y)] = color;
            colorData[(rad-x) * (diam+1) + (rad-y)] = color;
            colorData[(rad+y) * (diam+1) + (rad+x)] = color;
            colorData[(rad-y) * (diam+1) + (rad+x)] = color;
            colorData[(rad+y) * (diam+1) + (rad-x)] = color;
            colorData[(rad-y) * (diam+1) + (rad-x)] = color;
        }

    }
}
