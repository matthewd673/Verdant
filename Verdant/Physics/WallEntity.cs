using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics;

/// <summary>
/// An Entity with a Wall body. Doesn't render or move by default.
/// </summary>
public class WallEntity : PhysicsEntity
{
    /// <summary>
    /// Initialize a new WallEntity.
    /// </summary>
    /// <param name="start">The start position of the Wall.</param>
    /// <param name="end">The end position of the Wall.</param>
    public WallEntity(Vec2 start, Vec2 end)
        : base(new Vec2(), RenderObject.None, 0, 0, mass: 0)
    {
        Components = new Shape[] { new Line(start.X, start.Y, end.X, end.Y) };
        Position = new Vec2(
            (start.X + end.X) / 2f,
            (start.Y + end.Y) / 2f
            );
    }
}

