using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoEngine.Physics
{
    public class BoxEntity : Entity
    {

        private float _bodyX;
        private float _bodyY;
        private float _bodyW;
        private float _bodyH;
        private float _bodyM;

        public BoxEntity(RenderObject sprite, Vec2 position, int width, int height, float mass)
            : base(sprite, position, width, height)
        {
            _bodyX = position.X;
            _bodyY = position.Y;
            _bodyW = width;
            _bodyH = height;
            _bodyM = mass;

            InitializeBody();
        }

        protected override void InitializeBody()
        {
            float x1 = _bodyX;
            float y1 = _bodyY;
            float x2 = x1;
            float y2 = y1 + _bodyH;
            float r = _bodyW / 2;

            Vec2 top = new Vec2(x1, y1);
            Vec2 bottom = new Vec2(x2, y2);

            Vec2 recVec1 = bottom + (bottom - top).Unit().Normal() * r;
            Vec2 recVec2 = top + (bottom - top).Unit().Normal() * r;

            Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * r);
            rectangle1.CalculateVertices();

            Components = new Shape[] { rectangle1 };
            Mass = _bodyM;

            Inertia = Mass * (
                (float)Math.Pow(2 * rectangle1.Width, 2) +
                (float)Math.Pow(_bodyH + 2 * rectangle1.Width, 2)
                ) / 12;
        }

        public void SimpleInput()
        {
            bool up = InputHandler.KeyboardState.IsKeyDown(Keys.W);
            bool down = InputHandler.KeyboardState.IsKeyDown(Keys.S);
            bool left = InputHandler.KeyboardState.IsKeyDown(Keys.A);
            bool right = InputHandler.KeyboardState.IsKeyDown(Keys.D);

            Vec2 dir = Components[0].Dir;
            if (up) Acceleration = dir * -Speed;
            if (down) Acceleration = dir * Speed;
            if (!up && !down) Acceleration = new Vec2(0, 0);

            if (left) AngleSpeed -= 0.3f;
            if (right) AngleSpeed += 0.3f;
        }

        public override void Move()
        {
            base.Move();

            AngleSpeed *= 1 - AngleFriction;
            ((Rectangle)Components[0]).Angle += AngleSpeed;
            ((Rectangle)Components[0]).CalculateVertices();
        }

        public override void Update()
        {

            Speed = 0.5f;
            Friction = 0.05f;
            AngleFriction = 0.1f;

            //SimpleInput();

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle1 = (Rectangle)Components[0];

            spriteBatch.Draw(Renderer.GetPixel(), new Vector2((_bodyX + _bodyW / 2), _bodyY), Color.Red);
            spriteBatch.Draw(Renderer.GetPixel(), new Vector2((_bodyX + _bodyW / 2), _bodyY + _bodyH), Color.Red);

            spriteBatch.Draw(Renderer.GetPixel(), new Vector2(rectangle1.Position.X, rectangle1.Position.Y), Color.White);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[0], rectangle1.Vertices[1], Color.Blue);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[1], rectangle1.Vertices[2], Color.Blue);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[2], rectangle1.Vertices[3], Color.Blue);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[3], rectangle1.Vertices[0], Color.Blue);
        }

    }
}
