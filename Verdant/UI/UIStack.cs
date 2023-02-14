using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Verdant.Debugging;

namespace Verdant.UI
{
    /// <summary>
    /// A UIGroup that automatically positions its children in a stack.
    /// </summary>
    public class UIStack : UIGroup
    {
        // Indicates if the UIStack is aligned vertically or horizontally
        public bool Vertical { get; private set; }
        // The minimum margin between all elements in the stack.
        public float Gap { get; set; } = 0f;

        private Vec2 _position = Vec2.Zero;
        public override Vec2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Initialize a new UIStack.
        /// </summary>
        /// <param name="position">The position of the UIStack.</param>
        /// <param name="vertical">Determines if the UIStack is aligned vertically or horizontally.</param>
        public UIStack(Vec2 position, bool vertical = true) : base(position)
        {
            _position = position;
            Vertical = vertical;
        }

        /// <summary>
        /// Add an additional gap after the last element in the stack.
        /// </summary>
        /// <param name="gap">The size of the gap.</param>
        public void AddGap(float gap)
        {
            if (Vertical)
                AbsoluteHeight += gap;
            else
                AbsoluteWidth += gap;
        }

        public override void Update()
        {
            base.Update();

            float total = 0;

            // recalculate group size
            AbsoluteWidth = 0;
            AbsoluteHeight = 0;
            foreach (UIElement e in children)
            {
                if (Vertical)
                {
                    e.Position = new Vec2(Padding.Left, Padding.Top + total);
                    total += e.Height;
                    total += Gap;
                }
                else
                {
                    e.Position = new Vec2(Padding.Left + total, Padding.Top);
                    total += e.Width;
                    total += Gap;
                }

                AbsoluteWidth = Math.Max(e.Position.X + e.Width, AbsoluteWidth);
                AbsoluteHeight = Math.Max(e.Position.Y + e.Height, AbsoluteHeight);
            }

            // reposition into alignment
            if (Alignment != Alignment.Beginning)
            {
                foreach (UIElement e in children)
                {
                    switch (Alignment)
                    {
                        case Alignment.Center:
                            if (Vertical)
                            {
                                float center = (InnerWidth - Padding.Left - Padding.Right) / 2;
                                e.Position.X = center - (e.Width / 2) + Padding.Left/2;
                            }
                            else
                            {
                                float center = (InnerHeight - Padding.Top - Padding.Bottom) / 2;
                                e.Position.Y = center - (e.Height / 2) + Padding.Top/2;
                            }
                            break;
                        case Alignment.End:
                            if (Vertical)
                                e.Position.X = InnerWidth - 2*Padding.Right - e.Width;
                            else
                                e.Position.Y = InnerHeight - 2*Padding.Bottom - e.Height;
                            break;
                    }
                }
            }

            AbsoluteWidth -= Padding.Left;
            AbsoluteHeight -= Padding.Top;
        }
    }
}
