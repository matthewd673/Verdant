using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;

namespace TopdownShooter
{
    internal class PlayScene : Scene
    {

        public PlayScene(int index) : base(index) { }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    Ground g = new Ground(new Vec2(i * 32, j * 32));
                    EntityManager.AddEntity(g);
                }
            }

            Player player = new Player(new Vec2());
            EntityManager.AddEntity(player);
        }

    }
}
