using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant
{
    public delegate float TimingFunction(float percentage);

    public static class AnimationTimingFunctions
    {

        public static TimingFunction Linear { get; private set; }
            = (float percentage) =>
            {
                return percentage;
            };

    }
}
