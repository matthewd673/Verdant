using System;
using Microsoft.Xna.Framework;

namespace Verdant
{
    public class TransformAnimation
    {

        TransformState from;
        TransformState to;

        int animateDuration;
        int animateCountdown;

        TransformState current;
        TransformState intervals;

        /// <summary>
        /// Initialize a new TransformAnimation.
        /// </summary>
        /// <param name="from">The beginning TransformState.</param>
        /// <param name="to">The ending TransformState.</param>
        /// <param name="duration">The duration of the animation (number of frames).</param>
        public TransformAnimation(TransformState from, TransformState to, int duration)
        {
            this.from = from;
            this.to = to;
            animateDuration = duration;
            animateCountdown = duration;

            current = new TransformState(from);
            intervals = CalculateIntervals(from, to, duration);
        }

        /// <summary>
        /// Given a beginning and ending TransformState, and a duration, calculate the intervals that each property will change at.
        /// </summary>
        /// <param name="from">The beginning TransformState.</param>
        /// <param name="to">The ending TransformState.</param>
        /// <param name="duration">The duration of the animation (number of frames).</param>
        /// <returns>A TransformState containing the intervals that each property should change at.</returns>
        TransformState CalculateIntervals(TransformState from, TransformState to, int duration)
        {
            //calculate total differences
            float dX = to.X - from.Y;
            float dY = to.Y - from.Y;
            float dW = to.Width - from.Width;
            float dH = to.Height - from.Height;
            float dR = to.Rotation - from.Rotation;

            //build intervals into new transformstate
            return new TransformState(
                dX / duration,
                dY / duration,
                dW / duration,
                dH / duration,
                dR / duration
                );
        }

        /// <summary>
        /// Perform the next step of the animation, and return a TransformState representing the current state.
        /// </summary>
        /// <returns>A TransformState representing the current state of the animation.</returns>
        public TransformState Animate()
        {
            //perform animation if it isn't complete
            if (animateCountdown > 0)
            {
                //update current according to intervals
                current.X += intervals.X;
                current.Y += intervals.Y;
                current.Width += intervals.Width;
                current.Height += intervals.Height;
                current.Rotation += intervals.Rotation;

                //tick animation
                animateCountdown--;

                return current; //return current state
            }

            return to; //if animation complete, return final state
        }

        /// <summary>
        /// Get the current state of the animation without performing any animation.
        /// </summary>
        /// <returns>A TransformState representing the current state of the animation.</returns>
        public TransformState GetCurrentFrame()
        {
            return current;
        }

        /// <summary>
        /// Reset the animation to its original state.
        /// </summary>
        public void Reset()
        {
            current = new TransformState(from);
            animateCountdown = animateDuration;
        }

        /// <summary>
        /// Create a TransformState from the current values of a given Entity.
        /// </summary>
        /// <param name="e">The Entity to read from.</param>
        /// <returns>A TransformState representing the Entity's current state.</returns>
        //public static TransformState CaptureState(Entity e)
        //{
        //    return new TransformState(e.Position.X, e.Position.Y, e.Width, e.Height, e.Rotation);
        //}

    }
}
