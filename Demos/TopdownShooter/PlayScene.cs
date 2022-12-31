using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Verdant;
using Verdant.Debugging;
using Verdant.Physics;

namespace TopdownShooter
{
    internal class PlayScene : Scene
    {

        public PlayScene(int index) : base(index) { }

        public Player Player { get; private set; }
        public Pathfinder Pathfinder { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    Ground g = new Ground(new Vec2(i * 32, j * 32));
                    EntityManager.AddEntity(g);

                    int rng = GameMath.Random.Next(100);
                    if (rng <= 5)
                        EntityManager.AddEntity(new Wall(new Vec2(i * 32, j * 32)));
                    else if (rng == 6)
                        EntityManager.AddEntity(new Enemy(new Vec2(i * 32, j * 32)));
                }
            }

            Player = new Player(new Vec2());
            EntityManager.AddEntity(Player);

            EntityManager.ApplyQueues();

            Pathfinder = new Pathfinder(16, 16, 1000);
            Pathfinder.BuildPathMap<Wall>(EntityManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
