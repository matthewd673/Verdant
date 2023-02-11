using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UILabel : UIElement
    {

        private string _text;
        // The string displayed by the UILabel.
        public string Text
        {
            get { return _text; }
            set
            {
                if (value.Equals(_text)) return;

                _text = value;
                Width = Font.MeasureString(_text).X;
                Height = Font.MeasureString(_text).Y;
            }
        }

        // The font used to draw the UILabel.
        public SpriteFont Font { get; private set; }
        // The color of the text.
        public Color Color { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.Transparent;

        /// <summary>
        /// Initialize a new UILabel.
        /// </summary>
        /// <param name="position">The position of the text.</param>
        /// <param name="font">The SpriteFont to draw the text with.</param>
        /// <param name="text">The string to display.</param>
        public UILabel(Vec2 position, SpriteFont font, string text = "")
            : base(position, font.MeasureString(text).X, font.MeasureString(text).Y)
        {
            Font = font;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Doesn't this need to be scaled?
            spriteBatch.Draw(Renderer.Pixel, new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Width, (int)Height), BackgroundColor);
            spriteBatch.DrawString(Font, Text, (Vector2)AbsolutePosition * Renderer.Scale, Color);
        }

        public override void DrawBounds(SpriteBatch spriteBatch)
        {
            // UILabel width/height don't scale
            Renderer.DrawRectangle(spriteBatch,
                                   AbsolutePosition * Renderer.Scale,
                                   (AbsolutePosition * Renderer.Scale) + new Vec2(Width, Height),
                                   Color.Pink
                                   );
        }

    }
}
