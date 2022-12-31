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

        public Player(Vec2 position) : base(Resources.Player, position, 16, 900)
        {
            AngleFriction = 1f;
            Speed = 2f;
            Friction = 0.25f;
        }

        public override void Update()
        {
            base.Update();

            SimpleInput();

            // TODO: why is this broken?
            //Renderer.Camera.CenterOnEntity(this);

            Manager.Scene.Camera.CenterOnPoint(Position);

            if (InputHandler.IsMouseFirstPressed(InputHandler.MouseButton.Left))
            {
                Vec2 mouseWorldPos = Manager.Scene.Camera.ScreenToWorldPos(InputHandler.MousePosition);
                
                SimpleStats.UpdateField("mouse pos", InputHandler.MousePosition);
                SimpleStats.UpdateField("mouse world pos", mouseWorldPos);
                
                float shootAngle = GameMath.AngleBetweenPoints(Position, mouseWorldPos);
                Projectile p = new Projectile(Position.Copy(), shootAngle);
                Manager.AddEntity(p);
            }

            SimpleStats.UpdateField("pos", Position);
            SimpleStats.UpdateField("key", Key);
            SimpleStats.UpdateField("zindex", ZIndex);
            SimpleStats.UpdateField("player col", GetColliding().Count);
        }

    }
}
