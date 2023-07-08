using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant.UI
{
    /// <summary>
    /// Represents dimensions on all four sides of a UIElement in the UI box model.
    /// </summary>
    public struct BoxDimensions
    {

        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        /// <summary>
        /// Initialize a new BoxDimensions.
        /// </summary>
        /// <param name="all">The value of every dimension.</param>
        public BoxDimensions(float all)
        {
            Top = all;
            Bottom = all;
            Left = all;
            Right = all;
        }

        /// <summary>
        /// Initialize a new BoxDimensions.
        /// </summary>
        /// <param name="vertical">The value of the top and bottom dimensions.</param>
        /// <param name="horizontal">The value of the left and right dimensions.</param>
        public BoxDimensions(float vertical, float horizontal)
        {
            Top = vertical;
            Bottom = vertical;
            Left = horizontal;
            Right = horizontal;
        }

        /// <summary>
        /// Initialize a new BoxDimensions.
        /// </summary>
        /// <param name="top">The value of the top dimension.</param>
        /// <param name="bottom">The value of the bottom dimension.</param>
        /// <param name="left">The value of the left dimension.</param>
        /// <param name="right">The value of the right dimension.</param>
        public BoxDimensions(float top, float bottom, float left, float right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

    }
}
