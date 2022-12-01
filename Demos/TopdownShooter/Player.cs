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
            ZIndex = 999;
            AngleFriction = 1f;
            Speed = 2f;
            Friction = 0.25f;
        }

        public override void Update()
        {
            base.Update();

            SimpleInput();

            // TODO: why is this broken? Renderer.Camera.CenterOnEntity(this);
            Renderer.Camera.Position.X = Position.X - (Renderer.Camera.Width / 2);
            Renderer.Camera.Position.Y = Position.Y - (Renderer.Camera.Height / 2);
            //Renderer.Camera.CenterOnEntity(this);

            SimpleStats.UpdateField("cam", $"x={Renderer.Camera.Position.X} y={Renderer.Camera.Position.Y}");
        }

    }
}
