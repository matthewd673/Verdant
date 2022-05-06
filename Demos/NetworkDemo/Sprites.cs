using System;
using Verdant;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NetworkDemo
{
    internal class Sprites
    {

        public static Sprite Player { get; private set; }
        public static Sprite HostButton { get; private set; }
        public static Sprite JoinButton { get; private set; }

        public static SpriteFont DebugFont { get; private set; }

        public static void LoadSprites(ContentManager content)
        {
            Player = content.Load<Texture2D>("player");
            HostButton = content.Load<Texture2D>("host-button");
            JoinButton = content.Load<Texture2D>("join-button");

            DebugFont = content.Load<SpriteFont>("debug-font");
        }

    }
}
