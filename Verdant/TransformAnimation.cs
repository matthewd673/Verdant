using System;
using Microsoft.Xna.Framework;

namespace Verdant
{
    public class TransformAnimation
    {
        public TransformState From { get; private set; }
        public TransformState To { get; private set; }

        // True if either the starting or ending TransformState are multiplicative.
        public bool Multiply { get; private set; }
        private Timer animationTimer;
        private TimingFunction timingFunction;

        public bool Running { get; private set; }
        public bool Complete { get; private set; }

        public bool FillForwards { get; set; } = true;

        private TransformState diff;
        private TransformState current;

        /// <summary>
        /// Initialize a new TransformAnimation.
        /// </summary>
        /// <param name="from">The beginning TransformState.</param>
        /// <param name="to">The ending TransformState.</param>
        /// <param name="duration">The duration of the animation (ms).</param>
        public TransformAnimation(TransformState from, TransformState to, float duration)
        {
            From = from;
            To = to;
            Multiply = from.Multiply || to.Multiply; // you can't mix them
            animationTimer = new Timer(duration, AnimationTimerCallback);
            timingFunction = AnimationTimingFunctions.Linear;
            current = new(Multiply);
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
            Multiply = from.Multiply || to.Multiply; // you can't mix them
            animationTimer = new Timer(duration, AnimationTimerCallback);
            this.timingFunction = timingFunction;
            current = new(Multiply);
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
            Multiply = from.Multiply || to.Multiply;
            animationTimer = new Timer(duration, (Timer) => { Running = false; Complete = true; });
            this.timingFunction = timingFunction;
            current = new(Multiply);
            this.diff = diff;
        }

        public void Start()
        {
            animationTimer.Reset();
            animationTimer.Start();
            Running = true;
            Complete = false;
        }

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

        public TransformAnimation Copy()
        {
            return new TransformAnimation(From, To, animationTimer.Duration, timingFunction, diff);
        }
    }
}
