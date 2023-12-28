using Verdant;
using Verdant.Physics;
using Microsoft.Xna.Framework;
using Verdant.Debugging;
using System.Linq;

namespace PhysicsDemo
{
    internal class BallController : BallEntity
    {

        public BallController(Vec2 position, float radius, float mass)
            : base(position, Renderer.GenerateCircleSprite((int)radius, Color.Salmon), radius, mass)
        {
            // Empty
        }

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
