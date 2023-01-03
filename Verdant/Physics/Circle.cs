using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{
    internal class Circle : Shape
    {

        internal float Radius { get; set; }
        private Sprite bodySprite;

        /// <summary>
        /// Initialize a new Circle shape.
        /// </summary>
        /// <param name="x">The X component of the center of the Circle.</param>
        /// <param name="y">The Y component of the center of the Circle.</param>
        /// <param name="radius">The radius of the Circle.</param>
        public Circle(float x, float y, float radius)
        {
            Vertices = new Vec2[2];
            Position = new Vec2(x, y);
            Radius = radius;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, Color color)
        {
            // only generate body sprite once, and only if Draw is actually called
            if (bodySprite == RenderObject.None)
            {
                bodySprite = Renderer.GenerateCircleSprite((int)(Radius * Renderer.Scale), Color.White).Draw();
            }

            spriteBatch.Draw(
                bodySprite.Draw(),
                camera.GetRenderBounds(
                    Position.X - Radius,
                    Position.Y - Radius,
                    (int)(Radius * 2),
                    (int)(Radius * 2)),
                color
                );
        }
    }
}
