using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// An object within the UI.
    /// </summary>
    public class UIElement
    {

        // The UIManager managing the element.
        public UIManager Manager { get; set; }
        // The UIGroup that contains the element. Not all UIElements have a parent.
        public UIGroup Parent { get; set; }

        // The position of the UIElement relative to its parent.
        public virtual Vec2 Position { get; set; }
        // The top-left position of the UIElement on the screen (where its margin begins).
        public Vec2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.InnerPosition + Position;
                return Position;
            }
        }

        // The width of the UIElement, not including padding or margin.
        protected virtual float AbsoluteWidth { get; set; }
        // The height of the UIElement, not including padding or margin.
        protected virtual float AbsoluteHeight { get; set; }

        // The width of the UIElement in screen space (including padding and margin).
        public float Width
        {
            get
            {
                return AbsoluteWidth + Padding.Left + Padding.Right + Margin.Left + Margin.Right;
            }
            set
            {
                AbsoluteWidth = value; // TODO: this seems right, but may not be intuitive
            }
        }
        // The height of the UIElement in screen space (including padding and margin).
        public float Height
        {
            get
            {
                return AbsoluteHeight + Padding.Top + Padding.Bottom + Margin.Top + Margin.Bottom;
            }
            set
            {
                AbsoluteHeight = value;
            }
        }

        // The margin surrounding the UIElement.
        public BoxDimensions Margin { get; set; }
        // The padding within the UIElement.
        public BoxDimensions Padding { get; set; }

        protected Vec2 InnerPosition { get { return AbsolutePosition + new Vec2(Margin.Left, Margin.Top); } }
        protected float InnerWidth { get { return AbsoluteWidth + Padding.Left + Padding.Right; } }
        protected float InnerHeight { get { return AbsoluteHeight + Padding.Top + Padding.Bottom; } }

        // Determines if the UIElement will be removed at the end of the update loop.
        public bool ForRemoval { get; set; }

        // The z-index, used for sorting.
        public int ZIndex { get; set; }

        // Determines if the UIElement should be rendered.
        public bool Show { get; set; } = true;

        /// <summary>
        /// Initialize a new UIElement.
        /// </summary>
        /// <param name="position">The position of the UIElement.</param>
        /// <param name="width">The width of the UIElement.</param>
        /// <param name="height">The height of the UIElement.</param>
        public UIElement(Vec2 position, float width, float height)
        {
            Position = position;
            AbsoluteWidth = width;
            AbsoluteHeight = height;
        }

        /// <summary>
        /// Update the UIElement.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Draw the UIElement.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Draw the UIElement box model bounds.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        public virtual void DrawBounds(SpriteBatch spriteBatch)
        {
            // draw with margin
            Renderer.DrawRectangle(spriteBatch,
                AbsolutePosition * Renderer.Scale,
                (AbsolutePosition + new Vec2(Width, Height)) * Renderer.Scale,
                Color.PaleGreen
                );
            // draw padding
            Renderer.DrawRectangle(spriteBatch,
                                   (InnerPosition + new Vec2(Padding.Left, Padding.Top)) * Renderer.Scale,
                                   (InnerPosition + new Vec2(InnerWidth, InnerHeight) - new Vec2(Padding.Right, Padding.Bottom)) * Renderer.Scale,
                                   Color.Pink
                                   );
            // draw border
            Renderer.DrawRectangle(spriteBatch,
                InnerPosition,
                (InnerPosition + new Vec2(InnerWidth, InnerHeight)) * Renderer.Scale,
                Color.Black
                );
        }

        /// <summary>
        /// Check if this UIElement is of a given type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>True if the UIElement is of the given type or is a subclass of the given type.</returns>
        public bool IsType(Type t)
        {
            return GetType() == t || GetType().IsSubclassOf(t);
        }

    }
}
