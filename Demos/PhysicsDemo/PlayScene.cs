using Verdant;
using Verdant.Physics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo
{
    internal class PlayScene : Scene
    {

        public PlayScene(string id) : base(id)
        {
            // Empty
        }

        public override void Initialize()
        {
            base.Initialize();

            EntityManager = new(1000)
            {
                Scene = this,
            };

            BallController ball = new(new Vec2(50, 50), 20, 20)
            {
                Speed = 0.8f,
                Friction = 0.05f,
            };
            EntityManager.AddEntity(ball);

            WallEntity wall = new(new Vec2(50, 300), new Vec2(450, 250));
            EntityManager.AddEntity(wall);

            CapsuleEntity capsule = new(Sprites.CapsuleEntity, new Vec2(600, 200), 20, 120, 5)
            {
                AngleFriction = 0f,
            };
            EntityManager.AddEntity(capsule);

            //CapsuleController capsule = new(new Vec2(600, 200), 20, 120, 5);
            //EntityManager.AddEntity(capsule);

            BoxEntity box = new(Sprites.BoxEntity, new Vec2(200, 50), 40, 120, 0);
            box.AngleFriction = 0.5f;
            EntityManager.AddEntity(box);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Renderer.Render(spriteBatch, this);
        }

    }
}
