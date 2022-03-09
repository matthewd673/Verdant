﻿using System;
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

        public CapsuleEntity(RenderObject sprite, Vec2 position, int radius, int height, float mass)
            : base(sprite, position, radius * 2, height)
        {
            _bodyX = position.X;
            _bodyY = position.Y;
            _bodyR = radius;
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

            Circle circle1 =  new Circle(x1, y1, _bodyR);
            Circle circle2 = new Circle(x2, y2, _bodyR);
            Vec2 recVec1 = circle2.Position + (circle2.Position - circle1.Position).Unit().Normal() * _bodyR;
            Vec2 recVec2 = circle1.Position + (circle2.Position - circle1.Position).Unit().Normal() * _bodyR;

            Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * _bodyR);
            rectangle1.CalculateVertices();

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

            Vec2 dir = Components[0].Dir;
            if (up) Acceleration = dir * -Speed;
            if (down) Acceleration = dir * Speed;
            if (!up && !down) Acceleration = new Vec2(0, 0);

            if (left) AngleSpeed -= 0.003f;
            if (right) AngleSpeed += 0.003f;
        }

        public override void Move()
        {
            base.Move();

            AngleSpeed *= 1 - AngleFriction;
            ((Rectangle)Components[0]).Angle += AngleSpeed;
            ((Rectangle)Components[0]).CalculateVertices();

            float length = ((Rectangle)Components[0]).Length;
            Components[1].Position = Components[0].Position + Components[0].Dir * -length / 2;
            Components[2].Position = Components[0].Position + Components[0].Dir * length / 2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vec2 origin = new Vec2(Sprite.Width / 2, Sprite.Height / 2);
            spriteBatch.Draw(
                Sprite.Get(),
                Renderer.Camera.GetRenderBounds(
                    Position.X,
                    Position.Y,
                    Width,
                    (int)(Height + 2*_bodyR)
                    ),
                null,
                Color.White,
                ((Rectangle)Components[0]).Angle,
                (Vector2)origin,
                SpriteEffects.None,
                0
                );
        }
    }
}
