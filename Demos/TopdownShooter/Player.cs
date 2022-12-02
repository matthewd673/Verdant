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
            SetZIndexToBase = false;
            AngleFriction = 1f;
            Speed = 2f;
            Friction = 0.25f;
        }

        public override void Update()
        {
            base.Update();

            SimpleInput();

            // TODO: why is this broken? Renderer.Camera.CenterOnEntity(this);
            Manager.Scene.Camera.Position.X = Position.X - (Manager.Scene.Camera.Width / 2);
            Manager.Scene.Camera.Position.Y = Position.Y - (Manager.Scene.Camera.Height / 2);
            //Renderer.Camera.CenterOnEntity(this);

            SimpleStats.UpdateField("cam", $"x={Manager.Scene.Camera.Position.X} y={Manager.Scene.Camera.Position.Y}");
        }

    }
}
