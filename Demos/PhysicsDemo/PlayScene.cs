using IsoEngine;
using IsoEngine.Physics;
using System;
using Microsoft.Xna.Framework;

namespace PhysicsDemo
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        public override void Initialize()
        {
            base.Initialize();

            //EntityManager.AddEntity(new Crate(new Vec2(20, 20), 32, 32, 5));
            BallController ball = new BallController(new Vec2(50, 50), 16, 2);
            ball.Speed = 0.8f;
            ball.Friction = 0.05f;
            EntityManager.AddEntity(ball);

            WallEntity wall = new WallEntity(new Vec2(50, 300), new Vec2(450, 250));
            //EntityManager.AddEntity(wall);

            CapsuleEntity capsule = new CapsuleEntity(Sprites.CapsuleEntity, new Vec2(600, 200), 20, 120, 1);
            EntityManager.AddEntity(capsule);

            BoxEntity box = new BoxEntity(Sprites.BoxEntity, new Vec2(200, 50), 40, 120, 1);
            box.Mass = 0;
            box.AngleFriction = 0.5f;
            EntityManager.AddEntity(box);
        }

    }

    class BallController : BallEntity
    {

        public BallController(Vec2 pos, float radius, float mass) : base(null, pos, radius, mass) { }

        public override void Update()
        {

            SimpleInput();

            base.Update();
        }

    }
}
