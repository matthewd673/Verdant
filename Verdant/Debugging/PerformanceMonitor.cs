using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Debugging
{
    public static class PerformanceMonitor
    {

        private static List<Metrics> history = new List<Metrics>();
        private static int maxHistory = 120;

        public static void Draw(Scene scene, SpriteBatch spriteBatch)
        {
            Vec2Int position = new Vec2Int(Renderer.ScreenWidth - 32, Renderer.ScreenHeight - 32);

            // update time
            history.Add(new Metrics(
                scene.EntityManager.UpdateDuration,
                Renderer.RenderDuration
                )
                );
            if (history.Count > maxHistory)
            {
                history.RemoveAt(0);
            }

            // render all metrics in history
            int i = 0;
            foreach (Metrics m in history)
            {
                int stackHeight = 0;

                stackHeight += DrawOnStack(spriteBatch, m.UpdateDuration, Color.Blue, position, stackHeight, i);
                stackHeight += DrawOnStack(spriteBatch, m.RenderDuration, Color.Red, position, stackHeight, i);

                i++;
            }

            // draw 16ms reference line
            spriteBatch.Draw(Renderer.Pixel,
                new Rectangle(
                    position.X - maxHistory - 4,
                    position.Y - 16,
                    maxHistory + 8,
                    1),
                Color.Black
                );

        }

        private static int DrawOnStack(SpriteBatch spriteBatch, float value, Color color, Vec2Int position, int stackHeight, int histIndex)
        {
            int height = (int)(value * 2);
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

            public Metrics(float updateDuration, float renderDuration)
            {
                UpdateDuration = updateDuration;
                RenderDuration = renderDuration;
            }
        }

    }
}
