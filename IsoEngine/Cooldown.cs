using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoEngine
{
    public class Cooldown
    {

        int time = 0;
        int duration = 0;

        public Cooldown(int duration)
        {
            this.duration = duration;
        }

        public void Tick()
        {
            time += 1;
        }

        public bool Check()
        {
            return (time >= duration);
        }

        public bool Consume()
        {
            bool ready = false;
            if (time >= duration)
            {
                ready = true;
                time = 0;
            }
            return ready;
        }

        public void Reset()
        {
            time = 0;
        }

        public int GetDuration()
        {
            return duration;
        }

    }
}
