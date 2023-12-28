using System;
using Verdant;
using Verdant.Physics;
namespace PhysicsDemo
{
    public class CapsuleController : CapsuleEntity
    {
        public CapsuleController(Vec2 position, float radius, float height, float mass)
            : base(position, Sprites.CapsuleEntity, radius, height, mass)
        {
            AngleFriction = 0.05f;
            Friction = 0.05f;
        }

        public override void Update()
        {
            SimpleInput();
            base.Update();
        }
    }
}

