using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public interface IRenderObject
    {

        Texture2D Get();
        Texture2D GetIndex(int i, int j = 0);

    }
}
