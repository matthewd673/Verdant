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
        private static int maxHistory = 120;
        private static int avgCounter = 0;
        private static int smoothing = 3;

        private static int scale = 2;

        private static float runningUpdate = 0;
        private static float runningRender = 0;
        private static float runningUi = 0;

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
            if (avgCounter < smoothing)
            {
                runningUpdate += scene.EntityManager.UpdateDuration;
                runningRender += Renderer.RenderDuration;
                runningUi += scene.UIManager.UpdateDuration;
            }
            else
            {
                runningUpdate /= smoothing;
                runningRender /= smoothing;
                runningUi /= smoothing;

                // update time
                history.Add(new Metrics(
                    runningUpdate,
                    runningRender,
                    runningUi
                    )
                    );
                if (history.Count > maxHistory)
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

                stackHeight += DrawOnStack(spriteBatch, m.UpdateDuration, Color.Blue, position, stackHeight, i);
                stackHeight += DrawOnStack(spriteBatch, m.RenderDuration, Color.Red, position, stackHeight, i);

                i++;

                // frame took longer than 16ms
                if (m.Total > 16.67)
                    totalOver += 1f;
            }

            // draw 16ms reference line
            spriteBatch.Draw(Renderer.Pixel,
                new Rectangle(
                    position.X - maxHistory - 4,
                    position.Y - 16 * scale,
                    maxHistory + 8,
                    1),
                Color.Black
                );
        }

        private static int DrawOnStack(SpriteBatch spriteBatch, float value, Color color, Vec2Int position, int stackHeight, int histIndex)
        {
            int height = (int)(value * scale);
            spriteBatch.Draw(Renderer.Pixel,
                             new Rectangle(
                                 position.X - (maxHistory - histIndex),
                                 position.Y - height - stackHeight,
                                 1,
                                 height),
                             color);
            return height;
        }

        private struct Metrics
        {
            public float UpdateDuration { get; private set; }
            public float RenderDuration { get; private set; }
            public float UIDuration { get; private set; }

            public float Total { get; private set; }

            public Metrics(float updateDuration, float renderDuration, float uiDuration)
            {
                UpdateDuration = updateDuration;
                RenderDuration = renderDuration;
                UIDuration = uiDuration;

                Total = updateDuration +
                        renderDuration +
                        uiDuration;
            }
        }

    }
}
