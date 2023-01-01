using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;
using Verdant.Physics;

namespace TopdownShooter
{
    internal class Projectile : BallEntity
    {

        private static float speed = 4;

        public Projectile(Vec2 position, float angle) : base(Resources.Bullet, position, 4, 1)
        {
            Velocity = GameMath.Vec2FromAngle(angle) * speed;
            Trigger = true;
        }

        public override void Update()
        {
            base.Update();

            foreach (Enemy e in GetColliding<Enemy>())
            {
                e.Health -= 1;
                ForRemoval = true;
            }

            foreach (Wall w in GetColliding<Wall>())
            {
                ForRemoval = true;
            }
        }

    }
}
