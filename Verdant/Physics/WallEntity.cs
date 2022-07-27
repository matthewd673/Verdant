using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{
    /// <summary>
    /// An Entity with a Wall body. Doesn't render or move by default.
    /// </summary>
    public class WallEntity : Entity
    {
        /// <summary>
        /// Initialize a new WallEntity.
        /// </summary>
        /// <param name="start">The start position of the Wall.</param>
        /// <param name="end">The end position of the Wall.</param>
        public WallEntity(Vec2 start, Vec2 end)
            : base(null, new Vec2(0, 0), 0, 0)
        {
            InitializeBody(start.X, start.Y, end.X, end.Y);
        }

        protected void InitializeBody(float bodyX1, float bodyY1, float bodyX2, float bodyY2)
        {
            Components = new Shape[] { new Line(bodyX1, bodyY1, bodyX2, bodyY2) };
            Position = new Vec2(
                (bodyX1 + bodyX2) / 2f,
                (bodyY1 + bodyY2) / 2f
                );
        }

    }
}
