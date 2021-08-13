using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class ParticleSystem : Entity
    {

        Random rng;
        List<Particle> particles = new List<Particle>();

        int particleSpreadRadius;

        bool autoRemove;

        public ParticleSystem(Vec2 pos, int particleSpreadRadius, bool autoRemove = true) : base(Renderer.GetPixelSprite(), pos, 0, 0)
        {
            rng = new Random();
            this.particleSpreadRadius = particleSpreadRadius;
            this.autoRemove = autoRemove;
        }

        public Vec2 GetNewParticlePos()
        {
            float pX = pos.x + rng.Next(0, particleSpreadRadius * 2) - particleSpreadRadius;
            float pY = pos.y + rng.Next(0, particleSpreadRadius * 2) - particleSpreadRadius;
            return new Vec2(pX, pY);
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
        }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

    }
}
