using System;
using Verdant;
using Verdant.Networking;
using Verdant.Debugging;

namespace NetworkDemo
{
    internal class PlayScene : NetworkScene
    {

        public PlayScene(int id, bool host) : base(id, host) { }

        public override void Initialize()
        {
            base.Initialize();

            NetworkManager.AddNetworkEntity<Player>(new Player(new Vec2(50, 50)));
        }

    }
}
