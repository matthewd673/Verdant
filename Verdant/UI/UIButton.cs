using System;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIButton : UISprite
    {

        // Denotes if the UIButton is hovered.
        public bool Hovered { get; private set; } = false;

        /// <summary>
        /// Initialize a new UIButton.
        /// </summary>
        /// <param name="sprite">The sprite to render (also used to determine the button's width and height).</param>
        /// <param name="position">The position of the button.</param>
        public UIButton(RenderObject sprite, Vec2 position) : base(sprite, position) { }
        /// <summary>
        /// Initialize a new UIButton.
        /// </summary>
        /// <param name="sprite">The sprite to render.</param>
        /// <param name="position">The position of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public UIButton(RenderObject sprite, Vec2 position, int width, int height)
            : base(sprite, position, width, height) { }

        public override void Update()
        {
            base.Update();

            // check for hover
            if (GameMath.CheckPointOnRectIntersection(
                (Vec2)InputHandler.MousePosition,
                Position.X * Renderer.Scale,
                Position.Y * Renderer.Scale,
                (int)Width * Renderer.Scale,
                (int)Height * Renderer.Scale
                ))
            { // button is being hovered
                if (!Hovered) //it wasn't hovered last time, so trigger
                    OnHover();
                Hovered = true;
            }
            else // button is no longer being hovered
            {
                if (Hovered) //it was hovered before, so trigger exit
                    OnHoverExit();
                Hovered = false;
            }

            // check for click
            if (Hovered && InputHandler.IsMouseFirstReleased())
                OnClick();
        }

        public event EventHandler Hover;
        protected virtual void OnHover()
        {
            Hover?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HoverExit;
        protected virtual void OnHoverExit()
        {
            HoverExit?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Click;
        protected virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

    }
}
