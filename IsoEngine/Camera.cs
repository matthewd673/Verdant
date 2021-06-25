using System;

namespace IsoEngine
{
    public class Camera
    {

        public Vec2 pos;
        int w;
        int h;

        public Camera(int w, int h)
        {
            this.pos = new Vec2(0, 0);
            this.w = w;
            this.h = h;
        }

        public Camera(Vec2 pos, int w, int h)
        {
            this.pos = pos;
            this.w = w;
            this.h = h;
        }

        public void CenterOnEntity(Entity e)
        {
            pos.x = (e.pos.x + e.w / 2) - w / Renderer.scale / 2;
            pos.y = (e.pos.y + e.h / 2) - h / Renderer.scale / 2;
        }

        public Vec2 ScreenToWorldPos(float x, float y)
        {
            return new Vec2((x - pos.x) / Renderer.scale, (y - pos.y) / Renderer.scale);
        }

        public Vec2 ScreenToWorldPos(Vec2 screenPos)
        {
            return ScreenToWorldPos(screenPos.x, screenPos.y);
        }

        public Vec2 MouseToWorldPos()
        {
            return ScreenToWorldPos(InputHandler.mX, InputHandler.mY);
        }

    }
}
