using System;
using System.Collections.Generic;
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

        public string type;

        List<Collider> colliders = new List<Collider>();

        public Entity(string type, Texture2D sprite, Vec2 pos, int w, int h)
        {
            this.type = type;
            this.sprite = sprite;
            this.pos = pos;
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

        /// <summary>
        /// Create a Collider encompassing the Entity's bounding box.
        /// </summary>
        /// <param name="trigger">Determines if the Collider is a trigger or not.</param>
        public void AddSimpleCollider(bool trigger = false)
        {
            AddCollider(new Collider(this, trigger: trigger, relativePos: true));
        }
        public void AddCollider(Collider c)
        {
            colliders.Add(c);
        }
        public void RemoveCollider(Collider c)
        {
            colliders.Remove(c);
        }
        public void ClearColliders()
        {
            colliders.Clear();
        }
        public List<Collider> GetColliders()
        {
            return colliders;
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

            foreach (Collider c in colliders)
                c.Update();

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Renderer.GetRenderBounds(this), Color.White);
        }

        /// <summary>
        /// Draws all of the Colliders attached to the Entity (for debug). Yellow = Solid, Green = Trigger.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        public void DrawColliders(SpriteBatch spriteBatch)
        {
            foreach (Collider c in colliders)
            {
                //color code
                Color color = Color.Yellow;
                if (c.trigger)
                    color = Color.LimeGreen;

                //top line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(pos, w, 1), color);
                //bottom line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(pos.x, pos.y + h, w + 1, 1), color);
                //left line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(pos, 1, h), color);
                //right line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(pos.x + w, pos.y, 1, h), color);
            }
        }

    }
}
