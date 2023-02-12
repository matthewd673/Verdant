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

        private List<UIElement> children = new List<UIElement>();

        // Determines if the UIStack is aligned vertically or horizontally
        public bool Vertical { get; private set; }
        // The minimum padding between all elements in the stack.
        public float Gap { get; set; } = 0f;

        private Vec2 _position = Vec2.Zero;
        public override Vec2 Position
        {
            get { return _position; }
            set
            {
                if (_position.Equals(value)) return;
                _position = value;

                // reposition all elements
                if (Vertical) Height = 0;
                else Width = 0;
                foreach(UIElement e in children)
                {
                    if (Vertical)
                    {
                        e.Position = new Vec2(_position.X, _position.Y + Height);
                        Height += e.Height + Gap;
                        Width = Math.Max(Width, e.Width);
                    }
                    else
                    {
                        e.Position = new Vec2(_position.X + Width, _position.Y);
                        Width += e.Width + Gap;
                        Height = Math.Max(Height, e.Height);
                    }
                }
            }
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
        /// Add an element onto the UIStack.
        /// The element will be repositioned but will otherwise remain unchanged.
        /// </summary>
        /// <param name="element">The element to add to the stack.</param>
        public override void AddElement(UIElement element)
        {
            if (Vertical)
            {
                element.Position = new Vec2(0, Height + (Height > 0 ? Gap : 0));
            }
            else
            {
                element.Position = new Vec2(Width + (Width > 0 ? Gap : 0), 0);
            }

            base.AddElement(element);
            // if (Vertical)
            // {
            //     element.Position = new Vec2(Position.X, Position.Y + Height);
            //     Height += element.Height + Gap;
            //     Width = Math.Max(Width, element.Width);
            // }
            // else
            // {
            //     element.Position = new Vec2(Position.X + Width, Position.Y);
            //     Width += element.Width + Gap;
            //     Height = Math.Max(Height, element.Height);
            // }

            // children.Add(element);
        }

        public void AddPadding(float padding)
        {
            Height += padding;
        }
    }
}
