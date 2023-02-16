using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdant.Physics
{
    /// <summary>
    /// Simple implementation of inverse kinematics.
    /// </summary>
    public class IKSegment : Entity
    {
        // The head of the segment (point that moves/follows).
        public Vec2 Head { get; private set; }
        // The tail of the segment (which moves according to the head).
        public Vec2 Tail { get; private set; } = new Vec2(0, 0);

        // The angle of the IKSegment.
        public float Angle { get; set; }
        // The length of the IKSegment.
        public float Length { get; set; }

        /// <summary>
        /// Initialize a new IKSegment.
        /// </summary>
        /// <param name="position">The position of the IKSegment's head.</param>
        /// <param name="length">The length of the IKSegment</param>
        /// <param name="angle">The initial angle of the IKSegment.</param>
        public IKSegment(Vec2 position, float length, float angle = 0f) : base(null, position)
        {
            Head = position;
            Length = length;
            Angle = angle;
        }

        /// <summary>
        /// Initialize a new IKSegment.
        /// </summary>
        /// <param name="parent">The IKSegment to follow (the new IKSegment's head will be the parent's tail).</param>
        /// <param name="length">The length of the IKSegment.</param>
        /// <param name="angle">The initial angle of the IKSegment.</param>
        public IKSegment(IKSegment parent, float length, float angle = 0f) : base(null, parent.Tail)
        {
            Head = parent.Tail.Copy();
            Length = length;
            Angle = angle;
        }

        private void CalculateTail()
        {
            float dX = Length * (float)Math.Cos(Angle);
            float dY = Length * (float)Math.Sin(Angle);
            Tail.X = Head.X + dX;
            Tail.Y = Head.Y + dY;
        }

        /// <summary>
        /// Point and move towards a target point.
        /// </summary>
        /// <param name="target">The point to follow.</param>
        public void Follow(Vec2 target)
        {
            Vec2 dir = target - Head;
            Angle = dir.DirectionOpposite();

            dir = dir.Unit() * Length * -1;
            Head = target + dir;
        }

        public override void Update()
        {
            CalculateTail();
        }
    }
}
