using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{

    /// <summary>
    /// Sprite is a wrapper class for Texture2D which can be used anywhere an IRenderObject is required. Texture2Ds can be implicitly converted to Sprites.
    /// </summary>
    public class Sprite : RenderObject
    {

        Texture2D sprite;

        public Sprite(Texture2D sprite) { this.sprite = sprite; }

        public override Texture2D Get() { return sprite; }

        public override Texture2D GetIndex(int i, int j = 0) { return Get(); }

        public static implicit operator Sprite(Texture2D texture2d) => new Sprite(texture2d);

    }
}
