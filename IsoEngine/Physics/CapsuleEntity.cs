using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IsoEngine.Physics
{
    public class CapsuleEntity : Entity
    {

        private float _bodyX;
        private float _bodyY;
        private float _bodyH;
        private float _bodyR;
        private float _bodyM;

        public CapsuleEntity(RenderObject sprite, Vec2 position, int r, int height, float m)
            : base(sprite, position, r * 2, height)
        {
            _bodyX = position.X;
            _bodyY = position.Y;
            _bodyR = r;
            _bodyH = height;
            _bodyM = m;

            InitializeBody();
        }

        protected override void InitializeBody()
        {
            float x1 = _bodyX;
            float y1 = _bodyY;
            float x2 = x1;
            float y2 = y1 + _bodyH;

            Debugging.Log.WriteLine("width, height");
            Debugging.Log.WriteLine(Width + ", " + Height);

            Circle circle1 =  new Circle(x1, y1, _bodyR);
            Circle circle2 = new Circle(x2, y2, _bodyR);
            Vec2 recVec1 = circle2.Position + (circle2.Position - circle1.Position).Unit().Normal() * _bodyR;
            Vec2 recVec2 = circle1.Position + (circle2.Position - circle1.Position).Unit().Normal() * _bodyR;

            Debugging.Log.WriteLine("x1,y1 x2,y2: ");
            Debugging.Log.WriteLine(x1 + "," + y1 + " " + x2 + "," + y2);
            Debugging.Log.WriteLine("circle1 & 2 pos: ");
            Debugging.Log.WriteLine(circle1.Position + " , " + circle2.Position);

            Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * _bodyR);

            Components = new Shape[] { rectangle1, circle1, circle2 };
            Mass = _bodyM;

            Inertia = Mass * (
                (float) Math.Pow(2 * rectangle1.Width, 2) + 
                (float) Math.Pow(_bodyH + 2 * rectangle1.Width, 2)
                ) / 12;
        }

        public void SimpleInput()
        {
            bool up = InputHandler.KeyboardState.IsKeyDown(Keys.W);
            bool down = InputHandler.KeyboardState.IsKeyDown(Keys.S);
            bool left = InputHandler.KeyboardState.IsKeyDown(Keys.A);
            bool right = InputHandler.KeyboardState.IsKeyDown(Keys.D);

            Vec2 dir = ((Rectangle)Components[0]).Dir;
            if (up) Acceleration = dir * -Speed;
            if (down) Acceleration = dir * Speed;
            if (!up && !down) Acceleration = new Vec2(0, 0);

            if (left) AngleSpeed -= 0.003f;
            if (right) AngleSpeed += 0.003f;
        }

        public override void Move()
        {
            Acceleration = Acceleration.Unit() * Speed;
            Velocity += Acceleration;
            Velocity *= 1 - Friction;

            Components[0].Position += Velocity;

            AngleSpeed *= 1 - AngleFriction;
            ((Rectangle)Components[0]).Angle += AngleSpeed;
            ((Rectangle)Components[0]).CalculateVertices();

            Components[1].Position = Components[0].Position + ((Rectangle)Components[0]).Dir * -((Rectangle)Components[0]).Length / 2;
            Components[2].Position = Components[0].Position + ((Rectangle)Components[0]).Dir * ((Rectangle)Components[0]).Length / 2;
        }

        public override void Update()
        {
            Speed = 0.5f;
            Friction = 0.05f;
            AngleFriction = 0.1f;

            SimpleInput();

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite circleTexture = Renderer.GenerateCircleTexture(_bodyR, Color.Green);

            Rectangle rectangle1 = (Rectangle)Components[0];
            Circle circle1 = (Circle) Components[1];
            Circle circle2 = (Circle) Components[2];


            spriteBatch.Draw(
                circleTexture.Get(),
                Renderer.Camera.GetRenderBounds(circle1.Position - new Vec2(_bodyR, _bodyR), circleTexture.Width, circleTexture.Height),
                Color.White
                );

            spriteBatch.Draw(
                circleTexture.Get(),
                Renderer.Camera.GetRenderBounds(circle2.Position - new Vec2(_bodyR, _bodyR), circleTexture.Width, circleTexture.Height),
                Color.White
                );

            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[0], rectangle1.Vertices[1], Color.Green);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[1], rectangle1.Vertices[2], Color.Green);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[2], rectangle1.Vertices[3], Color.Green);
            Renderer.DrawLine(spriteBatch, rectangle1.Vertices[3], rectangle1.Vertices[0], Color.Green);
        }

    }
}
