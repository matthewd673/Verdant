﻿using System;
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
                BoxModel.Height += gap;
            else
                BoxModel.Width += gap;
        }

        public override void Update()
        {
            base.Update();

            float total = 0;

            // recalculate group size
            BoxModel.Width = 0;
            BoxModel.Height = 0;
            foreach (UIElement e in children)
            {
                if (Vertical)
                {
                    e.Position = new Vec2(BoxModel.Padding.Left, BoxModel.Padding.Top + total);
                    total += e.BoxModel.TotalHeight;
                    total += Gap;
                }
                else
                {
                    e.Position = new Vec2(BoxModel.Padding.Left + total, BoxModel.Padding.Top);
                    total += e.BoxModel.TotalWidth;
                    total += Gap;
                }

                BoxModel.Width = Math.Max(e.Position.X + e.BoxModel.TotalWidth, BoxModel.ElementWidth);
                BoxModel.Height = Math.Max(e.Position.Y + e.BoxModel.TotalHeight, BoxModel.ElementHeight);
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
                                float center = BoxModel.Width / 2;
                                e.Position.X = center - (e.BoxModel.ElementWidth / 2) + BoxModel.Padding.Left/2;
                            }
                            else
                            {
                                float center = BoxModel.Height / 2;
                                e.Position.Y = center - (e.BoxModel.ElementHeight / 2) + BoxModel.Padding.Top/2;
                            }
                            break;
                        case Alignment.End:
                            if (Vertical)
                                e.Position.X = BoxModel.Width - e.BoxModel.TotalWidth;
                            else
                                e.Position.Y = BoxModel.Height - e.BoxModel.TotalHeight;
                            break;
                    }
                }
            }

            BoxModel.Width -= BoxModel.Padding.Left;
            BoxModel.Height -= BoxModel.Padding.Top;
        }
    }
}
