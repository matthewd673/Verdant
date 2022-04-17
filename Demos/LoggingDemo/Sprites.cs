using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Verdant;

namespace LoggingDemo
{
    public static class Sprites
    {

        public static Sprite player;
        public static SpriteFont debugFont;

        public static bool Loaded { get; private set; }

        public static void LoadSprites(ContentManager content)
        {
            player = content.Load<Texture2D>("player");

            debugFont = content.Load<SpriteFont>("debugfont");

            Loaded = true;
        }

    }
}
