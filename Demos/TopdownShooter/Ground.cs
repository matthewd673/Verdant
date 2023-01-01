using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;

namespace TopdownShooter
{
    internal class Ground : Entity
    {

        public Ground(Vec2 position) : base(Resources.Ground, position)
        {
            ZIndexMode = EntityManager.ZIndexMode.Manual;
            ZIndex = -999;
        }

    }
}
