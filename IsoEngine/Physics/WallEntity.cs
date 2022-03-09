using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine.Physics
{
    public class WallEntity : Entity
    {

        private float _bodyX1;
        private float _bodyY1;
        private float _bodyX2;
        private float _bodyY2;

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
