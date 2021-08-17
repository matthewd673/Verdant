using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class ParticleSystem : Entity
    {

        List<Particle> particles = new List<Particle>();
        int particleSpreadRadius;
        bool autoRemove;

        /// <summary>
        /// Initialize a new ParticleSystem.
        /// </summary>
        /// <param name="pos">The position of the system.</param>
        /// <param name="particleSpreadRadius">The radius that GetNewParticlePos will spawn within.</param>
        /// <param name="autoRemove">Determine if dead particles should be removed automatically.</param>
        public ParticleSystem(Vec2 pos, int particleSpreadRadius, bool autoRemove = true) : base(Renderer.GetPixelSprite(), pos, 0, 0)
        {
            this.particleSpreadRadius = particleSpreadRadius;
            this.autoRemove = autoRemove;
        }

        /// <summary>
        /// Get a new, random spawn point within the spawn radius. The spawn position is NOT relative to the ParticleSystem's position.
        /// </summary>
        /// <returns>A Vec2 representing a new possible spawn position.</returns>
        public Vec2 GetNewParticlePos()
        {
            float pX = Position.X + Math.Random.Next(0, particleSpreadRadius * 2) - particleSpreadRadius;
            float pY = Position.Y + Math.Random.Next(0, particleSpreadRadius * 2) - particleSpreadRadius;
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
