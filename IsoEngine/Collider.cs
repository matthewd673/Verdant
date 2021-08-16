using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Collider
    {

        Entity parent;

        public Vec2 Position { get; set; } = new Vec2();
        float offsetX;
        float offsetY;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool RelativePosition { get; set; }
        public bool Trigger { get; set; }
        
        /// <summary>
        /// Initialize a new Collider.
        /// </summary>
        /// <param name="parent">The parent Entity of the Collider.</param>
        /// <param name="pos">The position.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="trigger">Determines if the Collider will be marked as a trigger.</param>
        /// <param name="relativePos">Determines if the Collider's position will be calculated relative to the parent's position (and updated accordingly).</param>
        public Collider(Entity parent, Vec2 pos, int w, int h, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = pos.X;
            offsetY = pos.Y;
            if (relativePos)
                ApplyOffset();

            Width = w;
            Height = h;
            RelativePosition = relativePos;
            Trigger = trigger;
        }
        /// <summary>
        /// Initialize a new Collider.
        /// </summary>
        /// <param name="parent">The parent Entity of the Collider.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="trigger">Determines if the Collider will be marked as a trigger.</param>
        /// <param name="relativePos">Determines if the Collider's position will be calculated relative to the parent's position (and updated accordingly).</param>
        public Collider(Entity parent, float x, float y, int w, int h, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = x;
            offsetY = y;
            if (relativePos)
                ApplyOffset();

            Width = w;
            Height = h;
            RelativePosition = relativePos;
            Trigger = trigger;
        }
        /// <summary>
        /// Initialize a new Collider using the bounds of the given Entity.
        /// </summary>
        /// <param name="parent">The parent Entity of the Collider.</param>
        /// <param name="trigger">Determines if the Collider will be marked as a trigger.</param>
        /// <param name="relativePos">Determines if the Collider's position will be calculated relative to the parent's position (and updated accordingly).</param>
        public Collider(Entity parent, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = 0;
            offsetY = 0;
            if (relativePos)
                ApplyOffset();

            Width = parent.Width;
            Height = parent.Height;
            RelativePosition = relativePos;
            Trigger = trigger;
        }

        /// <summary>
        /// Update the Collider (apply relative position).
        /// </summary>
        public void Update()
        {
            if (!RelativePosition)
                return;
            ApplyOffset();
        }

        /// <summary>
        /// Update the Collider's position according to the parent's position and the offset values.
        /// </summary>
        void ApplyOffset()
        {
            Position.X = offsetX + parent.Position.X;
            Position.Y = offsetY + parent.Position.Y;
        }

    }
}
