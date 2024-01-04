using Verdant.Physics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant;

/// <summary>
/// A Particle to be simulated within a ParticleSystem. Essentially a simplified Entity.
/// </summary>
public class Particle
{
    public Timer LifeTimer { get; private set; }

    public ParticleSystem System { get; set; }

    public RenderObject Sprite { get; set; }
    public virtual Vec2 Position { get; set; }

    private float _width;
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
    public float Height
    {
        get { return _height; }
        set
        {
            _height = value;
            HalfHeight = value / 2;
        }
    }

    protected float HalfWidth { get; private set; }
    protected float HalfHeight { get; private set; }

    public Vec2 Velocity { get; set; } = new Vec2(0, 0);
    public Vec2 Acceleration { get; set; } = new Vec2(0, 0);
    public float Friction { get; set; } = 0;

    public float Angle { get; set; } = 0;

    public TransformAnimation TransformAnimation { get; set; }

    public bool Dead { get; private set; }

    /// <summary>
    /// Initialize a new Particle.
    /// </summary>
    /// <param name="sprite">The Particle's sprite.</param>
    /// <param name="width">The width of the Particle.</param>
    /// <param name="height">The height of the Particle.</param>
    public Particle(RenderObject sprite, int width = -1, int height = -1)
    {
        if (sprite != RenderObject.None)
        {
            if (sprite.GetType() == typeof(Animation) ||
                sprite.GetType().IsSubclassOf(typeof(Animation)))
            {
                Sprite = ((Animation)sprite).Copy();
            }
            else
            {
                Sprite = sprite;
            }
        }

        LifeTimer = new Timer(1, (Timer timer) => { Dead = true; });
        Width = (width == -1 && sprite != RenderObject.None) ? sprite.Width : width;
        Height = (height == -1 && sprite != RenderObject.None) ? sprite.Height : height;
    }

    /// <summary>
    /// Update the Particle.
    /// </summary>
    public void Update()
    {
        Velocity += Acceleration;
        Velocity *= 1 - Friction;

        Position += Velocity;
    }

    /// <summary>
    /// Draw the Particle.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        if (Sprite == RenderObject.None)
        {
            return;
        }

        Transform animState = TransformAnimation.GetFrame();
        Sprite.Draw(spriteBatch, animState);
    }
}

