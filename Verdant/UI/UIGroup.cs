using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIGroup : UIElement
    {

        private List<UIElement> children = new List<UIElement>();

        private List<UIElement> addQueue = new List<UIElement>();
        private List<UIElement> removeQueue = new List<UIElement>();

        public int ChildCount { get; protected set; } = 0;

        public UIGroup(Vec2 position) : base(position, 0, 0) { }

        public void AddElement(UIElement e)
        {
            addQueue.Add(e);
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        public void AddElementRange(List<UIElement> l)
        {
            foreach (UIElement e in l)
            {
                addQueue.Add(e);
            }
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        public void RemoveElement(UIElement e)
        {
            removeQueue.Add(e);
            if (Manager == null || !Manager.Updating) ApplyQueues();
        }

        private void ApplyQueues()
        {
            foreach (UIElement e in addQueue)
            {
                e.Parent = this;
                e.Position += Position;

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
