using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UISprite : UIElement
    {

        public int Width { get; set; }
        public int Height { get; set; }

        Texture2D[,] sprites;
        int spriteX = 0;
        int spriteY = 0;

        public UISprite(Sprite sprite, Vec2 pos) : base(pos)
        {
            sprites = new Texture2D[,] { { sprite.Get() } };
            Width = sprite.Width;
            Height = sprite.Height;
        }
        public UISprite(Sprite sprite, Vec2 pos, int w, int h) : base(pos)
        {
            sprites = new Texture2D[,] { { sprite.Get() } };
            Width = w;
            Height = h;
        }
        public UISprite(SpriteSheet sheet, Vec2 pos) : base(pos)
        {
            sprites = sheet.GetFullSheet();
            Width = sheet.Get().Width;
            Height = sheet.Get().Height;
        }
        public UISprite(SpriteSheet sheet, Vec2 pos, int w, int h) : base(pos)
        {
            sprites = sheet.GetFullSheet();
            Width = w;
            Height = h;
        }

        public void SetSpriteX(int x)
        {
            spriteX = x;
            spriteY = 0;
        }

        public void SetSpriteCoords(int x, int y)
        {
            spriteX = x;
            spriteY = y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[spriteX, spriteY],
                new Rectangle(
                    (int)(Position.X * Renderer.Scale),
                    (int)(Position.Y * Renderer.Scale),
                    Width * Renderer.Scale,
                    Height * Renderer.Scale),
                Color.White);
        }

    }
}
