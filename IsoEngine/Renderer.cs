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

        public static Camera cam = new Camera(new Vec2(), 800, 600); //default, should be changed based on preferredbackbufferwidth & height
        public static int scale = 2;

        static Sprite pixel;

        public static bool sort = true;

        static IEnumerable<Entity> sortedQueue;

        public static Texture2D GetPixel() { return pixel.Get(); }

        public static Sprite GetPixelSprite() { return pixel; }

        public static void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, EntityManager entityManager, UIManager uiManager, bool visualizeColliders = false)
        {
            //update camera
            cam.Update();

            //build pixel texture, if necessary
            //TODO: find a more elegant way of accomplishing this (preferably in an init method or something)
            if (pixel == null)
            {
                Texture2D texturePixel = new Texture2D(graphicsDevice, 1, 1);
                Color[] data = new Color[1] { Color.White };
                texturePixel.SetData(data);
                pixel = texturePixel;
            }

            //render entities
            if (sort)
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

        public static Rectangle GetRenderBounds(float x, float y, int w, int h)
        {
            return new Rectangle((int)((x - cam.GetRenderPos().X) * scale), (int)((y - cam.GetRenderPos().Y) * scale), w * scale, h * scale);
        }
        public static Rectangle GetRenderBounds(Vec2 pos, int w, int h)
        {
            return GetRenderBounds(pos.X, pos.Y, w, h);
        }
        public static Rectangle GetRenderBounds(Entity e)
        {
            return GetRenderBounds(e.Position, e.Width, e.Height);
        }
        public static Rectangle GetRenderBounds(TransformAnimation.TransformState transformState)
        {
            return GetRenderBounds(transformState.x, transformState.y, (int)transformState.w, (int)transformState.h);
        }

        public static Vec2Int GetRenderPos(Entity e)
        {
            return new Vec2Int((int)((e.Position.X - cam.GetRenderPos().X) * scale), (int)((e.Position.Y - cam.GetRenderPos().Y) * scale));
        }

    }
}
