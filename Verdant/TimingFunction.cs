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
        /// Linear timing function.
        /// Equation: y = x
        /// </summary>
        public static TimingFunction Linear { get; private set; }
            = (float x) =>
            {
                return x;
            };

        /// <summary>
        /// Triangle timing function.
        /// Equation: y = -ABS(0.5 - (2x - 0.5)) + 1
        /// </summary>
        public static TimingFunction Triangle { get; private set; }
            = (float x) =>
            {
                return -(float)Math.Abs(.5 - (2 * x - .5)) + 1;
            };

        /// <summary>
        /// Inverted triangle timing function.
        /// Equation: y = ABS(0.5 - (2x - 0.5))
        /// </summary>
        public static TimingFunction InvertedTriangle { get; private set; }
            = (float x) =>
            {
                return (float)Math.Abs(.5 - (2 * x - .5));
            };

    }
}
