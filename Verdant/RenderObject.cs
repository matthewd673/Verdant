using System;
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
        public virtual int Width { get { return DrawIndex(0).Width; } }
        // The draw height of the RenderObject.
        public virtual int Height { get { return DrawIndex(0).Height; } }

        /// <summary>
        /// Draw the RenderObject.
        /// </summary>
        /// <returns>A Texture2D representing the current state of the RenderObject.</returns>
        public abstract Texture2D Draw();
        /// <summary>
        /// Draw the frame at a specific index within the RenderObject.
        /// </summary>
        /// <param name="i">The X coordinate.</param>
        /// <param name="j">The Y coordinate. Some RenderObjects with multiple frames may not index them with Y coordinates.</param>
        /// <returns></returns>
        public abstract Texture2D DrawIndex(int i, int j = 0);

    }
}
