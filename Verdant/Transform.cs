using System;
using Microsoft.Xna.Framework;

namespace Verdant;

/// <summary>
/// Modifies the way an Entity, Particle, etc. is rendered to the screen.
/// Effect can be either multiplicative (a.k.a "relative", Multiply=true) or absolute (Multiply=false).
/// </summary>
public class Transform
{
    // Determines the way the TransformState's effects are applied.
    public TransformBlendMode BlendMode { get; set; }

    // The position of the TransformState.
    public Vec2 Position { get; set; }
    private float _width;
    // The width of the TransformState.
    public float Width
    {
        get { return _width; }
        set
        {
            _width = value;
            HalfWidth = value / 2;
        }
    }
    private float _height;
    // The height of the TransformState.
    public float Height
    {
        get { return _height; }
        set
        {
            _height = value;
            HalfHeight = value / 2;
        }
    }
    // The rotation angle of the TransformState.
    public float Angle { get; set; }

    public float HalfWidth { get; private set; }
    public float HalfHeight { get; private set; }

    /// <summary>
    /// Initialize a new TransformState.
    /// By default, its properties will be initialized to not have any effect.
    /// </summary>
    /// <param name="blendMode">Determines the TransformState's blend mode.</param>
    public Transform(TransformBlendMode blendMode)
    {
        BlendMode = blendMode;

        // defaults to 1 if multiplicative
        if (BlendMode == TransformBlendMode.Multiply)
        {
            Position = new Vec2(1, 1);
            Width = 1;
            Height = 1;
            Angle = 1;
        }
        else
        {
            Position = new Vec2(0, 0);
        }
    }

    /// <summary>
    /// Initialize a new TransformState.
    /// </summary>
    /// <param name="position">The position of the TransformState.</param>
    /// <param name="width">The width of the TransformState.</param>
    /// <param name="height">The height of the TransformState.</param>
    /// <param name="angle">The rotation angle of the TransformState.</param>
    /// <param name="blendMode">Determiens the TransformState's blend mode.</param>
    public Transform(Vec2 position, float width, float height, float angle, TransformBlendMode blendMode)
    {
        Position = position;
        Width = width;
        Height = height;
        Angle = angle;
        BlendMode = blendMode;
    }

    /// <summary>
    /// Create a new TransformState with the same properties as this one.
    /// </summary>
    /// <returns>A new TransformState.</returns>
    public Transform Copy()
    {
        Transform newState = new(BlendMode)
        {
            Position = Position.Copy(),
            Width = Width,
            Height = Height,
            Angle = Angle,
        };
        return newState;
    }

    /// <summary>
    /// Blend another TransformState onto this one in-place.
    /// </summary>
    /// <param name="operand">The TransformState to blend with. The two will be blended according to the operand's BlendMode and the operand will not be modified.</param>
    public void Blend(Transform operand)
    {
        switch (operand.BlendMode)
        {
            case TransformBlendMode.Add:
                Position.X += operand.Position.X;
                Position.Y += operand.Position.Y;
                Width += operand.Width;
                Height += operand.Height;
                Angle += operand.Angle;
                break;
            case TransformBlendMode.Multiply:
                Position.X *= operand.Position.X;
                Position.Y *= operand.Position.Y;
                Width *= operand.Width;
                Height *= operand.Height;
                Angle *= operand.Angle;
                break;
            case TransformBlendMode.Override:
                Position.X = operand.Position.X;
                Position.Y = operand.Position.Y;
                Width = operand.Width;
                Height = operand.Height;
                Angle = operand.Angle;
                break;
        }
    }

    public Rectangle GetRenderRectangle()
    {
        return new((int)(Position.X),
                   (int)(Position.Y),
                   (int)Width,
                   (int)Height
                   );
    }

    public static Transform operator +(Transform a, Transform b)
    {
        return new(a.Position + b.Position,
                   a.Width + b.Width,
                   a.Height + b.Height,
                   a.Angle + b.Angle,
                   a.BlendMode
                   );
    }

    public static Transform operator -(Transform a, Transform b)
    {
        return new(a.Position - b.Position,
                   a.Width - b.Width,
                   a.Height - b.Height,
                   a.Angle - b.Angle,
                   a.BlendMode
                   );
    }
    public static Transform operator *(Transform a, Transform b)
    {
        return new(a.Position * b.Position,
                   a.Width * b.Width,
                   a.Height * b.Height,
                   a.Angle * b.Angle,
                   a.BlendMode
                   );
    }
    public static Transform operator /(Transform a, Transform b)
    {
        return new(a.Position / b.Position,
                   a.Width / b.Width,
                   a.Height / b.Height,
                   a.Angle / b.Angle,
                   a.BlendMode
                   );
    }
}

