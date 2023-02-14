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

        public int ChildCount { get; protected set; } = 0;

        public Color BackgroundColor { get; set; } = Color.Transparent;

        public UIGroup(Vec2 position) : base(position, 0, 0) { }

        public virtual void AddElement(UIElement element)
        {
            addQueue.Add(element);
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        public void AddElementRange(List<UIElement> elements)
        {
            foreach (UIElement e in elements)
            {
                addQueue.Add(e);
            }
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

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
                e.Position = new Vec2(e.Position.X + Padding.Left, e.Position.Y + Padding.Top);

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
            AbsoluteWidth = 0;
            AbsoluteHeight = 0;
            foreach (UIElement e in children)
            {
                AbsoluteWidth = Math.Max(e.Position.X + e.Width, AbsoluteWidth);
                AbsoluteHeight = Math.Max(e.Position.Y + e.Height, AbsoluteHeight);
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
                                 (int)InnerPosition.X,
                                 (int)InnerPosition.Y,
                                 (int)InnerWidth,
                                 (int)InnerHeight
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
