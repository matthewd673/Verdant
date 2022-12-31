using Microsoft.Xna.Framework;

namespace Verdant
{
    public class Camera : Entity
    {
        float shakeStrength;
        Timer shakeCooldown;
        float offsetX;
        float offsetY;

        /// <summary>
        /// Initialize a new Camera.
        /// </summary>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public Camera(int width, int height) : base(RenderObject.None, new Vec2(), width, height) { }

        /// <summary>
        /// Initialize a new Camera.
        /// </summary>
        /// <param name="position">The initial position of the Camera.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public Camera(Vec2 position, int width, int height) : base(RenderObject.None, position, width, height) { }

        /// <summary>
        /// Update the Camera, performing any animated effects (such as screen shake).
        /// </summary>
        public override void Update()
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

        /// <summary>
        /// Position the Camera so that the given world point is in the center of the view.
        /// </summary>
        /// <param name="point">The point to center on.</param>
        public void CenterOnPoint(Vec2 point)
        {
            Position.X = point.X - Width / Renderer.Scale / 2;
            Position.Y = point.Y - Height / Renderer.Scale / 2;
        }

        /// <summary>
        /// Given a position on the screen, return the corresponding position in the world.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 ScreenToWorldPos(int x, int y)
        {
            return new Vec2((x + Position.X) / Renderer.Scale, (y + Position.Y) / Renderer.Scale);
        }
        /// <summary>
        /// Given a position on the screen, return the corresponding position in the world.
        /// </summary>
        /// <param name="screenPos">The position on the screen.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 ScreenToWorldPos(Vec2Int screenPos)
        {
            return ScreenToWorldPos(screenPos.X, screenPos.Y);
        }

        /// <summary>
        /// Given a position in the world, return the corresponding position on the screen. 
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 WorldToScreenPos(float x, float y)
        {
            return new Vec2((x - Position.X) * Renderer.Scale, (y - Position.Y) * Renderer.Scale);
        }
        /// <summary>
        /// Given a position in the world, return the corresponding position on the screen. 
        /// </summary>
        /// <param name="worldPos">The position in the world.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 WorldToScreenPos(Vec2 worldPos)
        {
            return WorldToScreenPos(worldPos.X, worldPos.Y);
        }

        /// <summary>
        /// Given rectangular dimensions in world-space, return the rectangle's on-screen bounds.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given coordinates.</returns>
        public Rectangle GetRenderBounds(float x, float y, int w, int h)
        {
            return new Rectangle((int)((x - GetCameraRenderPos().X) * Renderer.Scale), (int)((y - GetCameraRenderPos().Y) * Renderer.Scale), w * Renderer.Scale, h * Renderer.Scale);
        }

        /// <summary>
        /// Given rectangular dimensions, calculate the Rectangle bounds to render at through the Camera.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given coordinates.</returns>
        public Rectangle GetRenderBounds(Vec2 pos, int w, int h)
        {
            return GetRenderBounds(pos.X, pos.Y, w, h);
        }

        /// <summary>
        /// Given an Entity, calculate the Rectangle bounds to render at through the Camera.
        /// </summary>
        /// <param name="e">The Entity.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given Entity. The Entity's rotation will not be considered.</returns>
        public Rectangle GetRenderBounds(Entity e)
        {
            return GetRenderBounds(e.Position.X, e.Position.Y, e.Width, e.Height);
        }

        /// <summary>
        /// Given a TransformState, calculate the Rectangle bounds to render at through the Camera. The TransformState's rotation will not be considered.
        /// </summary>
        /// <param name="transformState">The TransformState.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given TransformState.</returns>
        public Rectangle GetRenderBounds(TransformState transformState)
        {
            return GetRenderBounds(transformState.X, transformState.Y, (int)transformState.Width, (int)transformState.Height);
        }

        /// <summary>
        /// Calculate the screen position that the given coordinates should be rendered at. Unlike WorldToScreenPos(), this method accounts for Camera effects such as screen shake.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2Int representing the screen render position.</returns>
        public Vec2Int GetRenderPos(float x, float y)
        {
            return new Vec2Int((int)((x - GetCameraRenderPos().X) * Renderer.Scale), (int)((y - GetCameraRenderPos().Y) * Renderer.Scale));
        }

        /// <summary>
        /// Calculate the screen position that the given Vec2 should be rendered at. Unlike WorldToScreenPos(), this method accounts for Camera effects such as screen shake.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>A Vec2Int representing the screen render position.</returns>
        public Vec2Int GetRenderPos(Vec2 pos)
        {
            return GetRenderPos(pos.X, pos.Y);
        }

        /// <summary>
        /// Given an Entity, calculate the screen position that it should be rendered at through the Camera.
        /// </summary>
        /// <param name="e">The Entity.</param>
        /// <returns>A Vec2Int representing the rendering position for the given Entity.</returns>
        public Vec2Int GetRenderPos(Entity e)
        {
            return GetRenderPos(e.Position.X, e.Position.Y);
        }

        /// <summary>
        /// Add a shake effect to the Camera.
        /// </summary>
        /// <param name="strength">The strength of the shake.</param>
        /// <param name="duration">The duration of the shake (number of frames).</param>
        public void SetShake(float strength, int duration)
        {
            shakeStrength = strength;
            shakeCooldown = new Timer(duration);
            offsetX = 0;
            offsetY = 0;
        }

        /// <summary>
        /// Check if the Camera is currently shaking.
        /// </summary>
        /// <returns>Returns true if the Camera is currently shaking. Otherwise, return false.</returns>
        public bool IsShaking()
        {
            return (shakeCooldown != null);
        }

        /// <summary>
        /// Apply the current shake of the Camera.
        /// </summary>
        void ApplyShake()
        {
            offsetX = shakeStrength * GameMath.RandomFloat();
            offsetY = shakeStrength * GameMath.RandomFloat();
        }

        /// <summary>
        /// Get the position of the Camera with any offsets (shake effect, etc.) applied.
        /// </summary>
        /// <returns>A Vec2 representing the render position of the Camera.</returns>
        public Vec2 GetCameraRenderPos()
        {
            return new Vec2(Position.X + offsetX, Position.Y + offsetY);
        }

    }
}
