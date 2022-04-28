using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UISprite : UIElement
    {

        // The width of the sprite.
        public int Width { get; set; }
        // The height of the sprite.
        public int Height { get; set; }

        // The RenderObject to draw.
        public RenderObject Sprite { get; set; }
        // The index of the RenderObject to draw (only use if it is an Animation or SpriteSheet).
        public int SpriteIndex { get; set; } = 0;

        /// <summary>
        /// Initialize a new UISprite.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="pos">The position of the sprite on the UI.</param>
        public UISprite(RenderObject sprite, Vec2 pos) : base(pos)
        {
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }
        /// <summary>
        /// Initialize a new UISprite.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        /// <param name="pos">The position of the sprite on the UI.</param>
        /// <param name="w">The width of the sprite.</param>
        /// <param name="h">The height of the sprite.</param>
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
