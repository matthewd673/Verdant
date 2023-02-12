using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A collection of UIElements.
    /// </summary>
    public class UIGroup : UIElement
    {

        private List<UIElement> children = new List<UIElement>();

        private List<UIElement> addQueue = new List<UIElement>();
        private List<UIElement> removeQueue = new List<UIElement>();

        public int ChildCount { get; protected set; } = 0;

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
            foreach (UIElement e in addQueue)
            {
                e.Parent = this;

                // calculate new width/height
                Width = Math.Max(e.Position.X + e.Width, Width);
                Height = Math.Max(e.Position.Y + e.Height, Height);

                children.Add(e);
                ChildCount++;
            }

            foreach (UIElement e in removeQueue)
            {
                if (children.Remove(e))
                {
                    e.Parent = null;
                    ChildCount--;
                }
            }

            // if elements were removed, must recompute width/height
            // hopefully, removing an element will be pretty rare
            if (removeQueue.Any())
            {
                Width = 0;
                Height = 0;
                foreach (UIElement e in children)
                {
                    Width = Math.Max(e.Position.X + e.Width, Width);
                    Height = Math.Max(e.Position.Y + e.Height, Height);
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

            // recalculate bounds
            Width = 0;
            Height = 0;
            foreach (UIElement e in children)
            {
                Width = Math.Max(e.Position.X + e.Width, Width);
                Height = Math.Max(e.Position.Y + e.Height, Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            IEnumerable<UIElement> renderList;
            if (Renderer.SortUIElements)
                renderList = children.OrderBy(n => n.ZIndex);
            else
                renderList = children;

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
