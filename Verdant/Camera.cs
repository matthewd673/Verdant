using Microsoft.Xna.Framework;
using System;

namespace Verdant
{
    /// <summary>
    /// Used by the Renderer to render Entities in world space to the screen.
    /// </summary>
    public class Camera : Entity
    {
        private float shakeStrength;
        private Timer shakeTimer;
        private float offsetX;
        private float offsetY;

        public bool Shaking { get; private set; }

        public Vec2 RenderPosition { get; private set; } = new();

        /// <summary>
        /// Initialize a new Camera.
        /// </summary>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public Camera(int width, int height)
            : base(RenderObject.None, new Vec2(), width, height)
        {
            RenderPosition.X = Position.X;
            RenderPosition.Y = Position.Y;
        }

        /// <summary>
        /// Initialize a new Camera.
        /// </summary>
        /// <param name="position">The initial position of the Camera.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public Camera(Vec2 position, int width, int height)
            : base(RenderObject.None, position, width, height)
        {
            RenderPosition.X = Position.X;
            RenderPosition.Y = Position.Y;
        }

        /// <summary>
        /// Update the Camera, performing any animated effects (such as screen shake).
        /// </summary>
        public override void Update()
        {
            // apply camera shake
            if (shakeTimer == null || !shakeTimer.Running)
            {
                return;
            }
            ApplyShake();

            // update render position
            RenderPosition.X = Position.X + offsetX;
            RenderPosition.Y = Position.Y + offsetY;
        }

        /// <summary>
        /// Position the Camera so that the given world point is in the center of the view.
        /// </summary>
        /// <param name="point">The point to center on.</param>
        public void CenterOnPoint(Vec2 point)
        {
            Position.X = point.X - Width / Renderer.WorldScale / 2;
            Position.Y = point.Y - Height / Renderer.WorldScale / 2;
        }

        /// <summary>
        /// Given a position on the screen, return the corresponding position in the world.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 ScreenToWorldPosition(int x, int y)
        {
            return new((x / Renderer.WorldScale) + Position.X,
                       (y / Renderer.WorldScale) + Position.Y
                       );
        }
        /// <summary>
        /// Given a position on the screen, return the corresponding position in the world.
        /// </summary>
        /// <param name="screenPos">The position on the screen.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 ScreenToWorldPosition(Vec2Int screenPos)
        {
            return ScreenToWorldPosition(screenPos.X, screenPos.Y);
        }

        /// <summary>
        /// Given a position in the world, return the corresponding position on the screen.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 WorldToScreenPosition(float x, float y)
        {
            // TODO: doesn't take TransformState into account (is this a feature, not a bug?)
            return new((x - Position.X) * Renderer.WorldScale,
                       (y - Position.Y) * Renderer.WorldScale
                       );
        }
        /// <summary>
        /// Given a position in the world, return the corresponding position on the screen.
        /// </summary>
        /// <param name="worldPos">The position in the world.</param>
        /// <returns>A Vec2 representing the position in the world.</returns>
        public Vec2 WorldToScreenPosition(Vec2 worldPos)
        {
            return WorldToScreenPosition(worldPos.X, worldPos.Y);
        }

        /// <summary>
        /// Given rectangular dimensions in world-space, return the rectangle's on-screen bounds.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given coordinates.</returns>
        public Rectangle GetRenderBounds(float x, float y, float width, float height)
        {
            return new((int)((x - RenderPosition.X) * Renderer.WorldScale),
                       (int)((y - RenderPosition.Y) * Renderer.WorldScale),
                       (int)(width * Renderer.WorldScale),
                       (int)(height * Renderer.WorldScale)
                       );
        }

        /// <summary>
        /// Given rectangular dimensions, calculate the Rectangle bounds to render at through the Camera.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given coordinates.</returns>
        public Rectangle GetRenderBounds(Vec2 pos, float width, float height)
        {
            return GetRenderBounds(pos.X, pos.Y, width, height);
        }

        /// <summary>
        /// Given an Entity, calculate the Rectangle bounds to render at through the Camera.
        /// </summary>
        /// <param name="e">The Entity.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given Entity. The Entity's rotation will not be considered.</returns>
        public Rectangle GetRenderBounds(Entity e)
        {
            return GetRenderBounds(e.Position.X,
                                   e.Position.Y,
                                   e.Width,
                                   e.Height
                                   );
        }

        /// <summary>
        /// Given a TransformState, calculate the Rectangle bounds to render at through the Camera. The TransformState's rotation will not be considered.
        /// </summary>
        /// <param name="transformState">The TransformState.</param>
        /// <returns>A Rectangle representing the proper rendering bounds for the given TransformState.</returns>
        public Rectangle GetRenderBounds(TransformState transformState)
        {
            return GetRenderBounds(transformState.Position,
                                   (int)transformState.Width,
                                   (int)transformState.Height
                                   );
        }

        /// <summary>
        /// Calculate the screen position that the given coordinates should be rendered at. Unlike WorldToScreenPos(), this method accounts for Camera effects such as screen shake.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A Vec2Int representing the screen render position.</returns>
        public Vec2Int GetRenderPosition(float x, float y)
        {
            return new((int)((x - RenderPosition.X) * Renderer.WorldScale),
                       (int)((y - RenderPosition.Y) * Renderer.WorldScale)
                       );
        }

        /// <summary>
        /// Calculate the screen position that the given Vec2 should be rendered at. Unlike WorldToScreenPos(), this method accounts for Camera effects such as screen shake.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>A Vec2Int representing the screen render position.</returns>
        public Vec2Int GetRenderPosition(Vec2 pos)
        {
            return GetRenderPosition(pos.X, pos.Y);
        }

        /// <summary>
        /// Given an Entity, calculate the screen position that it should be rendered at through the Camera.
        /// </summary>
        /// <param name="e">The Entity.</param>
        /// <returns>A Vec2Int representing the rendering position for the given Entity.</returns>
        public Vec2Int GetRenderPosition(Entity e)
        {
            return GetRenderPosition(e.Position.X, e.Position.Y);
        }

        /// <summary>
        /// Blend the operand TransformState with the Camera's transform.
        /// </summary>
        /// <param name="operand">The TransformState to blend onto in-place. The Camera blend uses both additive and multiplicative blends.</param>
        public void BlendOnto(TransformState operand)
        {
            // update manually to avoid allocating new Vec2s
            operand.Position.X -= RenderPosition.X;
            operand.Position.X *= Renderer.WorldScale;
            operand.Position.Y -= RenderPosition.Y;
            operand.Position.Y *= Renderer.WorldScale;

            operand.Width *= Renderer.WorldScale;
            operand.Height *= Renderer.WorldScale;
            // NOTE: angle is not affected by the camera
        }

        /// <summary>
        /// Add a shake effect to the Camera.
        /// </summary>
        /// <param name="strength">The strength of the shake.</param>
        /// <param name="duration">The duration of the shake (number of frames).</param>
        public void SetShake(float strength, float duration)
        {
            shakeStrength = strength;

            Shaking = true;
            shakeTimer = new(duration, (t) =>
            {
                // another shake has been started, keep it going
                if (shakeTimer != t)
                {
                    return;
                }
                // clean up after shake is done
                Shaking = false;
                offsetX = 0;
                offsetY = 0;
            });
            shakeTimer.Start();

            offsetX = 0;
            offsetY = 0;
        }

        /// <summary>
        /// Apply the current shake of the Camera.
        /// </summary>
        private void ApplyShake()
        {
            offsetX = shakeStrength * GameMath.RandomFloat();
            offsetY = shakeStrength * GameMath.RandomFloat();
        }
    }
}
