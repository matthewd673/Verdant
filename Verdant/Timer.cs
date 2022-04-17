using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant
{
    public class Timer
    {

        public int Time { get; private set; } = 0;
        public int Duration { get; private set; } = 0;

        /// <summary>
        /// Initialize a new Timer.
        /// </summary>
        /// <param name="duration">The duration of the Timer (number of frames).</param>
        public Timer(int duration, int startingTime = 0)
        {
            Duration = duration;

            Time = startingTime;
            if (Time > duration)
                Time = duration;
        }

        /// <summary>
        /// Increase the recorded time of the Timer (mark the passage of a frame).
        /// </summary>
        public void Tick()
        {
            Time += 1;
        }

        /// <summary>
        /// Check if the Timer has reached its duration. 
        /// </summary>
        /// <returns>Returns true if the current time is greater than or equal to the duration.</returns>
        public bool Check()
        {
            return (Time >= Duration);
        }

        /// <summary>
        /// If the Timer has reached its duration, reset the time and return true.
        /// </summary>
        /// <returns>Returnes true if the Timer has reached its duration. Otherwise, returns false.</returns>
        public bool Consume()
        {
            bool ready = false;
            if (Time >= Duration)
            {
                ready = true;
                Time = 0;
            }
            return ready;
        }

        /// <summary>
        /// Reset the Timer's current time.
        /// </summary>
        public void Reset()
        {
            Time = 0;
        }

    }
}
