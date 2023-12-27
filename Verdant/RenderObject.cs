using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// An object that determines what Texture2D should be rendered.
    /// </summary>
    public abstract class RenderObject
    {
        // A null RenderObject.
        public const RenderObject None = null;

        protected Texture2D texture;
        // Get the uncropped Texture2D associated with this RenderObject.
        public virtual Texture2D Texture { get { return texture; } }

        protected int width;
        protected int height;
        // The draw width of the RenderObject.
        public virtual int Width { get { return width; } }
        // The draw height of the RenderObject.
        public virtual int Height { get { return height; } }

        // TODO: remove or update old Draw APIs
        public abstract void Draw(SpriteBatch spriteBatch, TransformState transform);

        /// <summary>
        /// Draw the RenderObject.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle bounds);

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin);

        public abstract void DrawIndex(SpriteBatch spriteBatch, TransformState transform, int x, int y = 0);

        /// <summary>
        /// Draw a cropped portion of the RenderObject.
        /// Some RenderObjects do not index their texture and only support Draw().
        /// Some RenderObjects only use X coordinates to index their texture.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        /// <param name="x">The X coordinate of the cropped sprite.</param>
        /// <param name="y">The Y coordinate of the cropped sprite.</param>
        public abstract void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, int x, int y = 0);

        public abstract void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin, int x, int y = 0);
    }
}
