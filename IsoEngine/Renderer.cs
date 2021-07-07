using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public static class Renderer
    {

        public static Camera cam = new Camera(new Vec2(), 800, 600); //default, should be changed based on preferredbackbufferwidth & height
        public static int scale = 2;

        public static Texture2D pixel;

        public static bool sort = true;

        static IEnumerable<Entity> sortedQueue;

        public static void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, EntityManager entityManager, UIManager uiManager, bool visualizeColliders = false)
        {

            //update camera
            cam.Update();

            //build pixel texture, if necessary
            if (pixel == null)
            {
                pixel = new Texture2D(graphicsDevice, 1, 1);
                Color[] data = new Color[1] { Color.White };
                pixel.SetData(data);
            }

            //render entities
            if (sort)
            {
                sortedQueue = entityManager.GetEntities().OrderBy(n => n.zIndex);
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
            return new Rectangle((int)((x - cam.GetRenderPos().x) * scale), (int)((y - cam.GetRenderPos().y) * scale), w * scale, h * scale);
        }
        public static Rectangle GetRenderBounds(Vec2 pos, int w, int h)
        {
            return GetRenderBounds(pos.x, pos.y, w, h);
        }
        public static Rectangle GetRenderBounds(Entity e)
        {
            return GetRenderBounds(e.pos, e.w, e.h);
        }

        public static Vec2Int GetRenderPos(Entity e)
        {
            return new Vec2Int((int)((e.pos.x - cam.GetRenderPos().x) * scale), (int)((e.pos.y - cam.GetRenderPos().y) * scale));
        }

    }
}
