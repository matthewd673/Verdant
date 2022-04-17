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

        private float _bodyX1;
        private float _bodyY1;
        private float _bodyX2;
        private float _bodyY2;

        /// <summary>
        /// Initialize a new WallEntity.
        /// </summary>
        /// <param name="start">The start position of the Wall.</param>
        /// <param name="end">The end position of the Wall.</param>
        public WallEntity(Vec2 start, Vec2 end)
            : base(null, new Vec2(0, 0), 0, 0)
        {
            _bodyX1 = start.X;
            _bodyY1 = start.Y;
            _bodyX2 = end.X;
            _bodyY2 = end.Y;

            InitializeBody();
        }

        protected override void InitializeBody()
        {
            Components = new Shape[] { new Line(_bodyX1, _bodyY1, _bodyX2, _bodyY2) };
            Position = new Vec2(
                (_bodyX1 + _bodyX2) / 2f,
                (_bodyY1 + _bodyY2) / 2f
                );
        }

    }
}
