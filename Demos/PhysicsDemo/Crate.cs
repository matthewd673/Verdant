using System;
using Verdant;
using Verdant.Debugging;
using Verdant.Physics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhysicsDemo
{
    public class Crate : BoxEntity
    {

        public Crate(Vec2 pos, int w, int h, int m) : base(Sprites.Crate, pos, w, h, m)
        {
            Speed = 0.2f;
            Friction = 0.05f;
            AngleFriction = 0.05f;
        }

        public override void Update()
        {
            base.Update();

            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.W)) up = true;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.S)) down = true;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.A)) left = true;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.D)) right = true;

            if (up)
                Acceleration = ((Rectangle)Components[0]).Dir * -Speed;
            if (down)
                Acceleration = ((Rectangle)Components[0]).Dir * Speed;
            if (!up && !down)
            {
                Acceleration.X = 0;
                Acceleration.Y = 0;
            }

            if (left) AngleSpeed -= 0.003f;
            if (right) AngleSpeed += 0.003f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (((Rectangle) Components[0]).Angle == 0f)
            //Rotation = ((Rectangle)Components[0]).Angle;
            //base.Draw(spriteBatch);

            //TODO: ((Rectangle)Components[0]).Draw(spriteBatch);
        }

    }
}
