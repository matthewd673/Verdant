using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class TransformAnimation
    {

        TransformState from;
        TransformState to;
        Cooldown animateCooldown;

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
            animateCooldown = new Cooldown(duration);

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
            float dX = to.x - from.y;
            float dY = to.y - from.y;
            float dW = to.w - from.w;
            float dH = to.h - from.h;
            float dR = to.r - from.r;

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
            if (!animateCooldown.Check())
            {
                //update current according to intervals
                current.x += intervals.x;
                current.y += intervals.y;
                current.w += intervals.w;
                current.h += intervals.h;
                current.r += intervals.r;

                //tick animation
                animateCooldown.Tick();

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
            animateCooldown.Reset();
        }

        /// <summary>
        /// Create a TransformState from the current values of a given Entity.
        /// </summary>
        /// <param name="e">The Entity to read from.</param>
        /// <returns>A TransformState representing the Entity's current state.</returns>
        public static TransformState CaptureState(Entity e)
        {
            return new TransformState(e.Position.X, e.Position.Y, e.Width, e.Height, 0); //rotation currently unsupported by entities
        }

        public struct TransformState
        {
            public float x;
            public float y;
            public float w;
            public float h;
            public float r;

            /// <summary>
            /// Initialize a new TransformState.
            /// </summary>
            /// <param name="x">The x coordinate.</param>
            /// <param name="y">The y coordinate.</param>
            /// <param name="w">The width.</param>
            /// <param name="h">The height.</param>
            /// <param name="r">The rotation.</param>
            public TransformState(float x, float y, float w, float h, float r)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
                this.r = r;
            }

            /// <summary>
            /// Initialize a new TransformState from an existing TransformState.
            /// </summary>
            /// <param name="state">The TransformState to copy from.</param>
            public TransformState(TransformState state)
            {
                x = state.x;
                y = state.y;
                w = state.w;
                h = state.h;
                r = state.r;
            }

            public static TransformState operator +(TransformState a, TransformState b) => new TransformState(a.x + b.x, a.y + b.y, a.w + b.w, a.h + b.h, a.r + b.r);
            public static TransformState operator -(TransformState a, TransformState b) => new TransformState(a.x - b.x, a.y - b.y, a.w - b.w, a.h - b.h, a.r - b.r);
            public static TransformState operator *(TransformState a, TransformState b) => new TransformState(a.x * b.x, a.y * b.y, a.w * b.w, a.h * b.h, a.r * b.r);
            public static TransformState operator /(TransformState a, TransformState b) => new TransformState(a.x / b.x, a.y / b.y, a.w / b.w, a.h / b.h, a.r / b.r);

        }

    }
}
