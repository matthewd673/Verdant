using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Verdant;
using Verdant.Physics;

namespace TopdownShooter
{
    internal class Enemy : BallEntity
    {

        static float speed = 2;

        List<Vec2> path = new();

        private int _health = 3;
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                if (_health <= 0)
                    ForRemoval = true;
            }
        }

        public Enemy(Vec2 position) : base(Resources.Enemy, position, 16, 1) { }

        public override void Update()
        {
            path = ((PlayScene)Manager.Scene).Pathfinder.FindPath(this, ((PlayScene)Manager.Scene).Player);
            if (path.Count > 1)
            {
                float angle = GameMath.AngleBetweenPoints(Position, path[1]);
                Velocity = GameMath.Vec2FromAngle(angle) * speed;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Renderer.DrawLine(spriteBatch, Manager.Scene.Camera, path[i], path[i + 1], Color.Blue);
            }
        }

    }
}
