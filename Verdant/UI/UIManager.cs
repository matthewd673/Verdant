using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verdant.UI
{
    /// <summary>
    /// Manages the creation, deletion, and update logic of UIElements
    /// </summary>
    public class UIManager
    {

        // The Scene that this UIManager belongs to.
        public Scene Scene { get; set; }

        private List<UIElement> elements = new List<UIElement>();

        private List<UIElement> addQueue = new List<UIElement>();
        private List<UIElement> removeQueue = new List<UIElement>();

        // The number of UIElements currently managed by the UIManager.
        public int UIElementCount { get; private set; }

        private Stopwatch updatePerformanceTimer = new Stopwatch();
        // The duration (in milliseconds) of the last Update call.
        public float UpdateDuration { get; private set; }
        // Indicates if the UIManager has entered the update loop.
        public bool Updating { get; protected set; } = false;

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
            addQueue.Add(e);
            if (!Updating) ApplyQueues();
        }

        /// <summary>
        /// Add a collection of UIElements to the manager.
        /// </summary>
        /// <param name="l">The List of elements to add.</param>
        public void AddElementRange(List<UIElement> l)
        {
            foreach (UIElement e in l)
            {
                addQueue.Add(e);
            }
            if (!Updating) ApplyQueues();
        }

        /// <summary>
        /// Remove a UIElement from the manager.
        /// </summary>
        /// <param name="e">The element to remove.</param>
        public void RemoveElement(UIElement e)
        {
            removeQueue.Add(e);
            if (!Updating) ApplyQueues();
        }

        /// <summary>
        /// Get all elements managed by this UIManager.
        /// </summary>
        /// <returns>A list of all elements in the manager.</returns>
        public List<UIElement> GetElements()
        {
            return elements;
        }

        private void ApplyQueues()
        {
            foreach (UIElement e in addQueue)
            {
                e.Manager = this;
                elements.Add(e);
                UIElementCount++;
                e.OnAdd();
            }

            foreach (UIElement e in removeQueue)
            {
                if (elements.Remove(e))
                {
                    e.Manager = null;
                    UIElementCount--;
                    e.OnRemove();
                }
            }

            addQueue.Clear();
            removeQueue.Clear();
        }

        /// <summary>
        /// Update all elements in the manager.
        /// </summary>
        public void Update()
        {
            updatePerformanceTimer.Start();
            Updating = true;

            foreach (UIElement e in elements)
            {
                e.Update();
                if (e.ForRemoval)
                    RemoveElement(e);
            }

            // remove and add
            ApplyQueues();

            updatePerformanceTimer.Stop();
            UpdateDuration = updatePerformanceTimer.ElapsedMilliseconds;
            updatePerformanceTimer.Reset();
        }

    }
}
