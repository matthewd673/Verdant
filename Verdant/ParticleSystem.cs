using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    public class ParticleSystem : Entity
    {

        List<Particle> particles = new List<Particle>();
        public int SpreadRadius { get; set; }
        public bool AutoRemove { get; set; }

        // particle settings
        protected RenderObject[] Sprites { get; set; }
        protected int[] WidthRange;
        protected int[] HeightRange;
        protected float[] AngleRange;
        protected float[] VelocityMagnitudeRange;
        protected float[] AccelerationMagnitudeRange;
        protected float[] FrictionRange;
        protected int[] LifetimeRange;

        /// <summary>
        /// Initialize a new ParticleSystem.
        /// </summary>
        /// <param name="pos">The position of the system.</param>
        /// <param name="spreadRadius">The radius that GetNewParticlePos will spawn within.</param>
        /// <param name="autoRemove">Determine if dead particles should be removed automatically.</param>
        public ParticleSystem(Vec2 pos, int spreadRadius, bool autoRemove = true) : base(Renderer.PixelSprite, pos, 0, 0)
        {
            SpreadRadius = spreadRadius;
            AutoRemove = autoRemove;
        }

        public void SpawnParticle()
        {
            //determine particle spawn values
            Vec2 spawnPos = GetNewParticlePos();
            RenderObject sprite = Sprites[GameMath.Random.Next(Sprites.Length)]; //pick a random sprite from the list
            int width = SelectIntFromRange(WidthRange);
            int height = SelectIntFromRange(HeightRange);
            float angle = SelectFloatFromRange(AngleRange); //get random angle in range set by array
            float velocityMagnitude = SelectFloatFromRange(VelocityMagnitudeRange);
            float accelerationMagnitude = SelectFloatFromRange(AccelerationMagnitudeRange);
            float friction = SelectFloatFromRange(FrictionRange);
            int lifetime = SelectIntFromRange(LifetimeRange);

            //create particle
            Particle particle = new Particle(sprite, spawnPos, width, height, lifetime);
            particle.Velocity = GameMath.Vec2FromAngle(angle) * velocityMagnitude;
            particle.Acceleration = GameMath.Vec2FromAngle(angle) * accelerationMagnitude;
            particle.Friction = friction;
            particles.Add(particle);
        }

        /// <summary>
        /// Get a new, random spawn point within the spawn radius. The spawn position is NOT relative to the ParticleSystem's position.
        /// </summary>
        /// <returns>A Vec2 representing a new possible spawn position.</returns>
        public Vec2 GetNewParticlePos()
        {
            float pX = Position.X + GameMath.Random.Next(0, SpreadRadius * 2) - SpreadRadius;
            float pY = Position.Y + GameMath.Random.Next(0, SpreadRadius * 2) - SpreadRadius;
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

            if (AutoRemove && particles.Count == 0)
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
