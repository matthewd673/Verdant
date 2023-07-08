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
        // The UIGroup that contains the element. UIElements don't need to have a parent.
        public UIGroup Parent { get; set; }

        // The relative position of the UIElement.
        public virtual Vec2 Position { get; set; }
        // The absolute screen position of the UIElement.
        public Vec2 AbsolutePosition
        {
            get { return Position + (Parent != null ? Parent.AbsoluteElementPosition : Vec2.Zero); }
        }

        // The position of the visible part of the UIElement (its position + margin).
        public Vec2 ElementPosition
        {
            get
            {
                return Position +
                    new Vec2(BoxModel.Margin.Left, BoxModel.Margin.Top);
            }
        }
        // The absolute screen position of the visible part of the UIElement (its position + margin).
        public Vec2 AbsoluteElementPosition
        {
            get { return ElementPosition + (Parent != null ? Parent.AbsoluteElementPosition : Vec2.Zero); }
        }

        // The position of the content of the UIElement (its position + margin + padding).
        public Vec2 ContentPosition
        {
            get
            {
                return Position +
                    new Vec2(
                        BoxModel.Margin.Left + BoxModel.Padding.Left,
                        BoxModel.Margin.Top + BoxModel.Margin.Left
                        );
            }
        }
        // The absolute screen position of the content of the UIElement (its position + margin + padding).
        public Vec2 AbsoluteContentPosition
        {
            get { return ContentPosition + (Parent != null ? Parent.AbsoluteElementPosition : Vec2.Zero); }
        }

        // The UIElement's box model, including width, height, margin, and padding.
        public virtual BoxModel BoxModel { get; set; } = new();

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
            BoxModel.Width = width;
            BoxModel.Height = height;
        }

        /// <summary>
        /// Called when the UIElement has been added to a UIManager and is ready to use.
        /// NOTE: This occurs immediately after the UIElement has been processed through the add queue.
        /// </summary>
        public virtual void OnAdd() { }

        /// <summary>
        /// Called when the UIElement has been removed from a UIManager.
        /// NOTE: This occurs immediately after the UIElement has been processed through the remove queue.
        /// This will not be called if a UIElement is removed from a UIManager that it wasn't managed by.
        /// </summary>
        public virtual void OnRemove() { }

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
                (AbsolutePosition + new Vec2(BoxModel.TotalWidth, BoxModel.TotalHeight)) * Renderer.Scale,
                Color.PaleGreen
                );
            // draw padding
            Renderer.DrawRectangle(spriteBatch,
                                   AbsoluteContentPosition * Renderer.Scale,
                                   (AbsoluteContentPosition - new Vec2(BoxModel.Padding.Right, BoxModel.Padding.Bottom)) * Renderer.Scale,
                                   Color.Pink
                                   );
            // draw border
            Renderer.DrawRectangle(spriteBatch,
                AbsoluteElementPosition,
                (AbsoluteElementPosition + new Vec2(BoxModel.ElementWidth, BoxModel.ElementHeight)) * Renderer.Scale,
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
