using System;
using System.Collections.Generic;

namespace Verdant.UI
{
    public class UIManager
    {

        List<UIElement> elements = new List<UIElement>();
        List<UIElement> addQueue = new List<UIElement>();
        List<UIElement> removeQueue = new List<UIElement>();

        /// <summary>
        /// Initialize a new UIManager.
        /// </summary>
        public UIManager() { }

        /// <summary>
        /// Add a new UIElement to the manager.
        /// </summary>
        /// <param name="e">The element to add.</param>
        public void AddElement(UIElement e)
        {
            e.Manager = this;
            addQueue.Add(e);
        }

        /// <summary>
        /// Add a collection of UIElements to the manager.
        /// </summary>
        /// <param name="l">The List of elements to add.</param>
        public void AddElementRange(List<UIElement> l)
        {
            foreach (UIElement e in l)
            {
                e.Manager = this;
                addQueue.Add(e);
            }
        }

        /// <summary>
        /// Remove a UIElement from the manager.
        /// </summary>
        /// <param name="e">The element to remove.</param>
        public void RemoveElement(UIElement e)
        {
            e.Manager = null;
            removeQueue.Add(e);
        }
        
        /// <summary>
        /// Get all elements managed by this UIManager.
        /// </summary>
        /// <returns>A list of all elements in the manager.</returns>
        public List<UIElement> GetElements()
        {
            return elements;
        }

        /// <summary>
        /// Force the UIManager to add and remove all queued elements. Use with caution.
        /// </summary>
        public void ForceApplyQueues()
        {
            //remove and add
            foreach (UIElement e in removeQueue)
                elements.Remove(e);
            elements.AddRange(addQueue);
            removeQueue.Clear();
            addQueue.Clear();
        }

        /// <summary>
        /// Update all elements in the manager.
        /// </summary>
        public void Update()
        {
            foreach (UIElement e in elements)
            {
                e.Update();
                if (e.ForRemoval)
                    removeQueue.Add(e);
            }

            // remove and add
            elements.AddRange(addQueue);
            foreach (UIElement e in removeQueue)
                elements.Remove(e);
            removeQueue.Clear();
            addQueue.Clear();
        }

    }
}
