using System;
using Verdant;
using Verdant.Debugging;
using Microsoft.Xna.Framework.Input;

namespace LoggingDemo
{
    public class Player : Entity
    {

        public Player() : base(Sprites.player, new Vec2(100, 100), 11, 10)
        {
            Log.WriteLine("Player created");
        }

        public override void Update()
        {
            bool moved = false;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.W))
            {
                Position.Y -= 2;
                moved = true;
            }
            if (InputHandler.KeyboardState.IsKeyDown(Keys.S))
            {
                Position.Y += 2;
                moved = true;
            }
            if (InputHandler.KeyboardState.IsKeyDown(Keys.A))
            {
                Position.X -= 2;
                moved = true;
            }
            if (InputHandler.KeyboardState.IsKeyDown(Keys.D))
            {
                Position.X += 2;
                moved = true;
            }

            if (moved)
                Log.WriteLine("X: " + Position.X.ToString() + " Y: " + Position.Y.ToString());

            base.Update();
        }

    }
}
