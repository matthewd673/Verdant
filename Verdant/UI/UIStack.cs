using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A UIGroup that automatically arranges its children in a stack.
    /// </summary>
    public class UIStack : UIGroup
    {
        // Determines if the UIStack is aligned vertically or horizontally
        public bool Vertical { get; private set; }
        // The minimum padding between all elements in the stack.
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
        /// <param name="position">The position of the UIStack (and its first element).</param>
        /// <param name="vertical">Determines if the UIStack is aligned vertically or horizontally.</param>
        public UIStack(Vec2 position, bool vertical = true) : base(position)
        {
            _position = position;
            Vertical = vertical;
        }

        /// <summary>
        /// Add additional padding after the last element in the stack.
        /// </summary>
        /// <param name="padding">The amount of padding to add.</param>
        public void AddPadding(float padding)
        {
            if (Vertical)
                Height += padding;
            else
                Width += padding;
        }

        public override void Update()
        {
            base.Update();

            float total = 0;
            foreach (UIElement e in children)
            {
                if (Vertical)
                {
                    e.Position = new Vec2(e.Position.X, total);
                    total += e.Height;
                    total += Gap;
                }
                else
                {
                    e.Position = new Vec2(total, e.Position.Y);
                    total += e.Width;
                    total += Gap;
                }

                Width = Math.Max(Width, e.Position.X + e.Width);
                Height = Math.Max(Height, e.Position.Y + e.Height);
            }
        }
    }
}
