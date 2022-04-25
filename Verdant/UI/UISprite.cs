using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UISprite : UIElement
    {

        public int Width { get; set; }
        public int Height { get; set; }

        public RenderObject Sprite { get; set; }
        public int SpriteIndex { get; set; } = 0;

        public UISprite(RenderObject sprite, Vec2 pos) : base(pos)
        {
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }
        public UISprite(RenderObject sprite, Vec2 pos, int w, int h) : base(pos)
        {
            Sprite = sprite;
            Width = w;
            Height = h;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.DrawIndex(SpriteIndex),
                new Rectangle(
                    (int)(Position.X * Renderer.Scale),
                    (int)(Position.Y * Renderer.Scale),
                    Width * Renderer.Scale,
                    Height * Renderer.Scale),
                Color.White);
        }

    }
}
