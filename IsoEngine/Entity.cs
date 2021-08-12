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
        public IRenderObject sprite;
        public int w;
        public int h;

        protected bool hasPhysics = false;
        protected bool moveSafelyWithPhysics = false;
        protected float friction = 0f;
        protected Vec2 velocity = Vec2.zero;
        protected Vec2 acceleration = Vec2.zero;

        List<Collider> colliders = new List<Collider>();

        protected bool setZIndexToBase = false;
        public int zIndex = 0;

        bool forRemoval = false;

        /// <summary>
        /// Initialize a new Entity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="pos">The position of the Entity.</param>
        /// <param name="w">The width of the Entity.</param>
        /// <param name="h">The height of the Entity.</param>
        public Entity(IRenderObject sprite, Vec2 pos, int w, int h)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.w = w;
            this.h = h;
        }

        /// <summary>
        /// Set the Entity's current friction.
        /// </summary>
        /// <param name="friction">The friction value, typically between 0 and 1.</param>
        public void SetFriction(float friction)
        {
            this.friction = friction;
        }

        /// <summary>
        /// Set the Entity's current velocity.
        /// </summary>
        /// <param name="velocity">The velocity value, represented as a Vec2.</param>
        public void SetVelocity(Vec2 velocity)
        {
            this.velocity = velocity;
        }

        /// <summary>
        /// Get the Entity's current velocity.
        /// </summary>
        /// <returns>The current velocity, represented as a Vec2.</returns>
        public Vec2 GetVelocity()
        {
            return velocity;
        }

        /// <summary>
        /// Set the Entity's current acceleration.
        /// </summary>
        /// <param name="acceleration">The acceleration value, represented as a Vec2.</param>
        public void SetAcceleration(Vec2 acceleration)
        {
            this.acceleration = acceleration;
        }

        /// <summary>
        /// Get the Entity's current acceleration.
        /// </summary>
        /// <returns>The current acceleration, represented as a Vec2.</returns>
        public Vec2 GetAcceleration()
        {
            return acceleration;
        }

        /// <summary>
        /// Manually set the Entity's parent EntityManager. This is performed automatically when adding an Entity to an EntityManager.
        /// </summary>
        /// <param name="manager">The EntityManager to mark as parent.</param>
        public void SetManager(EntityManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Mark this Entity to be removed from the EntityManager at the end of the update loop.
        /// </summary>
        public void MarkForRemoval()
        {
            forRemoval = true;
        }

        /// <summary>
        /// Check if this Entity is marked for removal from the EntityManager.
        /// </summary>
        /// <returns>Returns true if the Entity is marked for removal.</returns>
        public bool IsForRemoval()
        {
            return forRemoval;
        }

        /// <summary>
        /// Create a Collider encompassing the Entity's bounding box.
        /// </summary>
        /// <param name="trigger">Determines if the Collider is a trigger or not.</param>
        public void AddSimpleCollider(bool trigger = false)
        {
            AddCollider(new Collider(this, trigger: trigger, relativePos: true));
        }

        /// <summary>
        /// Add a Collider to the Entity.
        /// </summary>
        /// <param name="c">The Collider to add.</param>
        public void AddCollider(Collider c)
        {
            colliders.Add(c);
        }

        /// <summary>
        /// Remove a Collider from the Entity.
        /// </summary>
        /// <param name="c">The Collider to remove.</param>
        public void RemoveCollider(Collider c)
        {
            colliders.Remove(c);
        }

        /// <summary>
        /// Remove all Colliders from the Entity.
        /// </summary>
        public void ClearColliders()
        {
            colliders.Clear();
        }

        /// <summary>
        /// Get a list of all the Colliders currently attached to the Entity.
        /// </summary>
        /// <returns>A list of all Colliders on the Entity.</returns>
        public List<Collider> GetColliders()
        {
            return colliders;
        }

        /// <summary>
        /// Perform the Entity's basic update loop: performing physics movement, updating child Colliders, and updating the z-index if appropriate.
        /// </summary>
        public virtual void Update()
        {
            if (hasPhysics && velocity != null)
            {
                if (!moveSafelyWithPhysics)
                    pos += velocity;
                else
                    Move(velocity.x, velocity.y);
                
                if (acceleration != null)
                {
                    velocity += acceleration - (velocity * friction);
                    acceleration *= friction;
                }
            }

            foreach (Collider c in colliders)
                c.Update();

            if (setZIndexToBase)
                zIndex = (int)(pos.y + h);

        }

        /// <summary>
        /// Move the Entity with the given deltas until it hits a solid Collider. NOTE: The Entity must have at least one Collider (solid or trigger).
        /// </summary>
        /// <param name="xDelta">The distance to move on the x axis.</param>
        /// <param name="yDelta">The distance to move on the y axis.</param>
        /// <param name="moveCollider">The specific Collider to check against. Otherwise, the first Collider added will be used (whether it is a trigger or solid).</param>
        /// <param name="discreteSteps">The number of discrete steps to take when checking collisions. Increase for fast objects.</param>
        /// <param name="moveThroughIfColliding">If the Entity is already colliding with another Entity when it begins to move, allow the Entity to pass through it.</param>
        public void Move(float xDelta, float yDelta, Collider moveCollider = null, int discreteSteps = 4, bool moveThroughIfColliding = false)
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

            List<Entity> preColliding = new List<Entity>();
            if (moveThroughIfColliding)
                preColliding = manager.GetAllColliding(this, moveCollider, onlySolids: true);

            for (int i = 0; i < discreteSteps; i++)
            {
                if (!collidedOnX) //all clear on X
                {
                    List<Entity> collided = manager.CheckRectCollisions(newX + xInc, newY, moveCollider.w, moveCollider.h, onlySolids: true, ignoreEntity: this);

                    if (!moveThroughIfColliding && collided.Count == 0)
                    {
                        newX += xInc;
                        xTotalMove += xInc;
                    }
                    else
                    {
                        foreach (Entity e in preColliding)
                            collided.Remove(e);
                        if (collided.Count == 0)
                        {
                            newX += xInc;
                            xTotalMove += xInc;
                        }
                    }
                }
                else
                    collidedOnX = true;

                if (!collidedOnY) //all clear on Y
                {
                    List<Entity> collided = manager.CheckRectCollisions(newX, newY + yInc, moveCollider.w, moveCollider.h, onlySolids: true, ignoreEntity: this);

                    if (!moveThroughIfColliding && collided.Count == 0)
                    {

                        newY += yInc;
                        yTotalMove += yInc;
                    }
                    else
                    {
                        foreach (Entity e in preColliding)
                            collided.Remove(e);
                        if (collided.Count == 0)
                        {
                            newY += yInc;
                            yTotalMove += yInc;
                        }
                    }
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

        /// <summary>
        /// Perfom a basic render of the Entity.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Get(), Renderer.GetRenderBounds(this), Color.White);
        }

        /// <summary>
        /// Draws all of the Colliders attached to the Entity (for debug). Yellow = Solid, Green = Trigger.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public void DrawColliders(SpriteBatch spriteBatch)
        {
            foreach (Collider c in colliders)
            {
                //color code
                Color color = Color.Yellow;
                if (c.trigger)
                    color = Color.LimeGreen;

                //top line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(c.pos, c.w, 1), color);
                //bottom line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(c.pos.x, c.pos.y + c.h, c.w + 1, 1), color);
                //left line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(c.pos, 1, c.h), color);
                //right line
                spriteBatch.Draw(Renderer.pixel, Renderer.GetRenderBounds(c.pos.x + c.w, c.pos.y, 1, c.h), color);
            }
        }

    }
}
