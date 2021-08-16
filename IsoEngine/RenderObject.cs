using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public abstract class RenderObject
    {

        public int Width { get { return Get().Width; } }
        public int Height { get { return Get().Height; } }

        public abstract Texture2D Get();
        public abstract Texture2D GetIndex(int i, int j = 0);

    }
}
