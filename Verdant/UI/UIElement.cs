using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIElement
    {

        // The UIManager managing the element.
        public UIManager Manager { get; set; }

        // The position of the UIElement in screen space.
        public Vec2 Position { get; set; }

        // Determines if the UIElement will be removed at the end of the update loop.
        public bool ForRemoval { get; set; }

        /// <summary>
        /// Initialize a new UIElement.
        /// </summary>
        /// <param name="pos">The position of the UIElement.</param>
        public UIElement(Vec2 pos)
        {
            Position = pos;
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

    }
}
