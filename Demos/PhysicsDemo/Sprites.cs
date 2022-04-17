using System;
using Verdant;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PhysicsDemo
{
    public static class Sprites
    {

        public static Sprite Crate;
        public static Sprite BoxEntity;
        public static Sprite CapsuleEntity;

        public static void LoadSprites(ContentManager content)
        {
            Crate = content.Load<Texture2D>("crate");
            BoxEntity = content.Load<Texture2D>("boxentity");
            CapsuleEntity = content.Load<Texture2D>("capsuleentity");
        }

    }
}
