using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIText : UIElement
    {

        private string _text;
        // The string displayed by the UIText.
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Width = Font.MeasureString(_text).X;
            }
        }

        // The font used to draw the UIText.
        public SpriteFont Font { get; private set; }
        // The color of the text.
        public Color Color { get; set; } = Color.Black;

        // The width of the current string rendered with the current SpriteFont.
        public float Width { get; private set; }
        // The height (line spacing) of the current SpriteFont. Changing the string has no effect.
        public float Height { get { return Font.LineSpacing; } }

        /// <summary>
        /// Initialize a new UIText.
        /// </summary>
        /// <param name="pos">The position of the text.</param>
        /// <param name="font">The SpriteFont to draw the text with.</param>
        /// <param name="text">The string to display.</param>
        public UIText(Vec2 pos, SpriteFont font, string text = "") : base(pos)
        {
            Font = font;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, (Vector2)Position, Color);
        }

    }
}
