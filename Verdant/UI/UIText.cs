﻿using System;
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
        public Color BackgroundColor { get; set; } = Color.Transparent;

        // The width of the current string rendered with the current SpriteFont.
        public new float Width { get; private set; }
        // The height (line spacing) of the current SpriteFont. Changing the string has no effect.
        public new float Height { get { return Font.LineSpacing; } }

        /// <summary>
        /// Initialize a new UIText.
        /// </summary>
        /// <param name="position">The position of the text.</param>
        /// <param name="font">The SpriteFont to draw the text with.</param>
        /// <param name="text">The string to display.</param>
        public UIText(Vec2 position, SpriteFont font, string text = "")
            : base(position, font.MeasureString(text).X, font.LineSpacing)
        {
            Font = font;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Renderer.GetPixel(), new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height), BackgroundColor);
            spriteBatch.DrawString(Font, Text, (Vector2)Position, Color);
        }

    }
}
