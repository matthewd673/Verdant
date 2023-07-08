using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// A simple way to play multiple Animations in sequence.
    /// </summary>
    public class AnimationStateMachine : RenderObject
    {

        List<Animation> queue = new List<Animation>();
        // An Animation that will be drawn if the queue is empty.
        public Animation DefaultAnimation { get; set; }
        // The current Animation in the queue (or DefaultAnimation, if the queue is empty).
        public Animation CurrentAnimation
        {
            get
            {
                return queue.Count > 0 ? queue[0] : DefaultAnimation;
            }
        }

        /// <summary>
        /// Initialize a new AnimationStateMachine.
        /// </summary>
        public AnimationStateMachine() { }

        /// <summary>
        /// Add an Animation to the queue.
        /// </summary>
        /// <param name="a">The Animation to enqueue.</param>
        public void QueueAnimation(Animation a)
        {
            queue.Add(a);
        }

        /// <summary>
        /// Skip the current Animation and play the next one in the queue.
        /// The current Animation will not be marked as completed.
        /// </summary>
        public void SkipCurrentAnimation()
        {
            if (queue.Count > 0)
                queue.RemoveAt(0);
        }

        /// <summary>
        /// Clear the Animation queue.
        /// </summary>
        public void ClearQueue()
        {
            queue.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            CurrentAnimation?.Draw(spriteBatch, bounds);
            if (CurrentAnimation.Complete && !CurrentAnimation.Looping && queue.Count > 0)
                queue.RemoveAt(0);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin)
        {
            CurrentAnimation.Draw(spriteBatch, bounds, angle, origin);
            if (CurrentAnimation.Complete && !CurrentAnimation.Looping && queue.Count > 0)
                queue.RemoveAt(0);
        }

        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, int x, int y = 0)
        {
            CurrentAnimation?.DrawIndex(spriteBatch, bounds, x, y);
            if (CurrentAnimation.Complete && !CurrentAnimation.Looping && queue.Count > 0)
                queue.RemoveAt(0);
        }

        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin, int x, int y = 0)
        {
            CurrentAnimation?.DrawIndex(spriteBatch, bounds, angle, origin, x, y);
            if (CurrentAnimation.Complete && !CurrentAnimation.Looping && queue.Count > 0)
                queue.RemoveAt(0);
        }

    }
}
