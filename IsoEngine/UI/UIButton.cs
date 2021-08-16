using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.UI
{
    public class UIButton : UISprite
    {

        bool hovered = false;

        Action<UIButton> hoverAction;
        Action<UIButton> hoverExitAction;
        Action<UIButton> clickAction;

        public UIButton(Texture2D sprite, Vec2 pos) : base(sprite, pos) { }
        public UIButton(Texture2D sprite, Vec2 pos, int w, int h) : base(sprite, pos, w, h) { }
        public UIButton(SpriteSheet sheet, Vec2 pos) : base(sheet, pos) { }
        public UIButton(SpriteSheet sheet, Vec2 pos, int w, int h) : base(sheet, pos, w, h) { }

        public override void Update()
        {
            base.Update();

            //check for hover
            if (Math.CheckPointOnRectIntersection(InputHandler.mouseVec, pos.X * Renderer.Scale, pos.Y * Renderer.Scale, w * Renderer.Scale, h * Renderer.Scale))
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

        void OnHover()
        {
            if (hoverAction != null)
                hoverAction.Invoke(this);
        }
        void OnHoverExit()
        {
            if (hoverExitAction != null)
                hoverExitAction.Invoke(this);
        }
        void OnClick()
        {
            if (clickAction != null)
                clickAction.Invoke(this);
        }

        public void SetHoverAction(Action<UIButton> hoverAction)
        {
            this.hoverAction = hoverAction;
        }
        public void SetHoverExitAction(Action<UIButton> hoverExitAction)
        {
            this.hoverExitAction = hoverExitAction;
        }
        public void SetClickAction(Action<UIButton> clickAction)
        {
            this.clickAction = clickAction;
        }

    }
}
