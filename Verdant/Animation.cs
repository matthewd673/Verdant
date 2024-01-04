using System;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// A RenderObject that automatically displays different frames of an animation when rendered.
    /// </summary>
    public class Animation : SpriteSheet
    {
        public delegate void AnimationCallback(Animation sender);

        private FrameSet frameSet;

        private int frameDelay;
        private int frameTimeCounter;

        // Determines if the Animation should loop indefinitely or only play once.
        public bool Looping { get; set; }

        private int frameIndex;

        private bool settled = false;
        private bool hasLooped = false;

        // Is true after a non-looping Animation has completed or a looping Animation has completed at least one loop.
        public bool Complete
        {
            get { return Looping ? hasLooped : settled; }
        }

        // The current frame of Animation, relative to the FrameSet's start frame.
        public int FrameIndex
        {
            get { return frameIndex - frameSet.startFrame; }
            set
            {
                frameIndex = value + frameSet.startFrame;
                settled = false; // otherwise, if it had been settled it won't animate
            }
        }

        // The number of frames in the Animation's FrameSet
        public int Count
        {
            get { return frameSet.endFrame - frameSet.startFrame; }
        }

        // The number of loops the Animation has completed.
        public int Loops { get; private set; }

        // A callback that will be called every time an Animation complete a loop.
        public AnimationCallback OnComplete { get; set; }

        /// <summary>
        /// Initialize a new Animation.
        /// </summary>
        /// <param name="sheetTexture">The Texture2D sheet of sprites. Sprites should be in one row, ordered left to right.</param>
        /// <param name="spriteWidth">The width of each sprite.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(Texture2D sheetTexture, int spriteWidth, int frameDelay, bool looping = true)
            : base(sheetTexture, spriteWidth)
        {
            frameSet = new FrameSet(0, horizontalSprites, row: 0);
            this.frameDelay = frameDelay;
            frameTimeCounter = frameDelay;
            Looping = looping;
        }

        /// <summary>
        /// Initialize a new Animation.
        /// </summary>
        /// <param name="sheetTexture">The Texture2D sheet of sprites. Sprites should be in one row, ordered left to right.</param>
        /// <param name="frameSet">The subset of frames in the SpriteSheet to animate through.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(Texture2D sheetTexture, int spriteWidth, FrameSet frameSet, int frameDelay, bool looping = true)
            : base(sheetTexture, spriteWidth)
        {
            this.frameSet = frameSet;
            this.frameDelay = frameDelay;
            frameTimeCounter = frameDelay;
            Looping = looping;
        }

        /// <summary>
        /// Reset the Animation to the first frame. The Animation will be marked incomplete, and the number of ticks until next frame will be reset.
        /// </summary>
        public void Reset()
        {
            frameIndex = frameSet.startFrame;
            settled = false;
            hasLooped = false;
        }

        private void PerformAnimationStep()
        {
            if (settled) // skip other steps if settled
            {
                return;
            }

            // tick towards next frame
            frameTimeCounter--;

            // time for the next frame
            if (frameTimeCounter <= 0)
            {
                // reset counter
                frameTimeCounter = frameDelay;
                frameIndex++;

                // loop back to beginning, if looping
                if (frameIndex >= frameSet.endFrame)
                {
                    if (Looping)
                    {
                        frameIndex = frameSet.startFrame;
                        hasLooped = true;
                    }
                    else
                    {
                        frameIndex = frameSet.endFrame - 1;
                        settled = true;
                    }
                    Loops++;
                    OnComplete?.Invoke(this);
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch, Transform transform)
        {
            PerformAnimationStep();

            DrawIndex(spriteBatch,
                      transform,
                      frameIndex,
                      frameSet.row
                      );
        }

        /// <summary>
        /// Copy the settings of the current animation to a new instance.
        /// </summary>
        /// <returns>A new Animation instance.</returns>
        public Animation Copy()
        {
            return new(texture, spriteWidth, frameSet, frameDelay, Looping)
            {
                Loops = 0,
            };
        }

        public struct FrameSet
        {
            public int startFrame;
            public int endFrame;
            public int row;

            /// <summary>
            /// Define a new FrameSet.
            /// </summary>
            /// <param name="startFrame">The index (x-coordinate) of the first frame of animation.</param>
            /// <param name="endFrame">The index (x-coordinate) of the last frame of animation.</param>
            /// <param name="row">If using a SpriteSheet with multiple rows, the row to pull frames from (y-coordinate).</param>
            public FrameSet(int startFrame, int endFrame, int row = 0)
            {
                this.startFrame = startFrame;
                this.endFrame = endFrame;
                this.row = row;
            }
        }

    }
}
