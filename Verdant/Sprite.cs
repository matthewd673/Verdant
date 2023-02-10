using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{

    /// <summary>
    /// Sprite is a wrapper class for Texture2D which can be used anywhere a RenderObject is required. Texture2Ds can be implicitly converted to Sprites.
    /// </summary>
    public class Sprite : RenderObject
    {
        protected Rectangle crop;

        /// <summary>
        /// Initialize a new Sprite.
        /// </summary>
        /// <param name="sprite">The Sprite's texture.</param>
        public Sprite(Texture2D sprite)
        {
            texture = sprite;
            width = texture.Width;
            height = texture.Height;
        }

        /// <summary>
        /// Initialize a new Sprite.
        /// </summary>
        /// <param name="sprite">The Sprite's texture.</param>
        /// <param name="crop">The section of the texture to draw.</param>
        public Sprite(Texture2D sprite, Rectangle crop)
        {
            texture = sprite;
            this.crop = crop;
            width = crop.Width;
            height = crop.Height;
        }

        /// <summary>
        /// Draw the Sprite's texture at the given screen bounds.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.Draw(texture, bounds, crop, Color.White);
        }

        /// <summary>
        /// Sprites do not support DrawIndex, it will be a normal Draw call.
        /// </summary>
        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, int x, int y = 0)
        {
            Draw(spriteBatch, bounds);
        }

        public static implicit operator Sprite(Texture2D texture2d) => new Sprite(texture2d);

    }
}
