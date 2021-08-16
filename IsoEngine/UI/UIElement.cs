using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.UI
{
    public class UIElement
    {

        public UIManager Manager { get; set; }

        public Vec2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        bool forRemoval;

        public UIElement(Vec2 pos, int w, int h)
        {
            Position = pos;
            Width = w;
            Height = h;
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void MarkForRemoval()
        {
            forRemoval = true;
        }

        public bool IsForRemoval()
        {
            return forRemoval;
        }

    }
}
