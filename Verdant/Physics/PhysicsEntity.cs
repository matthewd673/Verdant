using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{

    /// <summary>
    /// An Entity with collider and additional physical properties, which can collide with other PhysicsEntities and be simulated by the EntityManager.
    /// </summary>
    public class PhysicsEntity : Entity
    {
        // The Shapes that make up the PhysicsEntity's collider.
        public Shape[] Components { get; set; } = new Shape[0];

        // The position of the PhysicsEntity (the center of its first Shape component).
        public override Vec2 Position
        {
            get
            {
                if (Components.Length > 0) //the important one is always at index 0
                    return Components[0].Position;
                return null;
            }
            set
            {
                if (Components.Length == 1)
                    Components[0].Position = value;
            }
        }

        private float _mass;
        // The mass of the PhysicsEntity.
        public float Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                if (value == 0)
                    InvMass = 0;
                else
                    InvMass = 1 / value;
            }
        }
        internal float InvMass { get; private set; }

        private float _inertia;
        // The inertia of the PhysicsEntity, calculated from mass.
        public float Inertia
        {
            get { return _inertia; }
            set
            {
                _inertia = value;
                if (value == 0)
                    InvInertia = 0;
                else
                    InvInertia = 1 / value;
            }
        }
        internal float InvInertia { get; private set; }

        // The velocity of the PhysicsEntity.
        public Vec2 Velocity { get; set; } = new Vec2(0, 0);
        // The acceleration of the PhysicsEntity.
        public Vec2 Acceleration { get; set; } = new Vec2(0, 0);

        // The speed multiplier applied to acceleration when the PhysicsEntity moves.
        public float Speed { get; set; } = 1f;
        // The friction of the PhysicsEntity.
        public float Friction { get; set; }
        // The speed at which the PhysicsEntity is currently rotating.
        public float AngleSpeed { get; set; }
        // The friction of the PhysicsEntity's rotation.
        public float AngleFriction { get; set; }
        // The elasticity of the PhysicsEntity when a collision is resolved.
        public float Elasticity { get; set; } = 1f;
        // Determines if the PhysicsEntity is a trigger. Collisions with triggers will not be resolved, but will still be registered.
        public bool Trigger { get; set; } = false;
        // The color to render the PhysicsEntity's Shape components when visualizing bodies.
        public Color BodyColor { get; set; } = Color.Yellow;

        internal List<PhysicsEntity> Colliding { get; private set; } = new();

        /// <summary>
        /// Initialize a new PhysicsEntity. By default, it will have a mass but no Components.
        /// In most cases, it is more appropriate to use an extension like a BallEntity or BoxEntity.
        /// </summary>
        public PhysicsEntity(RenderObject sprite, Vec2 position, float width, float height, float mass)
                : base(sprite, position, (int)width, (int)height)
        {
            Mass = mass;
        }

        /// <summary>
        /// Perform physics movement for the Body.
        /// </summary>
        public virtual void Move()
        {
            Acceleration = Acceleration.Unit() * Speed; // TODO: this speed thing is weird
            Velocity += Acceleration;
            Velocity *= 1 - Friction;

            Components[0].Position += Velocity;
        }

        /// <summary>
        /// Get a list of all PhysicsEntities of a given type currently colliding with this Entity.
        /// </summary>
        /// <typeparam name="TPhysicsEntity">The type of PhysicsEntity to check for.</typeparam>
        /// <param name="includeTriggers">Include PhysicsEntities that are triggers.</param>
        /// <param name="includeSolids">Include PhysicsEntities that are not triggers.</param>
        /// <returns>A list of all PhyiscsEntities currently colliding with this one.</returns>
        public List<TPhysicsEntity> GetColliding<TPhysicsEntity>(bool includeTriggers = true, bool includeSolids = true) where TPhysicsEntity : PhysicsEntity // largely copied from GetAllColliding
        {
            List<TPhysicsEntity> colliding = new List<TPhysicsEntity>();

            foreach (PhysicsEntity p in Colliding)
            {
                if (!p.IsType(typeof(TPhysicsEntity)))
                    continue;

                if (includeTriggers && p.Trigger)
                {
                    colliding.Add((TPhysicsEntity)p);
                    continue;
                }

                if (includeSolids && !p.Trigger)
                {
                    colliding.Add((TPhysicsEntity)p);
                    continue;
                }

                colliding.Add((TPhysicsEntity)p);
            }

            return colliding;
        }

        /// <summary>
        /// Get a list of all PhysicsEntities currently colliding with this Entity.
        /// </summary>
        /// <param name="includeTriggers">Include PhysicsEntities that are triggers.</param>
        /// <param name="includeSolids">Include PhysicsEntities that are not triggers.</param>
        /// <returns>A list of all PhysicsEntities currently colliding with this one.</returns>
        public List<PhysicsEntity> GetColliding(bool includeTriggers = true, bool includeSolids = true)
        {
            return GetColliding<PhysicsEntity>(includeTriggers, includeSolids);
        }

        /// <summary>
        /// Visualize the Components of the Body according to the BodyColor.
        /// The default implementation is only intended for debug purposes as
        /// some draw calls, particularly for Circles are highly inefficient.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void DrawBody(SpriteBatch spriteBatch)
        {
            foreach (Shape s in Components)
            {
                s.Draw(spriteBatch, Manager.Scene.Camera, BodyColor);
            }
        }

    }
}
