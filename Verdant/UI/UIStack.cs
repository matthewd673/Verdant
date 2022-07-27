using System;
using System.Collections.Generic;

namespace Verdant.UI
{
    public class UIStack : UIElement
    {

        private List<UIElement> children = new List<UIElement>();

        public float Height { get; private set; } = 0;

        public UIStack(Vec2 pos) : base(pos)
        {

        }

        public void AddElement(UIElement element, float elementHeight)
        {
            element.Position = new Vec2(Position.X, Position.Y + Height);
            Height += elementHeight;
            children.Add(element);

            Manager.AddElement(element);
        }

        public void AddPadding(float paddingHeight)
        {
            Height += paddingHeight;
        }

    }
}
