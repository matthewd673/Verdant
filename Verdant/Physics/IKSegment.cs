using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant.Physics
{
    public class IKSegment : Entity
    {
        public Vec2 Head { get; private set; }
        public Vec2 Tail { get; private set; } = new Vec2(0, 0);

        public float Angle { get; set; }
        public float Length { get; set; }

        public IKSegment(Vec2 pos, float angle, float length) : base(null, pos)
        {
            Head = pos;
            Angle = angle;
            Length = length;
        }

        public IKSegment(IKSegment parent, float angle, float length) : base(null, parent.Tail)
        {
            Head = parent.Tail.Copy();
            Angle = angle;
            Length = length;
        }

        private void CalculateTail()
        {
            float dX = Length * (float)Math.Cos(Angle);
            float dY = Length * (float)Math.Sin(Angle);
            Tail.X = Head.X + dX;
            Tail.Y = Head.Y + dY;
        }

        public void Follow(Vec2 target)
        {
            Vec2 dir = target - Head;
            Angle = dir.DirectionOpposite();

            dir = dir.Unit() * Length * -1;
            Head = target + dir;
        }

        public void Update()
        {
            CalculateTail();
        }
    }
}
