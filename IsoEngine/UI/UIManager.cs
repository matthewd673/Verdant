﻿using System;
using System.Collections.Generic;

namespace IsoEngine.UI
{
    public class UIManager
    {

        List<UIElement> elements = new List<UIElement>();
        List<UIElement> addQueue = new List<UIElement>();
        List<UIElement> removeQueue = new List<UIElement>();

        public UIManager()
        {

        }

        public void AddElement(UIElement e)
        {
            addQueue.Add(e);
        }

        public void AddElementRange(List<UIElement> l)
        {
            foreach (UIElement e in l)
            {
                e.Manager = this;
                addQueue.Add(e);
            }
        }

        public void RemoveElement(UIElement e)
        {
            e.Manager = null;
            removeQueue.Add(e);
        }
        
        public List<UIElement> GetElements()
        {
            return elements;
        }

        public void ForceApplyQueues()
        {
            //remove and add
            foreach (UIElement e in removeQueue)
                elements.Remove(e);
            elements.AddRange(addQueue);
            removeQueue.Clear();
            addQueue.Clear();
        }

        public void Update()
        {
            foreach (UIElement e in elements)
            {
                e.Update();
                if (e.IsForRemoval())
                    removeQueue.Add(e);
            }
            //remove and add
            foreach (UIElement e in removeQueue)
                elements.Remove(e);
            elements.AddRange(addQueue);
            removeQueue.Clear();
            addQueue.Clear();
        }

    }
}
