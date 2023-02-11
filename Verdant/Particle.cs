using Verdant.Physics;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace Verdant
{
    public class Particle
    {

        int lifetime = int.MaxValue;

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

        public bool Dead { get { return lifetime <= 0; } }

        /// <summary>
        /// Initialize a new Particle.
        /// </summary>
        /// <param name="sprite">The Particle's sprite.</param>
        /// <param name="position">The position of the Particle.</param>
        /// <param name="width">The width of the Particle.</param>
        /// <param name="height">The height of the Particle.</param>
        /// <param name="lifetime">The number of frames the Particle should live before being marked as dead.</param>
        public Particle(RenderObject sprite, int width = -1, int height = -1)
        {
            Sprite = sprite;
            Width = (width == -1 && sprite != RenderObject.None) ? sprite.Width : width;
            Height = (height == -1 && sprite != RenderObject.None) ? sprite.Height : height;
        }

        /// <summary>
        /// Update the Particle.
        /// </summary>
        public void Update()
        {
            lifetime--;
            if (lifetime <= 0)
                return;
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

            Sprite.Draw(spriteBatch,
                System.Manager.Scene.Camera.GetRenderBounds(
                    Position.X - HalfWidth,
                    Position.Y - HalfHeight,
                    Width,
                    Height)
                );
        }

    }
}
