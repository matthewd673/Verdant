using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Verdant.Physics
{
    /// <summary>
    /// An Entity with a Capsule body.
    /// </summary>
    public class CapsuleEntity : Entity
    {

        /// <summary>
        /// Initialize a new CapsuleEntity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="position">The position of the center of the Entity.</param>
        /// <param name="radius">The radius of the Capsule (half the width of the Rectangle component).</param>
        /// <param name="height">The height of the Rectangle component.</param>
        /// <param name="mass">The mass of the Entity's Body. 0 = infinite mass.</param>
        public CapsuleEntity(RenderObject sprite, Vec2 position, int radius, int height, float mass)
            : base(sprite, position, radius * 2, height)
        {
            InitializeBody(position.X, position.Y, radius, height, mass);
        }

        protected void InitializeBody(float bodyX, float bodyY, float bodyR, float bodyH, float bodyM)
        {
            float x1 = bodyX;
            float y1 = bodyY;
            float x2 = x1;
            float y2 = y1 + bodyH;

            Circle circle1 =  new Circle(x1, y1, bodyR);
            Circle circle2 = new Circle(x2, y2, bodyR);
            Vec2 recVec1 = circle2.Position + (circle2.Position - circle1.Position).Unit().Normal() * bodyR;
            Vec2 recVec2 = circle1.Position + (circle2.Position - circle1.Position).Unit().Normal() * bodyR;

            Rectangle rectangle1 = new Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * bodyR);
            rectangle1.CalculateVertices();

            Components = new Shape[] { rectangle1, circle1, circle2 };
            Mass = bodyM;

            Inertia = Mass * (
                (float) Math.Pow(2 * rectangle1.Width, 2) + 
                (float) Math.Pow(bodyH + 2 * rectangle1.Width, 2)
                ) / 12;
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
            float rad = ((Circle)Components[1]).Radius;

            spriteBatch.Draw(
                Sprite.Draw(),
                Renderer.Camera.GetRenderBounds(
                    Position.X,
                    Position.Y,
                    Width,
                    (int)(Height + 2*rad)
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
