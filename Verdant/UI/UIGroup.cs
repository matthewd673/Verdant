using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A collection of UIElements.
    /// </summary>
    public class UIGroup : UIElement
    {

        protected List<UIElement> children = new List<UIElement>();

        private List<UIElement> addQueue = new List<UIElement>();
        private List<UIElement> removeQueue = new List<UIElement>();

        // The number of children currently in the UIGroup.
        public int ChildCount { get; protected set; } = 0;

        // The background color of the UIGroup.
        public Color BackgroundColor { get; set; } = Color.Transparent;
        // The alignment of the UIGroup. For UIGroups that don't reposition children this will have no effect.
        public Alignment Alignment { get; set; } = Alignment.Beginning;

        /// <summary>
        /// Initialize a new UIGroup.
        /// </summary>
        /// <param name="position">The position of the UIGroup.</param>
        public UIGroup(Vec2 position) : base(position, 0, 0) { }

        /// <summary>
        /// Add an element to the UIGroup.
        /// This UIElement will be managed by the UIGroup's UIManager.
        /// </summary>
        /// <param name="element"></param>
        public virtual void AddElement(UIElement element)
        {
            addQueue.Add(element);
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        /// <summary>
        /// Add a set of elements to the UIGroup.
        /// These UIElements will be managed by the UIGroup's UIManager.
        /// </summary>
        /// <param name="elements"></param>
        public void AddElementRange(List<UIElement> elements)
        {
            foreach (UIElement e in elements)
            {
                addQueue.Add(e);
            }
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        /// <summary>
        /// Remove an element from the UIGroup.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public virtual void RemoveElement(UIElement element)
        {
            removeQueue.Add(element);
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        private void ApplyQueues()
        {
            // add
            foreach (UIElement e in addQueue)
            {
                e.Parent = this;
                e.Position = new Vec2(e.Position.X + BoxModel.Padding.Left, e.Position.Y + BoxModel.Padding.Top);

                children.Add(e);
                ChildCount++;
            }

            // remove
            foreach (UIElement e in removeQueue)
            {
                if (children.Remove(e))
                {
                    e.Parent = null;
                    ChildCount--;
                }
            }

            addQueue.Clear();
            removeQueue.Clear();
        }

        public override void Update()
        {
            base.Update();

            foreach (UIElement e in children)
            {
                e.Update();
                if (e.ForRemoval)
                    RemoveElement(e);
            }

            ApplyQueues();

            // calculate new width/height
            BoxModel.Width = 0;
            BoxModel.Height = 0;
            foreach (UIElement e in children)
            {
                BoxModel.Width = Math.Max(e.Position.X + e.BoxModel.TotalWidth, BoxModel.Width);
                BoxModel.Height = Math.Max(e.Position.Y + e.BoxModel.TotalHeight, BoxModel.Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // sort children
            IEnumerable<UIElement> renderList;
            if (Renderer.SortUIElements)
                renderList = children.OrderBy(n => n.ZIndex);
            else
                renderList = children;

            // draw background
            spriteBatch.Draw(Renderer.Pixel,
                             new Rectangle(
                                 (int)AbsoluteElementPosition.X,
                                 (int)AbsoluteElementPosition.Y,
                                 (int)BoxModel.ElementWidth,
                                 (int)BoxModel.ElementHeight
                                 ),
                             BackgroundColor);

            // draw children
            foreach (UIElement e in renderList)
            {
                if (e.Show)
                    e.Draw(spriteBatch);
            }
        }

        public override void DrawBounds(SpriteBatch spriteBatch)
        {
            base.DrawBounds(spriteBatch);

            foreach (UIElement e in children)
            {
                e.DrawBounds(spriteBatch);
            }
        }

    }
}
