using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class Particle : Entity
    {

        int lifetime;

        public Particle(Texture2D sprite, Vec2 pos, int w, int h, int lifetime) : base(sprite, pos, w, h)
        {
            this.lifetime = lifetime;
            hasPhysics = true;
        }

        public override void Update()
        {
            lifetime--;
            if (lifetime <= 0)
                return;
            base.Update();
        }

        public bool IsDead()
        {
            return lifetime <= 0;
        }

    }
}
