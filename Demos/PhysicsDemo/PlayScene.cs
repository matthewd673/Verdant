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
            BallEntity ball = new BallEntity(Sprites.Crate, new Vec2(50, 50), 16, 1);
            EntityManager.AddEntity(ball);

            WallEntity wall = new WallEntity(new Vec2(50, 300), new Vec2(450, 250), Color.Yellow);
            wall.Elasticity = 1;
            EntityManager.AddEntity(wall);

            CapsuleEntity capsule = new CapsuleEntity(Sprites.Crate, new Vec2(600, 200), 20, 120, 1);
            EntityManager.AddEntity(capsule);

            BoxEntity box = new BoxEntity(Sprites.Crate, new Vec2(200, 50), 40, 120, 1);
            EntityManager.AddEntity(box);
        }

    }
}
