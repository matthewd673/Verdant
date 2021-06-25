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

        protected bool hasPhysics = false;
        protected float friction = 0.6f;
        protected Vec2 velocity;
        protected Vec2 acceleration;

        public Entity(Texture2D sprite, Vec2 pos, int w, int h)
        {
            this.pos = pos;
            this.sprite = sprite;
            this.w = w;
            this.h = h;
        }

        public void SetFriction(float friction)
        {
            this.friction = friction;
        }

        public void SetVelocity(Vec2 velocity)
        {
            this.velocity = velocity;
        }

        public void SetAcceleration(Vec2 acceleration)
        {
            this.acceleration = acceleration;
        }

        public virtual void Update()
        {
            if (hasPhysics && velocity != null)
            {
                pos += velocity;
                if (acceleration != null)
                {
                    velocity += acceleration - (velocity * friction);
                    acceleration *= friction;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Renderer.GetRenderBounds(this), Color.White);
        }

    }
}
