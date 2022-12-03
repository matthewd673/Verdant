using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

using Verdant;
using Verdant.Debugging;
using Verdant.Physics;

namespace TopdownShooter
{
    internal class Player : BallEntity
    {

        public Player(Vec2 position) : base(Resources.Player, position, 16, 1)
        {
            AngleFriction = 1f;
            Speed = 2f;
            Friction = 0.25f;
        }

        public override void Update()
        {
            base.Update();

            SimpleInput();

            // TODO: why is this broken? Renderer.Camera.CenterOnEntity(this);
            //Renderer.Camera.CenterOnEntity(this);

            Manager.Scene.Camera.CenterOnPoint(Position);

            SimpleStats.UpdateField("pos", Position);
            SimpleStats.UpdateField("key", Key);
            SimpleStats.UpdateField("zindex", ZIndex);
        }

    }
}
