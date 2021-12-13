using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IsoEngine;

namespace LoggingDemo
{
    public static class Sprites
    {

        public static Sprite player;

        public static void LoadSprites(ContentManager content)
        {
            player = content.Load<Texture2D>("player");
        }

    }
}
