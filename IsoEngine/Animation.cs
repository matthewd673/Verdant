using System;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class Animation
    {

        SpriteSheet sheet;
        FrameSet frameSet;

        Cooldown animateCooldown;
        bool looping;

        int frameIndex;

        bool settled = false;
        bool hasLooped = false;

        /// <summary>
        /// Initialize a new Animation.
        /// </summary>
        /// <param name="sheet">The SpriteSheet of frames.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(SpriteSheet sheet, int frameDelay, bool looping = true)
        {
            this.sheet = sheet;
            frameSet = new FrameSet(0, sheet.GetSheetWidth(), row: 0);
            animateCooldown = new Cooldown(frameDelay);
            this.looping = looping;
        }
        /// <summary>
        /// Initialize a new Animation.
        /// </summary>
        /// <param name="sheet">The SpriteSheet of frames.</param>
        /// <param name="frameSet">The subset of frames in the SpriteSheet to animate through.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(SpriteSheet sheet, FrameSet frameSet, int frameDelay, bool looping = true)
        {
            this.sheet = sheet;
            this.frameSet = frameSet;
            animateCooldown = new Cooldown(frameDelay);
            this.looping = looping;
        }
        /// <summary>
        /// Initialize a new Animation directly from a Texture2D sheet.
        /// </summary>
        /// <param name="sheetTexture">The sheet texture to crop frames from. Frames must be in a single row.</param>
        /// <param name="spriteW">The width of each sprite in the sheet.</param>
        /// <param name="graphicsDevice">The GraphicsDevice to use when cropping the sheet.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(Texture2D sheetTexture, int spriteW, GraphicsDevice graphicsDevice, int frameDelay, bool looping = true)
        {
            sheet = new SpriteSheet(SpriteSheet.BuildTexture2DArray(sheetTexture, spriteW, sheetTexture.Height, graphicsDevice));
            frameSet = new FrameSet(0, sheet.GetSheetWidth(), row: 0);
            animateCooldown = new Cooldown(frameDelay);
            this.looping = looping;
        }
        /// <summary>
        /// Initialize a new Animation directly from a Texture2D sheet.
        /// </summary>
        /// <param name="sheetTexture">The sheet texture to crop frames from. Frames must be in a single row.</param>
        /// <param name="spriteW">The width of each sprite in the sheet.</param>
        /// <param name="graphicsDevice">The GraphicsDevice to use when cropping the sheet.</param>
        /// <param name="frameSet">The subset of frames in the sheet to animate through.</param>
        /// <param name="frameDelay">The number of ticks between each frame.</param>
        /// <param name="looping">Determines if the Animation loops.</param>
        public Animation(Texture2D sheetTexture, int spriteW, GraphicsDevice graphicsDevice, FrameSet frameSet, int frameDelay, bool looping = true)
        {
            sheet = new SpriteSheet(SpriteSheet.BuildTexture2DArray(sheetTexture, spriteW, sheetTexture.Height, graphicsDevice));
            this.frameSet = frameSet;
            animateCooldown = new Cooldown(frameDelay);
            this.looping = looping;
        }

        /// <summary>
        /// Perform the next step of the Animation and return the current frame.
        /// </summary>
        /// <returns>The current Texture2D frame in the Animation sequence.</returns>
        public Texture2D Animate()
        {

            if (settled) //skip other steps if settled
                return sheet.Get(frameIndex, frameSet.row);

            animateCooldown.Tick();

            if (animateCooldown.Consume())
            {
                frameIndex++;

                if (frameIndex >= frameSet.endFrame)
                {
                    if (looping)
                    {
                        frameIndex = frameSet.startFrame;
                        hasLooped = true;
                    }
                    else
                    {
                        frameIndex = frameSet.endFrame - 1;
                        settled = true;
                    }
                }

            }

            return sheet.Get(frameIndex, frameSet.row);
        }

        /// <summary>
        /// Get the current frame of the Animation (without animating further).
        /// </summary>
        /// <returns>The current Texture2D frame in the Animation sequence.</returns>
        public Texture2D GetCurrentFrame()
        {
            return sheet.Get(frameIndex, frameSet.row);
        }

        /// <summary>
        /// Reset the Animation to the first frame. The Animation will also be marked incomplete.
        /// </summary>
        public void Reset()
        {
            frameIndex = frameSet.startFrame;
            settled = false;
            hasLooped = false;
        }

        /// <summary>
        /// Check if the Animation has been played all the way through. If the Animation loops, it will be marked as complete after the first play.
        /// </summary>
        /// <returns>Returns true if the Animation is complete.</returns>
        public bool IsComplete()
        {
            if (!looping)
                return settled;
            else
                return hasLooped;
        }

        /// <summary>
        /// Get the number of frames in the Animation.
        /// </summary>
        /// <returns>The number of frames in the Animation.</returns>
        public int GetLength()
        {
            return frameSet.endFrame - frameSet.startFrame;
        }

        /// <summary>
        /// Get the index of the current frame of Animation, relative to the FrameSet's start frame.
        /// </summary>
        /// <returns>The index of the current frame.</returns>
        public int GetCurrentFrameIndex()
        {
            return frameIndex - frameSet.startFrame;
        }

        /// <summary>
        /// Manually set the index of the current frame of Animation, relative to the FrameSet's start frame.
        /// </summary>
        /// <param name="index">The new frame index.</param>
        public void SetCurrentFrameIndex(int index)
        {
            frameIndex = index + frameSet.startFrame;
        }

        /// <summary>
        /// Get the sprite at the specified index, relative to the FrameSet's start frame.
        /// </summary>
        /// <param name="index">The frame index to pull from.</param>
        /// <returns>The sprite at the specified index.</returns>
        public Texture2D Get(int index)
        {
            return sheet.Get(index + frameSet.startFrame, frameSet.row);
        }

        /// <summary>
        /// Copy the settings of the current animation to a new instance.
        /// </summary>
        /// <returns>A new Animation instance.</returns>
        public Animation Copy()
        {
            return new Animation(sheet, frameSet, animateCooldown.GetDuration(), looping);
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
