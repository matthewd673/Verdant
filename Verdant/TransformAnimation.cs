using System;
using Microsoft.Xna.Framework;

namespace Verdant
{
    /// <summary>
    /// Smoothly animates between two TransformStates, according to a TimingFunction.
    /// </summary>
    public class TransformAnimation
    {
        // The initial TransformState (it will be filled backwards even if the animation hasn't started).
        public TransformState From { get; private set; }
        // The final TransformState.
        public TransformState To { get; private set; }

        // Should be the same for both TransformStates, but will be set to the "from" state's blend mode.
        public TransformStateBlendMode BlendMode { get; private set; }
        private Timer animationTimer;
        private TimingFunction timingFunction;

        // Indicates if the TransformAnimation is currently running.
        public bool Running { get; private set; }
        // Indicates if the TransformAnimation is complete.
        public bool Complete { get; private set; }

        // Determines if the final TransformState should fill forwards when the animation is complete.
        public bool FillForwards { get; set; } = true;

        private TransformState diff;
        private TransformState current;

        /// <summary>
        /// Initialize a new two-state TransformAnimation with a simple beginning and end.
        /// </summary>
        /// <param name="from">The beginning TransformState.</param>
        /// <param name="to">The ending TransformState.</param>
        /// <param name="duration">The duration of the animation (ms).</param>
        public TransformAnimation(TransformState from, TransformState to, float duration)
        {
            From = from;
            To = to;
            BlendMode = From.BlendMode; // you can't mix them
            animationTimer = new Timer(duration, AnimationTimerCallback);
            timingFunction = AnimationTimingFunctions.Linear;
            current = new(BlendMode);
            diff = to - from;
        }

        /// <summary>
        /// Initialize a new TransformAnimation.
        /// </summary>
        /// <param name="from">The beginning TransformState.</param>
        /// <param name="to">The ending TransformState.</param>
        /// <param name="duration">The duration of the animation (ms).</param>
        /// <param name="timingFunction">The timing function of the animation.</param>
        public TransformAnimation(TransformState from, TransformState to, float duration, TimingFunction timingFunction)
        {
            From = from;
            To = to;
            BlendMode = From.BlendMode; // you can't mix them
            animationTimer = new Timer(duration, AnimationTimerCallback);
            this.timingFunction = timingFunction;
            current = new(BlendMode);
            diff = to - from;
        }

        private void AnimationTimerCallback(Timer t)
        {
            Running = false;
            Complete = true;
        }

        private TransformAnimation(TransformState from, TransformState to, float duration, TimingFunction timingFunction, TransformState diff)
        {
            From = from;
            To = to;
            BlendMode = From.BlendMode;
            animationTimer = new Timer(duration, (Timer) => { Running = false; Complete = true; });
            this.timingFunction = timingFunction;
            current = new(BlendMode);
            this.diff = diff;
        }

        /// <summary>
        /// Start the TransformAnimation.
        /// </summary>
        public void Start()
        {
            animationTimer.Start();
            Running = true;
            Complete = false;
        }

        /// <summary>
        /// Stop the TransformAnimation.
        /// </summary>
        public void Stop()
        {
            animationTimer.Stop();
            Running = false;
            Complete = false;
        }

        /// <summary>
        /// Reset the TransformAnimation. If it is running it will be stopped.
        /// </summary>
        public void Reset()
        {
            animationTimer.Reset();
            Running = false;
        }

        /// <summary>
        /// Reset and start the TransformAnimation.
        /// </summary>
        public void Restart()
        {
            Reset();
            Start();
        }

        /// <summary>
        /// Get the current state of the TransformAnimation.
        /// </summary>
        /// <returns>A TransformState with properties representing the current state of the TransformAnimation.</returns>
        public TransformState GetFrame()
        {
            if (FillForwards && Complete)
                return To;

            float percentage = animationTimer.ElapsedTime / animationTimer.Duration;
            float timingPercentage = timingFunction.Invoke(percentage);

            current.Position.X = From.Position.X + (diff.Position.X * timingPercentage);
            current.Position.Y = From.Position.Y + (diff.Position.Y * timingPercentage);
            current.Width = From.Width + (diff.Width * timingPercentage);
            current.Height = From.Height + (diff.Height * timingPercentage);
            current.Angle = From.Angle + (diff.Angle * timingPercentage);

            return current;
        }

        /// <summary>
        /// Create a new TransformAnimation with the same properties as this one.
        /// </summary>
        /// <returns>A new TransformAnimation.</returns>
        public TransformAnimation Copy()
        {
            return new TransformAnimation(From, To, animationTimer.Duration, timingFunction, diff);
        }
    }
}
