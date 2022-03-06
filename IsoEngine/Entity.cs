using System;
using IsoEngine.Physics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace IsoEngine
{

    [Serializable]
    public class Entity : Body
    {
        
        [JsonIgnore]
        EntityManager _manager;
        [JsonIgnore]
        public EntityManager Manager
        {   get { return _manager; }
            set
            {
                _manager = value;
                if (_manager != null)
                    Key = _manager.GetKeyFromPos(Position); //set initial key if manager isn't null
            }
        }
        public string Key { get; private set; }
        public string PreviousKey { get; private set; } = "";

        //public Vec2 Position { get; set; }
        [JsonIgnore]
        public RenderObject Sprite { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool HasPhysics { get; protected set; }
        //public bool MoveSafelyWithPhysics { get; protected set; }
        //protected int PhysicsMovementDiscreteSteps { get; set; }

        //public float Friction { get; set; } = 0f;
        //public Vec2 Velocity { get; set; } = Vec2.Zero;
        //public Vec2 Acceleration { get; set; } = Vec2.Zero;

        public float Rotation { get; set; } = 0f;
        protected Vec2Int RotationOrigin { get; set; } = Vec2Int.Zero;

        [JsonIgnore]
        public List<Collider> Colliders { get; set; } = new List<Collider>();

        protected bool SetZIndexToBase { get; set; }
        public int ZIndex { get; protected set; } = 0;

        public bool ForRemoval { get; set; } = false;

        /// <summary>
        /// Initialize a new Entity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="position">The position of the Entity.</param>
        /// <param name="width">The width of the Entity. Defaults to the width of the RenderObject.</param>
        /// <param name="height">The height of the Entity. Defaults to the height of the RenderObject.</param>
        public Entity(RenderObject sprite, Vec2 position, int width, int height) :
            base()
        {
            //apply default properties
            HasPhysics = false;
            SetZIndexToBase = true;

            Sprite = sprite;
            Position = position;
            Width = width;
            Height = height;

            //set automatic rotation origin
            //TODO: when working with textures stretched to different aspect ratios, this will result in an off-center origin
            RotationOrigin = new Vec2Int(Width / 2, Height / 2);
        }

        protected virtual void InitializeBody() { }

        /// <summary>
        /// Create a Collider encompassing the Entity's bounding box.
        /// </summary>
        /// <param name="trigger">Determines if the Collider is a trigger or not.</param>
        public void AddSimpleCollider(bool trigger = false)
        {
            //Colliders.Add(new Collider(this, trigger: trigger, relativePos: true));
        }

        /// <summary>
        /// Perform the Entity's basic update loop: performing physics movement, updating child Colliders, and updating the z-index if appropriate.
        /// </summary>
        public virtual void Update()
        {
            //apply physics
            //if (HasPhysics)
            //{
            //    if (!MoveSafelyWithPhysics)
            //        Position += Velocity;
            //    else
            //        Move(Velocity.X, Velocity.Y, discreteSteps: PhysicsMovementDiscreteSteps);

            //    if (Acceleration != Vec2.Zero)
            //    {
            //        Velocity += Acceleration - (Velocity * Friction);
            //        Acceleration *= Friction;
            //    }
            //}

            //move body
            Move();

            //update key
            if (Manager != null) //only if a managed entity (not Particles, for example)
            {
                PreviousKey = Key;
                Key = Manager.GetKeyFromPos(Position);
            }

            //update all colliders
            //foreach (Collider c in Colliders)
            //    c.Update();

            //update z index
            if (SetZIndexToBase)
                ZIndex = (int)(Position.Y + Height);

        }

        /// <summary>
        /// Move the Entity with the given deltas until it hits a solid Collider. NOTE: The Entity must have at least one Collider (solid or trigger).
        /// </summary>
        /// <param name="xDelta">The distance to move on the x axis.</param>
        /// <param name="yDelta">The distance to move on the y axis.</param>
        /// <param name="moveCollider">The specific Collider to check against. Otherwise, the first Collider added will be used (whether it is a trigger or solid).</param>
        /// <param name="discreteSteps">The number of discrete steps to take when checking collisions. Increase for fast objects.</param>
        /// <param name="moveThroughIfColliding">If the Entity is already colliding with another Entity when it begins to move, allow the Entity to pass through it.</param>
        //public void Move(float xDelta, float yDelta, Collider moveCollider = null, int discreteSteps = 4, bool moveThroughIfColliding = false)
        //{

        //    if (Colliders.Count == 0) //do nothing if there is no collider attached (while it could just move regardless, doing so would break the promise of not colliding imo, easier to debug too).
        //        return;

        //    if (moveCollider == null)
        //        moveCollider = Colliders[0];

        //    float newX = moveCollider.Position.X;
        //    float newY = moveCollider.Position.Y;
        //    float xInc = xDelta / discreteSteps;
        //    float yInc = yDelta / discreteSteps;

        //    float xTotalMove = 0f;
        //    float yTotalMove = 0f;

        //    bool collidedOnX = false;
        //    bool collidedOnY = false;

        //    List<Entity> preColliding = new List<Entity>();
        //    if (moveThroughIfColliding)
        //        preColliding = Manager.GetAllColliding(this, moveCollider, onlySolids: true);

        //    for (int i = 0; i < discreteSteps; i++)
        //    {
        //        if (!collidedOnX) //all clear on X
        //        {
        //            List<Entity> collided = Manager.CheckRectCollisions(newX + xInc, newY, moveCollider.Width, moveCollider.Height, onlySolids: true, ignoreList: new List<Entity> { this });

        //            if (!moveThroughIfColliding && collided.Count == 0)
        //            {
        //                newX += xInc;
        //                xTotalMove += xInc;
        //            }
        //            else
        //            {
        //                foreach (Entity e in preColliding)
        //                    collided.Remove(e);
        //                if (collided.Count == 0)
        //                {
        //                    newX += xInc;
        //                    xTotalMove += xInc;
        //                }
        //            }
        //        }
        //        else
        //            collidedOnX = true;

        //        if (!collidedOnY) //all clear on Y
        //        {
        //            List<Entity> collided = Manager.CheckRectCollisions(newX, newY + yInc, moveCollider.Width, moveCollider.Height, onlySolids: true, ignoreList: new List<Entity> { this });

        //            if (!moveThroughIfColliding && collided.Count == 0)
        //            {

        //                newY += yInc;
        //                yTotalMove += yInc;
        //            }
        //            else
        //            {
        //                foreach (Entity e in preColliding)
        //                    collided.Remove(e);
        //                if (collided.Count == 0)
        //                {
        //                    newY += yInc;
        //                    yTotalMove += yInc;
        //                }
        //            }
        //        }
        //        else
        //            collidedOnY = true;

        //        //skip the rest if colliding on both
        //        if (collidedOnX && collidedOnY)
        //            break;
        //    }

        //    Position.X += xTotalMove;
        //    Position.Y += yTotalMove;

        //}

        /// <summary>
        /// Set the Entity's bounds and rotation to be equal to those of a given TransformState.
        /// </summary>
        /// <param name="state">The TransformState to mirror.</param>
        public void ApplyTransformState(TransformState state)
        {
            Position.X = state.X;
            Position.Y = state.Y;
            Width = (int)state.Width;
            Height = (int)state.Height;
            Rotation = state.Rotation;
        }

        /// <summary>
        /// Perfom a basic render of the Entity.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite == null)
                return;

            if (Rotation == 0f) //no rotation, simple draw
                spriteBatch.Draw(Sprite.Get(), Renderer.Camera.GetRenderBounds(this), Color.White);
            else
            {
                Microsoft.Xna.Framework.Rectangle renderRect = Renderer.Camera.GetRenderBounds(this);
                renderRect.X += RotationOrigin.X * Renderer.Scale;
                renderRect.Y += RotationOrigin.Y * Renderer.Scale;
                spriteBatch.Draw(Sprite.Get(), renderRect, null, Color.White, Rotation, (Vector2)RotationOrigin * Renderer.Scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draws all of the Colliders attached to the Entity (for debug). Yellow = Solid, Green = Trigger.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public void DrawColliders(SpriteBatch spriteBatch)
        {
            //foreach (Collider c in Colliders)
            //{
            //    //color code
            //    Color color = Color.Yellow;
            //    if (c.Trigger)
            //        color = Color.LimeGreen;

            //    //top line
            //    Rectangle topLineBounds = Renderer.Camera.GetRenderBounds(c.Position, c.Width, c.Height);
            //    topLineBounds.Height = 1;
            //    spriteBatch.Draw(Renderer.GetPixel(), topLineBounds, color);
            //    //bottom line
            //    Rectangle bottomLineBounds = Renderer.Camera.GetRenderBounds(c.Position.X, c.Position.Y + c.Height, c.Width, 1);
            //    bottomLineBounds.Width += 1;
            //    bottomLineBounds.Height = 1;
            //    spriteBatch.Draw(Renderer.GetPixel(), bottomLineBounds, color);
            //    //left line
            //    Rectangle leftLineBounds = Renderer.Camera.GetRenderBounds(c.Position, 1, c.Height);
            //    leftLineBounds.Width = 1;
            //    spriteBatch.Draw(Renderer.GetPixel(), leftLineBounds, color);
            //    //right line
            //    Rectangle rightLineBounds = Renderer.Camera.GetRenderBounds(c.Position.X + c.Width, c.Position.Y, 1, c.Height);
            //    rightLineBounds.Width = 1;
            //    spriteBatch.Draw(Renderer.GetPixel(), rightLineBounds, color);
            //}
        }

    }
}
