using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public static class Renderer
    {

        public static int scale = 2;

        public static void Render(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            
            foreach (Entity e in entityManager.GetEntities())
            {
                e.Draw(spriteBatch);
            }

        }

        public static Rectangle GetRenderBounds(Entity e)
        {
            return new Rectangle((int)(e.pos.x * scale), (int)(e.pos.y * scale), e.w * scale, e.w * scale);
        }

    }
}
