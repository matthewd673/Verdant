using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Verdant.Physics;

/// <summary>
/// An Entity with a Capsule body.
/// </summary>
public class CapsuleEntity : PhysicsEntity
{

    /// <summary>
    /// Initialize a new CapsuleEntity.
    /// </summary>
    /// <param name="sprite">The Entity's sprite.</param>
    /// <param name="position">The position of the center of the Entity.</param>
    /// <param name="radius">The radius of the Capsule (half the width of the Rectangle component).</param>
    /// <param name="height">The height of the Rectangle component.</param>
    /// <param name="mass">The mass of the Entity's Body. 0 = infinite mass.</param>
    public CapsuleEntity(RenderObject sprite, Vec2 position, float radius, float height, float mass)
        : base(sprite, position, radius * 2, height, mass)
    {
        float x1 = position.X;
        float y1 = position.Y;
        float x2 = x1;
        float y2 = y1 + height;

        Circle circle1 = new Circle(x1, y1, radius);
        Circle circle2 = new Circle(x2, y2, radius);
        Vec2 recVec1 = circle2.Position + (circle2.Position - circle1.Position).Unit().Normal() * radius;
        Vec2 recVec2 = circle1.Position + (circle2.Position - circle1.Position).Unit().Normal() * radius;

        Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * radius);
        rectangle1.CalculateVertices();

        Components = new Shape[] { rectangle1, circle1, circle2 };

        Inertia = Mass * (
            (float)Math.Pow(2 * rectangle1.Width, 2) +
            (float)Math.Pow(height + 2 * rectangle1.Width, 2)
            ) / 12;
    }

    /// <summary>
    /// Move according to WASD input.
    /// </summary>
    public void SimpleInput()
    {
        bool up = InputHandler.KeyboardState.IsKeyDown(Keys.W);
        bool down = InputHandler.KeyboardState.IsKeyDown(Keys.S);
        bool left = InputHandler.KeyboardState.IsKeyDown(Keys.A);
        bool right = InputHandler.KeyboardState.IsKeyDown(Keys.D);

        Vec2 dir = Components[0].Dir;
        if (up) Acceleration = dir * -Speed;
        if (down) Acceleration = dir * Speed;
        if (!up && !down) Acceleration = new Vec2(0, 0);

        if (left) AngleSpeed -= 0.003f;
        if (right) AngleSpeed += 0.003f;
    }

    public override void Move()
    {
        base.Move();

        AngleSpeed *= 1 - AngleFriction;
        ((Rectangle)Components[0]).Angle += AngleSpeed;
        ((Rectangle)Components[0]).CalculateVertices();

        float length = ((Rectangle)Components[0]).Length;
        Components[1].Position = Components[0].Position + Components[0].Dir * -length / 2;
        Components[2].Position = Components[0].Position + Components[0].Dir * length / 2;
    }
}

