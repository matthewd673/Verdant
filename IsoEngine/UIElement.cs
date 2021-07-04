using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class UIElement
    {

        UIManager manager;

        public Vec2 pos;

        bool forRemoval;

        public UIElement(Vec2 pos)
        {
            this.pos = pos;
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
