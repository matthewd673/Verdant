using Verdant;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleToy
{
    internal class Resources
    {

        public static Sprite SliderIndicator { get; private set; }
        public static Sprite SliderBar { get; private set; }

        public static Sprite Particle { get; private set; }

        public static SpriteFont Font { get; private set; }

        public static void LoadSprites(ContentManager content)
        {
            SliderIndicator = content.Load<Texture2D>("slider-indicator");
            SliderBar = content.Load<Texture2D>("slider-bar");

            Particle = content.Load<Texture2D>("particle");

            Font = content.Load<SpriteFont>("debug");
        }

    }
}
