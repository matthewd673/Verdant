using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    public class ParticleSystem : Entity
    {

        List<Particle> particles = new List<Particle>();
        
        // Determines if the system should remove itself from its EntityManager when every Particle is dead.
        public bool SelfRemove { get; set; } = true;

        // The radius that Particles may spawn within.
        public float Radius { get; set; } = 0f;

        private bool overrideLifetime = false;
        private float _defaultLifetime;
        public float DefaultLifetime
        { 
            get { return _defaultLifetime; }
            set
            {
                _defaultLifetime = value;
                overrideLifetime = true;
            }
        }

        private bool overrideAcceleration = false;
        private Vec2 _defaultAcceleration = new Vec2(0, 0);
        public Vec2 DefaultAcceleration
        {
            get { return _defaultAcceleration; }
            set
            {
                _defaultAcceleration = value;
                overrideAcceleration = true;
            }
        }

        private bool overrideVelocity = false;
        private Vec2 _defaultVelocity = new Vec2(0, 0);
        public Vec2 DefaultVelocity
        {
            get { return _defaultVelocity; }
            set
            {
                _defaultVelocity = value;
                overrideVelocity = true;
            }
        }

        private bool overrideFriction = false;
        private float _defaultFriction = 0;
        public float DefaultFriction
        {
            get { return _defaultFriction; }
            set
            {
                _defaultFriction = value;
                overrideFriction = true;
            }
        }

        /// <summary>
        /// Initialize a new ParticleSystem.
        /// </summary>
        /// <param name="pos">The position of the system.</param>
        public ParticleSystem(Vec2 pos) : base(Renderer.PixelSprite, pos, 0, 0) { }

        /// <summary>
        /// Spawn a Particle within the system, according to the system's properties.
        /// </summary>
        /// <param name="particle">The Particle to add.</param>
        public void SpawnParticle(Particle particle)
        {
            particle.Position = GenerateParticlePos();
            
            if (overrideLifetime)
                particle.LifeTimer.Duration = DefaultLifetime;
            if (overrideAcceleration)
                particle.Acceleration = DefaultAcceleration.Copy();
            if (overrideVelocity)
                particle.Velocity = DefaultVelocity.Copy();
            if (overrideFriction)
                particle.Friction = DefaultFriction;

            particle.LifeTimer.Start();
            particles.Add(particle);
            particle.System = this;
        }

        /// <summary>
        /// Get a new, random spawn point within the spawn radius. The spawn position is NOT relative to the ParticleSystem's position.
        /// </summary>
        /// <returns>A Vec2 representing a new possible spawn position.</returns>
        private Vec2 GenerateParticlePos()
        {
            float pX = Position.X + GameMath.RandomFloat(-Radius, Radius);
            float pY = Position.Y + GameMath.RandomFloat(-Radius, Radius);
            return new Vec2(pX, pY);
        }

        /// <summary>
        /// Update the ParticleSystem, and all child Particles.
        /// </summary>
        public override void Update()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                //remove dead particles
                if (particles[i].Dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            if (SelfRemove && particles.Count == 0)
                ForRemoval = true;

            base.Update();
        }

        /// <summary>
        /// Draw all Particles in the system.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use when drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }

        private static float SelectFloatFromRange(float[] range)
        {
            return GameMath.RandomFloat(range[0], range[range.Length - 1]);
        }

        private static int SelectIntFromRange(int[] range)
        {
            return GameMath.Random.Next(range[0], range[range.Length - 1]);
        }

    }
}
