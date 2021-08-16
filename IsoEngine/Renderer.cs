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

        public static bool SortEntities = true;

        static IEnumerable<Entity> sortedQueue;

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
                sortedQueue = entityManager.GetEntities().OrderBy(n => n.ZIndex);
                foreach (Entity e in sortedQueue)
                {
                    e.Draw(spriteBatch);
                    if (visualizeColliders)
                        e.DrawColliders(spriteBatch);
                }
            }
            else
            {
                foreach (Entity e in entityManager.GetEntities())
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
        }
        /// <summary>
        /// Render the current frame containing the elements in the provided scene.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="scene">The Scene to draw from.</param>
        /// <param name="visualizeColliders">For debugging. Determine if Entities should be drawn with their colliders visible or not.</param>
        public static void Render(SpriteBatch spriteBatch, Scene scene, bool visualizeColliders = false)
        {
            Render(spriteBatch, scene.entityManager, scene.uiManager, visualizeColliders: visualizeColliders);
        }

    }
}
