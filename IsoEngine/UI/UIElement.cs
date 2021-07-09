using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.UI
{
    public class UIElement
    {

        UIManager manager;

        public Vec2 pos;
        public int w;
        public int h;

        bool forRemoval;

        public UIElement(Vec2 pos, int w, int h)
        {
            this.pos = pos;
            this.w = w;
            this.h = h;
        }

        public void SetManager(UIManager manager)
        {
            this.manager = manager;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

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
