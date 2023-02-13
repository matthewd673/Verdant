using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A UIElement that accepts text input.
    /// </summary>
    public class UITextBox : UIElement
    {

        // Denotes if the UITextBox is hovered.
        public bool Hovered { get; private set; }
        // Denotes if the UITextBox is focused. If the UITextBox is focused, it is accepting input.
        public bool Focused { get; private set; }

        private string _text;
        private Vector2 _textMeasurements;
        // The string contents of the UITextBox.
        public string Text
        {
            get { return _text; }
            set
            {
                if (MaxLength >= 0 && value.Length > MaxLength)
                    value = value[..MaxLength];
                _text = value;
                _textMeasurements = Font.MeasureString(_text);
                AbsoluteWidth = (int)_textMeasurements.X;
                OnChanged();
            }
        }
        // The maximum number of characters allowed in the text string. Set to a negative number for infinite length.
        public int MaxLength { get; set; } = -1;

        // The font used to draw the UITextBox.
        public SpriteFont Font { get; private set; }
        // The color of the text.
        public Color Color { get; set; } = Color.Black;
        // The color of the background box.
        public Color BackgroundColor { get; set; } = Color.White;
        // The color of the focus outline (if enabled).
        public Color OutlineColor { get; set; } = Color.Black;
        // The thickness of the focus outline (if enabled).
        public int OutlineThickness { get; set; } = 1;

        // Determines if an outline should be drawn when the UITextBox is focused.
        public bool ShowFocusOutline { get; set; } = true;
        // Determines if a caret should be drawn when typing. 
        public bool ShowCaret { get; set; } = true;
        // Determines if the UITextBox should only accept numeric (int & real) input.
        public bool Numeric { get; set; } = false;

        private int caretPreWidth = 0;
        private int _caretPosition;
        // The position of the caret within the string.
        public int CaretPosition
        {
            get { return _caretPosition; }
            set
            {
                _caretPosition = value;
                if (value > Text.Length)
                    _caretPosition = Text.Length;
                else if (value < 0)
                    _caretPosition = 0;

                caretPreWidth = (int)Font.MeasureString(Text.Substring(0, _caretPosition)).X;
            }
        }

        private int initialHoldFrames = 35;
        private int repeatHoldFrames = 2;
        private int currentHoldFrames = 0;
        private bool repeatingHold = false;

        private float _width;
        // The width of the UITextBox (may change when the text changes).
        protected override float AbsoluteWidth
        {
            get { return (_width > MinWidth) ? _width : MinWidth; }
            set { _width = value; }
        }
        // The minimum (pixel) width of the UITextBox.
        public float MinWidth { get; set; } = 64;
        // The height of the UITextBox (does not change when the text changes).
        protected override float AbsoluteHeight { get { return Font.LineSpacing; } }

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
                (InnerPosition.X) * Renderer.Scale,
                (InnerPosition.Y) * Renderer.Scale,
                (int)(InnerWidth) * Renderer.Scale,
                (int)(InnerHeight) * Renderer.Scale))
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
                bool sawHeldKey = false;
                foreach (Keys k in InputHandler.KeyboardState.GetPressedKeys())
                {
                    if (!InputHandler.IsKeyFirstPressed(k))
                    {
                        if (!sawHeldKey)
                        {
                            currentHoldFrames++;
                        }

                        sawHeldKey = true;

                        // hold is repeating but hasn't hit threshhold
                        if (repeatingHold && currentHoldFrames < repeatHoldFrames)
                        {
                            continue;
                        }
                        // hold is repeating and has hit threshhold
                        // so, trigger input and reset counter
                        else if (repeatingHold)
                        {
                            currentHoldFrames = 0;
                        }

                        // hold is not yet repeating but has hit the threshhold
                        // so, trigger input and switch to repeating mode
                        if (!repeatingHold && currentHoldFrames >= initialHoldFrames)
                        {
                            currentHoldFrames = 0;
                            repeatingHold = true;
                        }
                        else if (!repeatingHold)
                        {
                            continue;
                        }
                    }

                    OnKeyPressed(k);

                    char keyChar = GetKeyChar(InputHandler.KeyboardState, k);
                    if (keyChar != (char)0 && (MaxLength <= 0 || Text.Length < MaxLength) &&
                        (!Numeric || (Numeric && CharIsNumeric(keyChar))))
                    {
                        Text = Text.Insert(CaretPosition, keyChar.ToString());
                        CaretPosition++;
                    }

                    // backspace
                    if (k == Keys.Back && Text.Length > 0 && CaretPosition > 0)
                    {
                        Text = Text.Remove(CaretPosition - 1, 1);
                        CaretPosition--;
                    }

                    // delete
                    if (k == Keys.Delete && Text.Length > CaretPosition)
                    {
                        Text = Text.Remove(CaretPosition, 1);
                    }

                    // arrow keys
                    if (k == Keys.Left)
                    {
                        CaretPosition--;
                    }
                    if (k == Keys.Right)
                    {
                        CaretPosition++;
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

                if (!sawHeldKey)
                {
                    currentHoldFrames = 0;
                    repeatingHold = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw focus outline
            if (ShowFocusOutline && Focused)
            {
                spriteBatch.Draw(Renderer.Pixel,
                    new Rectangle(
                        (int)(InnerPosition.X - OutlineThickness),
                        (int)(InnerPosition.Y - OutlineThickness),
                        (int)((AbsoluteWidth >= MinWidth ? InnerWidth : MinWidth + Padding.Left + Padding.Right) + 2*OutlineThickness),
                        (int)(InnerHeight + 2*OutlineThickness)
                        ),
                    OutlineColor
                    );
            }
            // draw background
            spriteBatch.Draw(Renderer.Pixel,
                new Rectangle(
                    (int)(InnerPosition.X),
                    (int)(InnerPosition.Y),
                    (int)(AbsoluteWidth >= MinWidth ? InnerWidth: MinWidth + Padding.Left + Padding.Right),
                    (int)(InnerHeight)
                ),
                BackgroundColor
                );

            // draw string
            spriteBatch.DrawString(Font, Text, new Vector2(AbsolutePosition.X + Padding.Left, AbsolutePosition.Y + Padding.Top), Color);

            // draw caret
            if (ShowCaret && Focused)
            {
                spriteBatch.Draw(Renderer.Pixel,
                    new Rectangle(
                        (int)(InnerPosition.X + caretPreWidth),
                        (int)InnerPosition.Y,
                        1,
                        (int)(AbsoluteHeight)
                        ),
                    Color.Black);
            }
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

        public event EventHandler Changed;
        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
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

        private static bool CharIsNumeric(char c)
        {
            return (c >= 48 && c <= 57) || c == '.' || c == '-';
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
