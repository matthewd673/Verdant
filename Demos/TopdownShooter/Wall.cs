using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;
using Verdant.Physics;

namespace TopdownShooter
{
    internal class Wall : BoxEntity
    {

        public Wall(Vec2 position) : base(Resources.Wall, position, 32, 32, 0)
        {
            AngleFriction = 1;
        }

    }
}
