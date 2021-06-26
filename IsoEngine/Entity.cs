using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class Entity
    {

        EntityManager manager;

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

        public void SetManager(EntityManager manager)
        {
            this.manager = manager;
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

        /// <summary>
        /// Move the Entity with the given deltas until it hits a solid Collider. NOTE: The Entity must have at least one Collider (solid or trigger).
        /// </summary>
        /// <param name="xDelta">The distance to move on the x axis.</param>
        /// <param name="yDelta">The distance to move on the y axis.</param>
        /// <param name="moveCollider">The specific Collider to check against. Otherwise, the first Collider added will be used (whether it is a trigger or solid).</param>
        /// <param name="discreteSteps">The number of discrete steps to take when checking collisions. Increase for fast objects.</param>
        public void Move(float xDelta, float yDelta, Collider moveCollider = null, int discreteSteps = 4)
        {

            if (colliders.Count == 0) //do nothing if there is no collider attached (while it could just move regardless, doing so would break the promise of not colliding imo, easier to debug too).
                return;

            if (moveCollider == null)
                moveCollider = colliders[0];

            float newX = moveCollider.pos.x;
            float newY = moveCollider.pos.y;
            float xInc = xDelta / discreteSteps;
            float yInc = yDelta / discreteSteps;

            float xTotalMove = 0f;
            float yTotalMove = 0f;

            bool collidedOnX = false;
            bool collidedOnY = false;
            for (int i = 0; i < discreteSteps; i++)
            {
                if (!collidedOnX && manager.CheckRectCollisions(newX + xInc, newY, moveCollider.w, moveCollider.h, onlySolids: true, ignoreEntity: this).Count == 0) //all clear on X
                {
                    newX += xInc;
                    xTotalMove += xInc;
                }
                else
                    collidedOnX = true;

                if (!collidedOnY && manager.CheckRectCollisions(newX, newY + yInc, moveCollider.w, moveCollider.h, onlySolids: true, ignoreEntity: this).Count == 0) //all clear on Y
                {
                    newY += yInc;
                    yTotalMove += yInc;
                }
                else
                    collidedOnY = true;

                //skip the rest if colliding on both
                if (collidedOnX && collidedOnY)
                    break;
            }

            pos.x += xTotalMove;
            pos.y += yTotalMove;

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
