using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{

    /// <summary>
    /// Sprite is a wrapper class for Texture2D which can be used anywhere a RenderObject is required. Texture2Ds can be implicitly converted to Sprites.
    /// </summary>
    public class Sprite : RenderObject
    {

        Texture2D sprite;

        /// <summary>
        /// Initialize a new Sprite.
        /// </summary>
        /// <param name="sprite">The Sprite's texture.</param>
        public Sprite(Texture2D sprite) { this.sprite = sprite; }

        public override Texture2D Draw() { return sprite; }

        public override Texture2D DrawIndex(int i, int j = 0) { return Draw(); }

        public static implicit operator Sprite(Texture2D texture2d) => new Sprite(texture2d);

    }
}
