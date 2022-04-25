using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Verdant.Physics
{
    /// <summary>
    /// An Entity with a Ball body.
    /// </summary>
    public class BallEntity : Entity
    {

        private float _bodyX = 0f;
        private float _bodyY = 0f;
        private float _bodyR = 0f;
        private float _bodyMass = 0f;

        /// <summary>
        /// Initialize a new BallEntity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="position">The position of the center of the Entity.</param>
        /// <param name="radius">The radius of the Entity's Ball. Also used to determine rendering width and height.</param>
        /// <param name="mass">The mass of the Entity's Body. 0 = infinite mass.</param>
        public BallEntity(RenderObject sprite, Vec2 position, float radius, float mass)
            : base(sprite, position, (int)(radius * 2), (int)(radius * 2))
        {
            _bodyX = position.X;
            _bodyY = position.Y;
            _bodyR = radius;
            _bodyMass = mass;

            InitializeBody();
        }

        protected override void InitializeBody()
        {
            Components = new Shape[] { new Circle(_bodyX, _bodyY, _bodyR) };
            Position = new Vec2(_bodyX, _bodyY);
            Mass = _bodyMass;
        }

        /// <summary>
        /// Move according to WASD input. A basic template.
        /// </summary>
        public void SimpleInput()
        {
            bool up = InputHandler.KeyboardState.IsKeyDown(Keys.W);
            bool down = InputHandler.KeyboardState.IsKeyDown(Keys.S);
            bool left = InputHandler.KeyboardState.IsKeyDown(Keys.A);
            bool right = InputHandler.KeyboardState.IsKeyDown(Keys.D);

            if (left) Acceleration.X = -Speed;
            if (up) Acceleration.Y = -Speed;
            if (right) Acceleration.X = Speed;
            if (down) Acceleration.Y = Speed;
            if (!left && !right) Acceleration.X = 0;
            if (!up && !down) Acceleration.Y = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite circleTexture = Renderer.GenerateCircleSprite(_bodyR, Color.Red);

            spriteBatch.Draw(
                circleTexture.Draw(),
                Renderer.Camera.GetRenderBounds(Position - new Vec2(_bodyR, _bodyR), circleTexture.Width, circleTexture.Height),
                Color.White
                );
        }

    }
}
