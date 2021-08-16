using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.UI
{
    public class UIButton : UISprite
    {

        bool hovered = false;

        public UIButton(Sprite sprite, Vec2 pos) : base(sprite, pos) { }
        public UIButton(Sprite sprite, Vec2 pos, int w, int h) : base(sprite, pos, w, h) { }
        public UIButton(SpriteSheet sheet, Vec2 pos) : base(sheet, pos) { }
        public UIButton(SpriteSheet sheet, Vec2 pos, int w, int h) : base(sheet, pos, w, h) { }

        public override void Update()
        {
            base.Update();

            //check for hover
            if (Math.CheckPointOnRectIntersection(InputHandler.mouseVec, Position.X * Renderer.Scale, Position.Y * Renderer.Scale, Width * Renderer.Scale, Height * Renderer.Scale))
            { //button is being hovered
                if (!hovered) //it wasn't hovered last time, so trigger
                    OnHover();
                hovered = true;
            }
            else //button is no longer being hovered
            {
                if (hovered) //it was hovered before, so trigger exit
                    OnHoverExit();
                hovered = false;
            }

            //check for click
            if (hovered && InputHandler.IsMouseFirstReleased())
                OnClick();

        }

        public event EventHandler Hover;
        public virtual void OnHover()
        {
            EventHandler hoverEvent = Hover; //recommended to avoid null
            if (hoverEvent != null)
                hoverEvent(this, EventArgs.Empty);
        }

        public event EventHandler HoverExit;
        public virtual void OnHoverExit()
        {
            EventHandler hoverExitEvent = HoverExit; //recommended to avoid null
            if (hoverExitEvent != null)
                hoverExitEvent(this, EventArgs.Empty);
        }

        public event EventHandler Click;
        public virtual void OnClick()
        {
            EventHandler clickEvent = Click; //recommended to avoid null
            if (clickEvent != null)
                clickEvent(this, EventArgs.Empty);
        }

    }
}
