using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;
using Verdant.UI;

namespace ParticleToy
{
    internal class ControlTextBox : UITextBox
    {

        public ControlTextBox(Vec2 position) : base(position, Resources.Font, "")
        {
            Padding = new BoxDimensions(2);
        }

    }
}
