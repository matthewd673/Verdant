using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A UIElement that accepts mouse click input.
    /// </summary>
    public class UIButton : UISprite
    {
        /// <summary>
        /// Initialize a new UIButton.
        /// </summary>
        /// <param name="sprite">The sprite to render (also used to determine the button's width and height).</param>
        /// <param name="position">The position of the button.</param>
        public UIButton(RenderObject sprite, Vec2 position)
          : base(sprite, position)
        {
            // Empty
        }

        /// <summary>
        /// Initialize a new UIButton.
        /// </summary>
        /// <param name="sprite">The sprite to render.</param>
        /// <param name="position">The position of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public UIButton(RenderObject sprite, Vec2 position, int width, int height)
            : base(sprite, position, width, height)
        {
            // Empty
        }
    }
}
