using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    public abstract class RenderObject
    {
        public const RenderObject None = null;

        public virtual int Width { get { return DrawIndex(0).Width; } }
        public virtual int Height { get { return DrawIndex(0).Height; } }

        public abstract Texture2D Draw();
        public abstract Texture2D DrawIndex(int i, int j = 0);

    }
}
