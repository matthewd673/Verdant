using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class ParticleSystem : Entity
    {

        List<Particle> particles = new List<Particle>();
        public int SpreadRadius { get; set; }
        bool autoRemove;

        /// <summary>
        /// Initialize a new ParticleSystem.
        /// </summary>
        /// <param name="pos">The position of the system.</param>
        /// <param name="spreadRadius">The radius that GetNewParticlePos will spawn within.</param>
        /// <param name="autoRemove">Determine if dead particles should be removed automatically.</param>
        public ParticleSystem(Vec2 pos, int spreadRadius, bool autoRemove = true) : base(Renderer.GetPixelSprite(), pos, 0, 0)
        {
            SpreadRadius = spreadRadius;
            this.autoRemove = autoRemove;
        }

        public void SpawnParticle(ParticleConfiguration configuration)
        {
            //determine particle spawn values
            Vec2 spawnPos = GetNewParticlePos();
            RenderObject sprite = configuration.Sprites[Math.Random.Next(configuration.Sprites.Length)]; //pick a random sprite from the list
            int width = Math.Random.Next(configuration.WidthRange[0], configuration.WidthRange[configuration.WidthRange.Length - 1]);
            int height = Math.Random.Next(configuration.HeightRange[0], configuration.HeightRange[configuration.HeightRange.Length - 1]);
            float angle = Math.RandomFloat(configuration.AngleRange[0], configuration.AngleRange[configuration.AngleRange.Length - 1]); //get random angle in range set by array
            float velocityMagnitude = Math.RandomFloat(configuration.VelocityMagnitudeRange[0], configuration.VelocityMagnitudeRange[configuration.VelocityMagnitudeRange.Length - 1]);
            float accelerationMagnitude = Math.RandomFloat(configuration.AccelerationMagnitudeRange[0], configuration.AccelerationMagnitudeRange[configuration.AccelerationMagnitudeRange.Length - 1]);
            float friction = Math.RandomFloat(configuration.FrictionRange[0], configuration.FrictionRange[configuration.FrictionRange.Length - 1]);
            int lifetime = Math.Random.Next(configuration.LifetimeRange[0], configuration.LifetimeRange[configuration.LifetimeRange.Length - 1]);

            //create particle
            Particle particle = new Particle(sprite, spawnPos, width, height, lifetime);
            particle.Velocity = Math.Vec2FromAngle(angle) * velocityMagnitude;
            particle.Acceleration = Math.Vec2FromAngle(angle) * accelerationMagnitude;
            particle.Friction = friction;
            particles.Add(particle);
        }

        /// <summary>
        /// Get a new, random spawn point within the spawn radius. The spawn position is NOT relative to the ParticleSystem's position.
        /// </summary>
        /// <returns>A Vec2 representing a new possible spawn position.</returns>
        public Vec2 GetNewParticlePos()
        {
            float pX = Position.X + Math.Random.Next(0, SpreadRadius * 2) - SpreadRadius;
            float pY = Position.Y + Math.Random.Next(0, SpreadRadius * 2) - SpreadRadius;
            return new Vec2(pX, pY);
        }

        /// <summary>
        /// Add a Particle to the system.
        /// </summary>
        /// <param name="particle">The Particle to add.</param>
        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
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
                if (particles[i].IsDead())
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            if (autoRemove && particles.Count == 0)
                MarkForRemoval();

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

    }
}
