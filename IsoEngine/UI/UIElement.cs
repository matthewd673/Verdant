using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.UI
{
    public class UIElement
    {

        UIManager manager;

        public Vec2 Position;
        public int Width;
        public int Height;

        bool forRemoval;

        public UIElement(Vec2 pos, int w, int h)
        {
            Position = pos;
            Width = w;
            Height = h;
        }

        public void SetManager(UIManager manager)
        {
            this.manager = manager;
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
