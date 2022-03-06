using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.Physics
{
    public class Rectangle : Shape
    {

        Vec2 refDir;
        internal float Length { get; set; }
        internal float Width { get; set; }
        public float Angle { get; set; }
        Matrix rotMat;

        public Rectangle(float x1, float y1, float x2, float y2, float w, float h)
        {
            Vertices = new Vec2[4];
            Vertices[0] = new Vec2(x1, y1);
            Vertices[1] = new Vec2(x2, y2);

            Dir = (Vertices[1] - Vertices[0]).Unit();
            refDir = Dir.Copy();
            //Length = (Vertices[1] - Vertices[0]).Magnitude();
            Length = h;

            Width = w;

            Vertices[2] = Vertices[1] + (Dir.Normal() * Width);
            Vertices[3] = Vertices[2] + (Dir.Normal() * -Length);

            Position = Vertices[0] + (Dir * Length / 2) + (Dir.Normal() * Width / 2);
            Angle = 0;
            rotMat = new Matrix(2, 2);
        }

        public void CalculateVertices()
        {
            rotMat = PhysicsMath.CalculateRotMatrix(Angle);
            Dir = (rotMat * refDir).Unit();

            Debugging.Log.WriteLine(refDir + "\t" + Dir);

            Vertices[0] = Position + (Dir * -Length / 2) + (Dir.Normal() * Width / 2);
            Vertices[1] = Position + (Dir * -Length / 2) + (Dir.Normal() * -Width / 2);
            Vertices[2] = Position + (Dir * Length / 2) + (Dir.Normal() * -Width / 2);
            Vertices[3] = Position + (Dir * Length / 2) + (Dir.Normal() * Width / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Renderer.DrawLine(spriteBatch, Vertices[0], Vertices[1], Color.Yellow);
            Renderer.DrawLine(spriteBatch, Vertices[1], Vertices[2], Color.Yellow);
            Renderer.DrawLine(spriteBatch, Vertices[2], Vertices[3], Color.Yellow);
            Renderer.DrawLine(spriteBatch, Vertices[3], Vertices[0], Color.Yellow);
        }

    }
}
