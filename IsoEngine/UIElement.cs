using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class UIElement
    {

        UIManager manager;

        Vec2 pos;

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

    }
}
