using Verdant;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleToy
{
    internal class Sprites
    {

        public static Sprite SliderIndicator;
        public static Sprite SliderBar;

        public static void LoadSprites(ContentManager content)
        {
            SliderIndicator = content.Load<Texture2D>("slider-indicator");
            SliderBar = content.Load<Texture2D>("slider-bar");
        }

    }
}
