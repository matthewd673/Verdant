using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Debugging
{
    /// <summary>
    /// Visualize the computation time of each frame.
    /// </summary>
    public static class PerformanceMonitor
    {
        private static List<Metrics> history = new List<Metrics>();

        public static int MaxHistory = 120;
        public static int Smoothing { get; set; } = 3;
        public static int Scale { get; set; } = 2;
        public static float Ceiling { get; set; } = 16.67f;

        private static int avgCounter = 0;

        private static float runningUpdate = 0;
        private static float runningPhysics = 0;
        private static float runningRender = 0;
        private static float runningUi = 0;

        public static Color UpdateColor { get; set; } = Color.Blue;
        public static Color PhysicsColor { get; set; } = Color.Green;
        public static Color UIColor { get; set; } = Color.LightBlue;
        public static Color RenderColor { get; set; } = Color.Red;

        public static Color CeilingColor { get; set; } = Color.Black;

        // Determines if PerformanceMonitor should be rendered. Data will still be collected while it is hidden.
        public static bool Show { get; set; } = true;

        /// <summary>
        /// Draw the PerformanceMonitor.
        /// </summary>
        /// <param name="scene">The current Scene.</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public static void Draw(Scene scene, SpriteBatch spriteBatch)
        {

            Vec2Int position = new Vec2Int(Renderer.ScreenWidth - 32, Renderer.ScreenHeight - 32);

            avgCounter++;
            if (avgCounter < Smoothing)
            {
                runningUpdate += scene.EntityManager.UpdateDuration;
                runningPhysics += scene.EntityManager.PhysicsDuration;
                runningRender += Renderer.RenderDuration;
                runningUi += scene.UIManager.UpdateDuration;
            }
            else
            {
                runningUpdate /= Smoothing;
                runningPhysics /= Smoothing;
                runningRender /= Smoothing;
                runningUi /= Smoothing;

                // update time
                history.Add(new Metrics(
                    runningUpdate,
                    runningPhysics,
                    runningRender,
                    runningUi
                    )
                    );
                if (history.Count > MaxHistory)
                {
                    history.RemoveAt(0);
                }

                avgCounter = 0;
            }

            // don't render when hidden
            if (!Show) return;

            // render all metrics in history
            int i = 0;
            float totalOver = 0f;
            foreach (Metrics m in history)
            {
                int stackHeight = 0;

                stackHeight += DrawOnStack(spriteBatch, m.UpdateDuration - m.PhysicsDuration, UpdateColor, position, stackHeight, i);
                stackHeight += DrawOnStack(spriteBatch, m.PhysicsDuration, PhysicsColor, position, stackHeight, i);
                stackHeight += DrawOnStack(spriteBatch, m.UIDuration, UIColor, position, stackHeight, i);
                stackHeight += DrawOnStack(spriteBatch, m.RenderDuration, RenderColor, position, stackHeight, i);

                i++;

                // frame took longer than 16ms
                if (m.Total > Ceiling)
                    totalOver += 1f;
            }

            // draw 16ms reference line
            spriteBatch.Draw(Renderer.Pixel,
                new Rectangle(
                    position.X - MaxHistory - 4,
                    position.Y - (int)(Ceiling * Scale),
                    MaxHistory + 8,
                    1),
                CeilingColor
                );
        }

        private static int DrawOnStack(SpriteBatch spriteBatch, float value, Color color, Vec2Int position, int stackHeight, int histIndex)
        {
            int height = (int)(value * Scale);
            spriteBatch.Draw(Renderer.Pixel,
                             new Rectangle(
                                 position.X - (MaxHistory - histIndex),
                                 position.Y - height - stackHeight,
                                 1,
                                 height),
                             color);
            return height;
        }

        private struct Metrics
        {
            public float UpdateDuration { get; private set; }
            public float PhysicsDuration { get; private set; }
            public float RenderDuration { get; private set; }
            public float UIDuration { get; private set; }

            public float Total { get; private set; }

            public Metrics(float updateDuration, float physicsDuration, float renderDuration, float uiDuration)
            {
                UpdateDuration = updateDuration;
                PhysicsDuration = physicsDuration;
                RenderDuration = renderDuration;
                UIDuration = uiDuration;

                Total = updateDuration + // physics is within update
                        renderDuration +
                        uiDuration;
            }
        }

    }
}
