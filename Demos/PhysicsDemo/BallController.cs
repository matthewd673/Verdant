using Verdant;
using Verdant.Physics;
using Microsoft.Xna.Framework;
using Verdant.Debugging;
using System.Linq;

namespace PhysicsDemo
{
    internal class BallController : BallEntity
    {

        public BallController(Vec2 pos, float radius, float mass) : base(Renderer.GenerateCircleSprite(radius, Color.Salmon), pos, radius, mass) { }

        public override void Update()
        {
            // each physics entity class contains a simple input
            // method to get them up and running for testing
            SimpleInput();
            base.Update();

            SimpleStats.UpdateField("ball colliding", GetColliding().Count);
        }

    }
}
