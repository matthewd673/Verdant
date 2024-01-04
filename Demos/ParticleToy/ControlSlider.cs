using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;
using Verdant.UI;

namespace ParticleToy
{
    internal class ControlSlider : UISlider
    {
        public ControlSlider(Vec2 position, float minValue, float maxValue, int barWidth)
            : base(position, minValue, maxValue, Resources.SliderIndicator, Resources.SliderBar, barWidth)
        {
            BoxModel.Margin = new BoxDimensions(4, Resources.SliderIndicator.Width / 2);
        }
    }
}
