using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Verdant;

namespace TopdownShooter
{
    internal static class Resources
    {

        public static Sprite Player { get; private set; }
        public static Sprite Enemy { get; private set; }
        public static Sprite Wall { get; private set; }
        public static Sprite Ground { get; private set; }
        public static Sprite Bullet { get; private set; }

        public static SpriteFont DebugFont { get; private set; }

        public static void LoadResources(ContentManager content)
        {
            Player = content.Load<Texture2D>("player");
            Enemy = content.Load<Texture2D>("enemy");
            Wall = content.Load<Texture2D>("wall");
            Ground = content.Load<Texture2D>("ground");
            Bullet = content.Load<Texture2D>("bullet");

            DebugFont = content.Load<SpriteFont>("debugfont");
        }

    }
}
