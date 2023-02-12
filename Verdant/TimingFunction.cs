using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant
{
    public delegate float TimingFunction(float percentage);

    /// <summary>
    /// A collection of TimingFunctions to be used with TransformAnimations.
    /// </summary>
    public static class AnimationTimingFunctions
    {

        /// <summary>
        /// y = x
        /// </summary>
        public static TimingFunction Linear { get; private set; }
            = (float percentage) =>
            {
                return percentage;
            };

    }
}
