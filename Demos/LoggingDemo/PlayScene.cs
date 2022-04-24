using Verdant;
using System;

namespace LoggingDemo
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        public override void Initialize()
        {
            base.Initialize();

            Player player = new Player();
            EntityManager.AddEntity(player);
        }

    }
}
