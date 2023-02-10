using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// An object, attached to an Entity, that determines what texture is used when rendering.
    /// </summary>
    public abstract class RenderObject
    {
        // A null RenderObject.
        public const RenderObject None = null;

        // The draw width of the RenderObject.
        public virtual int Width { get; protected set; }
        // The draw height of the RenderObject.
        public virtual int Height { get; protected set; }

        /// <summary>
        /// Draw the RenderObject.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen bounds to draw to.</param>
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle bounds);

    }
}
