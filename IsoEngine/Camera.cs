using System;

namespace IsoEngine
{
    public class Camera
    {

        public Vec2 pos;
        int w;
        int h;

        float shakeStrength;
        Cooldown shakeCooldown;
        float offsetX;
        float offsetY;

        public Camera(int w, int h)
        {
            pos = new Vec2();
            this.w = w;
            this.h = h;
        }

        public Camera(Vec2 pos, int w, int h)
        {
            this.pos = pos;
            this.w = w;
            this.h = h;
        }

        public void Update()
        {
            if (shakeCooldown == null)
                return;

            shakeCooldown.Tick();
            if (!shakeCooldown.Check())
                ApplyShake();
            else
            {
                offsetX = 0;
                offsetY = 0;
                shakeCooldown = null;
            }
        }

        public void CenterOnEntity(Entity e)
        {
            pos.X = (e.Position.X + e.Width / 2) - w / Renderer.scale / 2;
            pos.Y = (e.Position.Y + e.Height / 2) - h / Renderer.scale / 2;
        }

        public Vec2 ScreenToWorldPos(float x, float y)
        {
            return new Vec2((x - pos.X) / Renderer.scale, (y - pos.Y) / Renderer.scale);
        }
        public Vec2 ScreenToWorldPos(Vec2 screenPos)
        {
            return ScreenToWorldPos(screenPos.X, screenPos.Y);
        }

        public Vec2 MouseToWorldPos()
        {
            return ScreenToWorldPos(InputHandler.mX, InputHandler.mY);
        }

        public Vec2 WorldToScreenPos(float x, float y)
        {
            return new Vec2((x - pos.X) * Renderer.scale, (y - pos.Y) * Renderer.scale);
        }
        public Vec2 WorldToScreenPos(Vec2 worldPos)
        {
            return WorldToScreenPos(worldPos.X, worldPos.Y);
        }

        public void SetShake(float strength, int duration)
        {
            shakeStrength = strength;
            shakeCooldown = new Cooldown(duration);
            offsetX = 0;
            offsetY = 0;
        }

        public bool IsShaking()
        {
            return (shakeCooldown != null);
        }

        void ApplyShake()
        {
            offsetX = shakeStrength * Math.RandomFloat();
            offsetY = shakeStrength * Math.RandomFloat();
        }

        public Vec2 GetRenderPos()
        {
            return new Vec2(pos.X + offsetX, pos.Y + offsetY);
        }

    }
}
