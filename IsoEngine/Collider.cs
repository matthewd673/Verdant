using System;
using Microsoft.Xna.Framework;

namespace IsoEngine
{
    public class Collider
    {

        Entity parent;

        public Vec2 pos = new Vec2();
        float offsetX;
        float offsetY;
        public int w;
        public int h;
        public bool relativePos;

        public bool trigger;
        
        public Collider(Entity parent, Vec2 pos, int w, int h, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = pos.x;
            offsetY = pos.y;
            if (relativePos)
                ApplyOffset();

            this.w = w;
            this.h = h;
            this.relativePos = relativePos;
            this.trigger = trigger;
        }
        public Collider(Entity parent, float x, float y, int w, int h, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = x;
            offsetY = y;
            if (relativePos)
                ApplyOffset();

            this.w = w;
            this.h = h;
            this.relativePos = relativePos;
            this.trigger = trigger;
        }
        public Collider(Entity parent, bool trigger = false, bool relativePos = true)
        {
            this.parent = parent;

            offsetX = 0;
            offsetY = 0;
            if (relativePos)
                ApplyOffset();

            w = parent.w;
            h = parent.h;
            this.relativePos = relativePos;
            this.trigger = trigger;
        }

        public void Update()
        {
            if (!relativePos)
                return;
            ApplyOffset();
        }

        void ApplyOffset()
        {
            pos.x = offsetX + parent.pos.x;
            pos.y = offsetY + parent.pos.y;
        }

    }
}
