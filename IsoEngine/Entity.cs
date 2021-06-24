using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class Entity
    {

        public Vec2 pos;
        public Texture2D sprite;
        public int w;
        public int h;

        public Entity(Texture2D sprite, Vec2 pos, int w, int h)
        {
            this.pos = pos;
            this.sprite = sprite;
            this.w = w;
            this.h = h;
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Renderer.GetRenderBounds(this), Color.White);
        }

    }
}
