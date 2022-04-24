using Verdant;
using Verdant.Physics;

namespace PhysicsDemo
{
    internal class BallController : BallEntity
    {

        public BallController(Vec2 pos, float radius, float mass) : base(null, pos, radius, mass) { }

        public override void Update()
        {
            // each physics entity class contains a simple input
            // method to get them up and running for testing
            SimpleInput();
            base.Update();
        }

    }
}
