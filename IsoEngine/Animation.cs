using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class Animation : SpriteSheet
    {

        int frameIndex;
        int frameCount;

        Cooldown animateCooldown;
        bool looping;

        bool settled = false;

        /// <summary>
        /// Initialize a new Animation.
        /// </summary>
        /// <param name="sheet">The Texture2D sheet of frames. Frames should be in one row, ordered left to right.</param>
        /// <param name="frameCount">The number of frames contained in the sheet.</param>
        /// <param name="graphicsDevice">The GraphicsDevice to use when cropping frames.</param>
        /// <param name="animationDelay">The number of Update calls before the next frame is displayed.</param>
        /// <param name="looping">Determines if the Animation loops continuously or plays once.</param>
        public Animation(Texture2D sheet, int frameCount, GraphicsDevice graphicsDevice, int animationDelay, bool looping = true) : base(sheet, frameCount, graphicsDevice)
        {
            this.frameCount = frameCount;
            animateCooldown = new Cooldown(animationDelay);
            this.looping = looping;
        }
        /// <summary>
        /// Initialize a new Animation from an existing frame array.
        /// </summary>
        /// <param name="sprites">The array of frames to use.</param>
        /// <param name="animationDelay">The number of Update calls before the next frame is displayed.</param>
        /// <param name="looping">Determines if the Animation loops continuously or plays once.</param>
        public Animation(Texture2D[,] sprites, int animationDelay, bool looping) : base(sprites)
        {
            frameCount = sprites.Length;
            animateCooldown = new Cooldown(animationDelay);
            this.looping = looping;
        }

        /// <summary>
        /// Iterate through the Animation sequence and return the current frame.
        /// </summary>
        /// <returns>The current Texture2D frame in the Animation sequence.</returns>
        public Texture2D Animate()
        {

            if (settled) //skip other steps if settled
                return Get(frameIndex);

            animateCooldown.Tick();

            if (animateCooldown.Consume())
            {
                frameIndex++;

                if (frameIndex >= frameCount)
                {
                    if (looping)
                        frameIndex = 0;
                    else
                    {
                        frameIndex = frameCount - 1;
                        settled = true;
                    }
                }

            }

            return Get(frameIndex);
        }

        /// <summary>
        /// Copy the settings of the current animation to a new instance.
        /// </summary>
        /// <returns>A new Animation instance.</returns>
        public Animation Copy()
        {
            return new Animation(sprites, animateCooldown.GetDuration(), looping);
        }

    }
}
