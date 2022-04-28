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
        /// <param name="pos">The position of the button.</param>
        public UIButton(RenderObject sprite, Vec2 pos) : base(sprite, pos) { }
        /// <summary>
        /// Initialize a new UIButton.
        /// </summary>
        /// <param name="sprite">The sprite to render.</param>
        /// <param name="pos">The position of the button.</param>
        /// <param name="w">The width of the button.</param>
        /// <param name="h">The height of the button.</param>
        public UIButton(RenderObject sprite, Vec2 pos, int w, int h) : base(sprite, pos, w, h) { }

        public override void Update()
        {
            base.Update();

            // check for hover
            if (GameMath.CheckPointOnRectIntersection(
                (Vec2)InputHandler.MousePosition,
                Position.X * Renderer.Scale,
                Position.Y * Renderer.Scale,
                Width * Renderer.Scale,
                Height * Renderer.Scale
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
