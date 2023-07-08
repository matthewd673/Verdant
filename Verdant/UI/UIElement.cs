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
            get { return Position + (Parent != null ? Parent.AbsoluteContentPosition : Vec2.Zero); }
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
            get { return ElementPosition + (Parent != null ? Parent.AbsoluteContentPosition : Vec2.Zero); }
        }

        // The position of the content of the UIElement (its position + margin + padding).
        public Vec2 ContentPosition
        {
            get
            {
                return Position +
                    new Vec2(
                        BoxModel.Margin.Left + BoxModel.Padding.Left,
                        BoxModel.Margin.Top + BoxModel.Padding.Top
                        );
            }
        }
        // The absolute screen position of the content of the UIElement (its position + margin + padding).
        public Vec2 AbsoluteContentPosition
        {
            get { return ContentPosition + (Parent != null ? Parent.AbsoluteContentPosition : Vec2.Zero); }
        }

        // The UIElement's box model, including width, height, margin, and padding.
        public virtual BoxModel BoxModel { get; set; } = new();

        // Determines if the UIElement will be removed at the end of the update loop.
        public bool ForRemoval { get; set; }

        // The z-index, used for sorting.
        public int ZIndex { get; set; }

        // Determines if the UIElement should be rendered.
        public bool Show { get; set; } = true;

        private bool wasHovered = false;
        // Determines if the mouse is hovering over this UIElement.
        public bool Hovered { get; protected set; } = false;

        private bool clickBeganOnElement = false;

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
        public virtual void Update()
        {
            // calculate hover
            wasHovered = Hovered;
            Hovered = GameMath.PointOnRectIntersection(
                (Vec2)InputHandler.MousePosition,
                AbsoluteElementPosition.X, AbsoluteElementPosition.Y,
                BoxModel.ElementWidth, BoxModel.ElementHeight);

            if (!wasHovered && Hovered)
                OnHoverBegin();
            else if (wasHovered && !Hovered)
                OnHoverEnd();

            // calculate click
            if (Hovered && InputHandler.IsMouseFirstPressed())
            {
                OnMouseDown();
                clickBeganOnElement = true;
            }
            else if (InputHandler.IsMouseFirstPressed())
            {
                clickBeganOnElement = false;
            }

            if (Hovered && InputHandler.IsMouseFirstReleased())
            {
                OnMouseUp();
            }
            // a click only counts if you pressed and released on the element
            // (a click will trigger (in this order):  OnMouseDown(), OnMouseUp(), OnClick())
            if (Hovered && InputHandler.IsMouseFirstReleased() && clickBeganOnElement)
            {
                OnClick();
            }
        }

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
                AbsolutePosition,
                AbsolutePosition + (new Vec2(BoxModel.TotalWidth, BoxModel.TotalHeight) * Renderer.UIScale),
                Color.PaleGreen
                );

            // draw padding
            Renderer.DrawRectangle(spriteBatch,
                                   AbsoluteContentPosition,
                                   AbsoluteContentPosition + (new Vec2(BoxModel.Width, BoxModel.Height) * Renderer.UIScale),
                                   Color.Pink
                                   );

            // draw border
            Renderer.DrawRectangle(spriteBatch,
                AbsoluteElementPosition,
                AbsoluteElementPosition + (new Vec2(BoxModel.ElementWidth, BoxModel.ElementHeight) * Renderer.UIScale),
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

        /// <summary>
        /// Called when the mouse starts hovering over the UIElement.
        /// </summary>
        public virtual void OnHoverBegin() { }
        /// <summary>
        /// Called when the mouse stops hovering over the UIElement.
        /// </summary>
        public virtual void OnHoverEnd() { }

        /// <summary>
        /// Called when the mouse is first pressed on this UIElement.
        /// </summary>
        public virtual void OnMouseDown() { }
        /// <summary>
        /// Called when the mouse is first released on this UIElement.
        /// </summary>
        public virtual void OnMouseUp() { }
        /// <summary>
        /// Called when the UIElement is formally clicked.
        /// A click means the mouse is pressed over a UIElement and then released over the same UIElement.
        /// A click will call OnMouseDown(), OnMouseUp(), and OnClick() in that order.
        /// </summary>
        public virtual void OnClick() { }

    }
}
