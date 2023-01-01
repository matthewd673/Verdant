using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{
    public class Shape
    {

        internal Vec2[] Vertices { get; set; }
        internal Vec2 Position { get; set; }
        public Vec2 Dir { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera, Color color) { }

    }
}
