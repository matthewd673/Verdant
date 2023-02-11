using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UITextBox : UIElement
    {

        // Denotes if the UITextBox is hovered.
        public bool Hovered { get; private set; }
        // Denotes if the UITextBox is focused. If the UITextBox is focused, it is accepting input.
        public bool Focused { get; private set; }

        private string _text;
        // The string contents of the UITextBox.
        public string Text
        {
            get { return _text; }
            set
            {
                if (MaxLength >= 0 && value.Length > MaxLength)
                    value = value[..MaxLength];
                _text = value;
                Width = Font.MeasureString(_text).X;
            }
        }
        // The maximum number of characters allowed in the text string. Set to 0 or less for infinite length.
        public int MaxLength { get; set; }

        // The font used to draw the UITextBox.
        public SpriteFont Font { get; private set; }
        // The color of the text.
        public Color Color { get; set; } = Color.Black;
        // The color of the background box.
        public Color BackgroundColor { get; set; } = Color.White;
        // The color of the focus outline (if DrawFocusOutline is enabled).
        public Color OutlineColor { get; set; } = Color.Black;

        // The amount of padding on each side of the box.
        public float Padding { get; set; } = 0f;
        // Determines if an outline should be drawn when the UITextBox is focused.
        public bool DrawFocusOutline { get; set; } = true;

        private float _width;
        // The width of the UITextBox (may change when the text changes).
        public new float Width
        {
            get { return (_width > MinWidth) ? _width : MinWidth; }
            private set { _width = value; }
        }
        // The minimum (pixel) width of the UITextBox.
        public float MinWidth { get; set; } = 64;
        // The height of the UITextBox (does not change when the text changes).
        public new float Height { get { return Font.LineSpacing; } }

        /// <summary>
        /// Initialize a new UITextBox.
        /// </summary>
        /// <param name="position">The position of the textbox.</param>
        /// <param name="font">The SpriteFont to draw the text with.</param>
        /// <param name="text">The string to display by default.</param>
        public UITextBox(Vec2 position, SpriteFont font, string text = "")
            : base(position, 0, font.LineSpacing) // TODO: 0 width?
        {
            Font = font;
            Text = text;
        }

        public override void Update()
        {
            base.Update();

            // check for hover
            if (GameMath.CheckPointOnRectIntersection(
                (Vec2)InputHandler.MousePosition,
                (AbsolutePosition.X - Padding) * Renderer.Scale,
                (AbsolutePosition.Y - Padding) * Renderer.Scale,
                (int)(Width + 2 * Padding) * Renderer.Scale,
                (int)(Height + 2 * Padding) * Renderer.Scale))
            {
                if (!Hovered)
                    OnHover();
                Hovered = true;
            }
            else
            {
                if (Hovered)
                    OnHoverExit();
                Hovered = false;
            }

            // check for focus
            if (Hovered && InputHandler.IsMouseFirstPressed())
            {
                OnFocus();
                Focused = true;
            }
            // check for click outside textbox (lose focus)
            if (!Hovered && InputHandler.IsMouseFirstPressed())
            {
                OnFocusLost();
                Focused = false;
            }

            // accept text input
            if (Focused)
            {
                foreach (Keys k in InputHandler.KeyboardState.GetPressedKeys())
                {
                    // skip held keys
                    if (!InputHandler.IsKeyFirstPressed(k))
                        continue;

                    OnKeyPressed(k);

                    char keyChar = GetKeyChar(InputHandler.KeyboardState, k);
                    if (keyChar != (char)0 && (MaxLength <= 0 || Text.Length < MaxLength))
                        Text += keyChar;

                    // backspace
                    if (k == Keys.Back && Text.Length > 0)
                    {
                        Text = Text.Remove(Text.Length - 1, 1);
                    }

                    // escape focus
                    if (k == Keys.Escape)
                    {
                        OnFocusLost();
                        Focused = false;
                    }

                    // submit action
                    if (k == Keys.Enter)
                    {
                        OnSubmit();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw focus outline
            if (DrawFocusOutline && Focused)
            {
                spriteBatch.Draw(Renderer.Pixel,
                    new Rectangle(
                        (int)(Position.X - Padding - 2),
                        (int)(Position.Y - Padding - 2),
                        (int)((Width >= MinWidth ? Width : MinWidth) + 2 * Padding + 4),
                        (int)(Height + 2 * Padding + 4)
                        ),
                    OutlineColor
                    );
            }
            // draw background
            spriteBatch.Draw(Renderer.Pixel,
                new Rectangle(
                    (int)(Position.X - Padding),
                    (int)(Position.Y - Padding),
                    (int)((Width >= MinWidth ? Width : MinWidth) + 2 * Padding),
                    (int)(Height + 2 * Padding)
                ),
                BackgroundColor
                );

            // draw string
            spriteBatch.DrawString(Font, Text, (Vector2)Position, Color);
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

        public event EventHandler Focus;
        protected virtual void OnFocus()
        {
            Focus?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler FocusLost;
        protected virtual void OnFocusLost()
        {
            FocusLost?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Submit;
        protected virtual void OnSubmit()
        {
            Submit?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler KeyPressed;
        protected virtual void OnKeyPressed(Keys key)
        {
            KeyPressed?.Invoke(this, new KeyPressedEventArgs(key));
        }

        private static char GetKeyChar(KeyboardState state, Keys k)
        {
            bool shift = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            switch (k)
            {
                // alphabetical
                case Keys.A: return (shift ? 'A' : 'a');
                case Keys.B: return (shift ? 'B' : 'b');
                case Keys.C: return (shift ? 'C' : 'c');
                case Keys.D: return (shift ? 'D' : 'd');
                case Keys.E: return (shift ? 'E' : 'e');
                case Keys.F: return (shift ? 'F' : 'f');
                case Keys.G: return (shift ? 'G' : 'g');
                case Keys.H: return (shift ? 'H' : 'h');
                case Keys.I: return (shift ? 'I' : 'i');
                case Keys.J: return (shift ? 'J' : 'j');
                case Keys.K: return (shift ? 'K' : 'k');
                case Keys.L: return (shift ? 'L' : 'l');
                case Keys.M: return (shift ? 'M' : 'm');
                case Keys.N: return (shift ? 'N' : 'n');
                case Keys.O: return (shift ? 'O' : 'o');
                case Keys.P: return (shift ? 'P' : 'p');
                case Keys.Q: return (shift ? 'Q' : 'q');
                case Keys.R: return (shift ? 'R' : 'r');
                case Keys.S: return (shift ? 'S' : 's');
                case Keys.T: return (shift ? 'T' : 't');
                case Keys.U: return (shift ? 'U' : 'u');
                case Keys.V: return (shift ? 'V' : 'v');
                case Keys.W: return (shift ? 'W' : 'w');
                case Keys.X: return (shift ? 'X' : 'x');
                case Keys.Y: return (shift ? 'Y' : 'y');
                case Keys.Z: return (shift ? 'Z' : 'z');

                // digits
                case Keys.D1: return (shift ? '!' : '1');
                case Keys.D2: return (shift ? '@' : '2');
                case Keys.D3: return (shift ? '#' : '3');
                case Keys.D4: return (shift ? '$' : '4');
                case Keys.D5: return (shift ? '%' : '5');
                case Keys.D6: return (shift ? '^' : '6');
                case Keys.D7: return (shift ? '&' : '7');
                case Keys.D8: return (shift ? '*' : '8');
                case Keys.D9: return (shift ? '(' : '9');
                case Keys.D0: return (shift ? ')' : '0');

                // symbols
                case Keys.OemTilde: return (shift ? '~' : '`');
                case Keys.OemMinus: return (shift ? '_' : '-');
                case Keys.OemPlus: return (shift ? '+' : '=');
                case Keys.OemOpenBrackets: return (shift ? '{' : '[');
                case Keys.OemCloseBrackets: return (shift ? '}' : ']');
                case Keys.OemSemicolon: return (shift ? ':' : ';');
                case Keys.OemQuotes: return (shift ? '"' : '\'');
                case Keys.OemComma: return (shift ? '<' : ',');
                case Keys.OemPeriod: return (shift ? '>' : '.');
                case Keys.OemQuestion: return (shift ? '?' : '/');
                case Keys.OemPipe: return (shift ? '|' : '\\');

                // space
                case Keys.Space: return ' ';
            }
            return (char)0;
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Key { get; set; }

        public KeyPressedEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
