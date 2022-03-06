using System;
using IsoEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PhysicsDemo
{
    public static class Sprites
    {

        public static Sprite Crate;

        public static void LoadSprites(ContentManager content)
        {
            Crate = content.Load<Texture2D>("crate");
        }

    }
}
