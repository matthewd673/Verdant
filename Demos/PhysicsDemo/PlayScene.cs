using IsoEngine;
using IsoEngine.Physics;
using System;

namespace PhysicsDemo
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        public override void Initialize()
        {
            base.Initialize();

            //EntityManager.AddEntity(new Crate(new Vec2(20, 20), 32, 32, 5));
            BallEntity ball = new BallEntity(Sprites.Crate, new Vec2(50, 50), 16, 1);
            EntityManager.AddEntity(ball);
        }

    }
}
