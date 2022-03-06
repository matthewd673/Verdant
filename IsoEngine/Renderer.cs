using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IsoEngine.UI;

namespace IsoEngine
{
    public static class Renderer
    {

        public static Camera Camera { get; set; }
        public static int Scale { get; private set; }

        static Sprite pixel;

        public static Sprite Cursor { get; set; } = null;
        public static bool ShowCursor { get; set; } = true;

        public static bool SortEntities { get; set; } = true;

        static IEnumerable<Entity> sortedQueue;

        static GraphicsDevice graphicsDevice;

        /// <summary>
        /// Get a Texture2D containing a single white pixel.
        /// </summary>
        /// <returns>A Texture2D pixel.</returns>
        public static Texture2D GetPixel() { return pixel.Get(); }
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
            Renderer.graphicsDevice = graphicsDevice;
            Camera = new Camera(new Vec2(), screenWidth, screenHeight);
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
        /// <param name="entityManager">The EntityManager to draw from.</param>
        /// <param name="uiManager">The UIManager to draw from.</param>
        /// <param name="visualizeColliders">For debugging. Determine if Entities should be drawn with their colliders visible or not.</param>
        public static void Render(SpriteBatch spriteBatch, EntityManager entityManager, UIManager uiManager, bool visualizeColliders = false)
        {
            //update camera
            Camera.Update();

            //render entities
            if (SortEntities)
            {
                sortedQueue = entityManager.GetAllEntities().OrderBy(n => n.ZIndex);
                foreach (Entity e in sortedQueue)
                {
                    e.Draw(spriteBatch);
                    if (visualizeColliders)
                        e.DrawColliders(spriteBatch);
                }
            }
            else
            {
                foreach (Entity e in entityManager.GetAllEntities())
                {
                    e.Draw(spriteBatch);
                    if (visualizeColliders)
                        e.DrawColliders(spriteBatch);
                }
            }

            //render ui elements
            foreach (UIElement e in uiManager.GetElements())
            {
                e.Draw(spriteBatch);
            }

            //render cursor
            if (ShowCursor && Cursor != null)
            {
                spriteBatch.Draw(Cursor.Get(),
                    new Rectangle(InputHandler.MouseX - (Cursor.Width / 2 * Scale),
                        InputHandler.MouseY - (Cursor.Height / 2 * Scale),
                        Cursor.Width * Scale,
                        Cursor.Height * Scale
                    ),
                    Color.White);
            }
        }
        /// <summary>
        /// Render the current frame containing the elements in the provided scene.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="scene">The Scene to draw from.</param>
        /// <param name="visualizeColliders">For debugging. Determine if Entities should be drawn with their colliders visible or not.</param>
        public static void Render(SpriteBatch spriteBatch, Scene scene, bool visualizeColliders = false)
        {
            Render(spriteBatch, scene.EntityManager, scene.UIManager, visualizeColliders: visualizeColliders);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vec2 start, Vec2 end, Color color)
        {
            Vec2 diff = end - start;
            float angle = (float)Math.Atan2(diff.Y, diff.X);

            Vec2 worldStart = Camera.WorldToScreenPos(start);
            Vec2 worldEnd = Camera.WorldToScreenPos(end);

            spriteBatch.Draw(GetPixel(),
                new Rectangle((int)worldStart.X, (int)worldStart.Y, (int)diff.Magnitude(), 1),
                null,
                color,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0
                );
        }

        public static Sprite GenerateCircleTexture(float radius, Color color)
        {
            int diam = (int)(radius * 2);
            int rSqr = (int)(radius * radius);

            Texture2D circleTex = new Texture2D(graphicsDevice, diam, diam);
            Color[] colorData = new Color[diam * diam];

            for (int i = 0; i < diam; i++)
            {
                for (int j = 0; j < diam; j++)
                {
                    int colorIndex = i * diam + j;
                    float distFromCenter = (new Vec2(i, j) - new Vec2(radius, radius)).Magnitude();
                    if (Math.Abs(distFromCenter) <= radius)
                        colorData[colorIndex] = color;
                    else
                        colorData[colorIndex] = Color.Transparent;
                }
            }

            circleTex.SetData(colorData);
            return new Sprite(circleTex);
        }

    }
}
