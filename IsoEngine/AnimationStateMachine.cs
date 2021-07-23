using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class AnimationStateMachine
    {

        Dictionary<string, AnimationState> animations = new Dictionary<string, AnimationState>();
        string currentId = "";

        string defaultId = "";
        List<string> queue = new List<string>();

        /// <summary>
        /// Initialize a new AnimationStateMachine.
        /// </summary>
        public AnimationStateMachine() { }

        /// <summary>
        /// Add a new Animation to the state machine, and define custom properties for it.
        /// </summary>
        /// <param name="id">The ID of the Animation to be used when playing and queuing.</param>
        /// <param name="animation">The Animation to add.</param>
        /// <param name="interruptFrame">The earliest frame index that the Animation can be interrupted at.</param>
        /// <param name="startAction">The Action that is invoked when the Animation is played.</param>
        /// <param name="endAction">The Action that is invoked when the Animation ends.</param>
        /// <param name="indefinite">Determines if the Animation plays indefinitely (the state machine does not return to the default animation upon completion). If the Animation does not loop, the final frame will be rendered.</param>
        /// <param name="nextId">The ID of an Animation to play upon completion.</param>
        public void DefineAnimation(string id, Animation animation,
            int interruptFrame = -1, Action<string, Animation> startAction = null, Action<string, Animation> endAction = null, bool indefinite = false, string nextId = "")
        {
            animations.Add(id, new AnimationState(animation, interruptFrame, startAction, endAction, indefinite, nextId));
        }

        /// <summary>
        /// Mark an Animation within the state machine as the default, which will be played whenever an Animation ends.
        /// </summary>
        /// <param name="id">The ID of the Animation to mark default.</param>
        public void SetDefaultAnimation(string id)
        {
            defaultId = id;
            if (currentId.Equals("")) //play default if no current animation
                currentId = id;
        }

        /// <summary>
        /// Play the Animation with the given ID.
        /// </summary>
        /// <param name="id">The ID of the Animation to play.</param>
        /// <param name="queueIfUninterruptable">If the current Animation cannot be interrupted, queue the given Animation to be played after.</param>
        public void Play(string id, bool queueIfUninterruptable = true)
        {
            if (animations[currentId].animation.GetCurrentFrameIndex() >= animations[currentId].interruptFrame) //can be interrupted, so play
            {
                InvokeAnimationEnd();
                currentId = id;
                InvokeAnimationStart();
            }
            else //uninterruptable
            {
                if (queueIfUninterruptable)
                    Queue(id);
            }
        }

        /// <summary>
        /// Queue the Animation with the given ID.
        /// </summary>
        /// <param name="id">The ID of the Animation to queue.</param>
        public void Queue(string id)
        {
            queue.Add(id);
        }

        /// <summary>
        /// Clear the Animation queue.
        /// </summary>
        public void ClearQueue()
        {
            queue.Clear();
        }

        /// <summary>
        /// Perform the next step of the current Animation, and switch to the next appropriate Animation if necessary.
        /// </summary>
        /// <returns>The current Texture2D frame of the current Animation.</returns>
        public Texture2D Animate()
        {
            Texture2D frame = animations[currentId].animation.Animate();

            if (animations[currentId].animation.IsComplete()) //animation finished - move on to whats next
            {
                if (!animations[currentId].nextId.Equals("")) //if there is a specified next id, play that
                {
                    InvokeAnimationEnd();
                    currentId = animations[currentId].nextId;
                    InvokeAnimationStart();

                    return frame;
                }

                if (queue.Count > 0) //if there is a queued animation, play that
                {
                    InvokeAnimationEnd();
                    //pull next and remove from queue
                    currentId = queue[0];
                    queue.RemoveAt(0);
                    InvokeAnimationStart();

                    return frame;
                }

                if (!animations[currentId].indefinite) //if not indefinite, return to default
                {
                    //end current animation
                    InvokeAnimationEnd();
                    //play default
                    animations[defaultId].animation.Reset(); //reset, just to be safe
                    currentId = defaultId;
                    InvokeAnimationStart();

                    return frame;
                }
            }

            if (animations[currentId].animation.GetCurrentFrameIndex() >= animations[currentId].interruptFrame &&
                queue.Count > 0) //the animation can be interrupted, and there is something in queue
            {
                InvokeAnimationEnd();
                //play next in queue
                currentId = queue[0];
                queue.RemoveAt(0);
                InvokeAnimationStart();

                return frame;
            }

            return frame;

        }
        
        void InvokeAnimationStart()
        {
            if (animations[currentId].startAction != null)
                animations[currentId].startAction.Invoke(currentId, animations[currentId].animation);
        }

        void InvokeAnimationEnd()
        {
            animations[currentId].animation.Reset(); //reset each time it ends
            if (animations[currentId].endAction != null)
                animations[currentId].endAction.Invoke(currentId, animations[currentId].animation);
        }

        /// <summary>
        /// Get the Animation that is currently playing.
        /// </summary>
        /// <returns>The Animation currently playing.</returns>
        public Animation GetCurrentAnimation()
        {
            return animations[currentId].animation;
        }

        struct AnimationState
        {
            public Animation animation;
            public int interruptFrame;
            public Action<string, Animation> startAction;
            public Action<string, Animation> endAction;
            public bool indefinite;
            public string nextId;

            public AnimationState(Animation animation, int interruptFrame = -1, Action<string, Animation> startAction = null, Action<string, Animation> endAction = null, bool indefinite = false, string nextId = "")
            {
                this.animation = animation;
                this.interruptFrame = interruptFrame;
                this.startAction = startAction;
                this.endAction = endAction;
                this.indefinite = indefinite;
                this.nextId = nextId;
            }
        }

    }
}
