using System;
using Verdant;
using Verdant.Networking;
using Microsoft.Xna.Framework.Input;

namespace NetworkDemo
{
    internal class Player : NetworkEntity
    {

        public Player(Vec2 pos) : base(Sprites.Player, pos, 8, 8) { }

        public override void Update()
        {
            base.Update();

            if (InputHandler.KeyboardState.IsKeyDown(Keys.W))
                Position.Y--;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.A))
                Position.X--;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.S))
                Position.Y++;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.D))
                Position.X++;
        }

    }
}
