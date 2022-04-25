using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIElement
    {

        public UIManager Manager { get; set; }

        public Vec2 Position { get; set; }

        bool forRemoval;

        public UIElement(Vec2 pos)
        {
            Position = pos;
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
