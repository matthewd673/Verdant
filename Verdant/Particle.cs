using Verdant.Physics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    public class Particle
    {

        public Timer LifeTimer { get; private set; }

        public ParticleSystem System { get; set; }

        public RenderObject Sprite { get; set; }
        public virtual Vec2 Position { get; set; }

        private int _width;
        public int Width
        { 
            get { return _width; }
            set 
            {
                _width = value;
                HalfWidth = value / 2;
            }
        }
        private int _height;
        public int Height
        { 
            get { return _height; }
            set
            {
                _height = value;
                HalfHeight = value / 2;
            }
        }

        protected int HalfWidth { get; private set; }
        protected int HalfHeight { get; private set; }

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
            Sprite = sprite;
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

            if (TransformAnimation == null)
            {
                Sprite.Draw(spriteBatch,
                    System.Manager.Scene.Camera.GetRenderBounds(
                        Position.X - HalfWidth,
                        Position.Y - HalfHeight,
                        Width,
                        Height)
                    );
            }
            else
            {
                TransformState animState = TransformAnimation.GetFrame();
                if (animState.Multiply)
                {
                    Sprite.Draw(spriteBatch,
                        System.Manager.Scene.Camera.GetRenderBounds(
                            (Position.X - (Width * animState.Width / 2f)) * animState.Position.X,
                            (Position.Y - (Height * animState.Height / 2f)) * animState.Position.Y,
                            (int)(Width * animState.Width),
                            (int)(Height * animState.Height)
                            ),
                        Angle * animState.Angle,
                        new Vector2(
                            Sprite.Width * animState.Width / 2,
                            Sprite.Height * animState.Height / 2
                            ));
                }
                else
                {
                    Sprite.Draw(spriteBatch,
                        System.Manager.Scene.Camera.GetRenderBounds(
                            (animState.Width / 2f) + animState.Position.X,
                            (animState.Height / 2f) + animState.Position.Y,
                            (int)animState.Width,
                            (int)animState.Height
                            ),
                        animState.Angle,
                        new Vector2(
                            animState.Width / 2,
                            animState.Height / 2
                            ));
                }
            }
        }

    }
}
