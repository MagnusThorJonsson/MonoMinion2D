using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics.Sprites
{
    /// <summary>
    /// Sprite Animation object that is coupled with the Sprite class
    /// </summary>
    public class SpriteAnimation : ICloneable
    {
        #region Variables
        public string Name;

        protected Rectangle[] frames;
        protected int frameCount;
        protected int currentFrame;
        protected float frameLength;

        public bool IsLoop;
        protected bool isPlaying;
        protected int playCount;

        public string NextAnimation;

        private float _timer;

        // Events
        /// <summary>
        /// Triggers when an animation ends it run
        /// </summary>
        public event SpriteAnimationEventHandler AnimationEnded;
        public delegate void SpriteAnimationEventHandler(SpriteAnimation animation, EventArgs e);
        #endregion

        #region Properties
        public int FrameCount { get { return frameCount; } }
        public bool IsPlaying { get { return isPlaying; } }
        public int PlayCount { get { return playCount; } }
        public Rectangle CurrentFrame { get { return frames[currentFrame]; } }
        #endregion

        /// <summary>
        /// Sprite Animation constructor
        /// </summary>
        /// <param name="name">Name of the animation</param>
        /// <param name="noOfFrames">Number of frames in the animation</param>
        /// <param name="fLength">The duration of each frame in seconds</param>
        public SpriteAnimation(string name, int noOfFrames, float fLength)
        {
            Name = name;
            frameCount = noOfFrames;
            frames = new Rectangle[frameCount];
            frameLength = fLength;

            currentFrame = 0;
            playCount = 0;
            isPlaying = false;
            IsLoop = false;

            NextAnimation = null;
            _timer = 0f;
        }

        #region Frame Helpers
        /// <summary>
        /// Adds a frame to the animation
        /// </summary>
        /// <param name="i">The frame index</param>
        /// <param name="rect">The frame rectangle</param>
        public void AddFrame(int i, Rectangle rect)
        {
            if (i < frameCount)
                frames[i] = rect;
        }

        /// <summary>
        /// Adds a frame to the animation
        /// </summary>
        /// <param name="i">The frame index</param>
        /// <param name="x">The X position of the frame on the spritesheet</param>
        /// <param name="y">The Y position of the frame on the spritesheet</param>
        /// <param name="width">The frame width</param>
        /// <param name="height">The frame height</param>
        public void AddFrame(int i, int x, int y, int width, int height)
        {
            if (i < frameCount)
            {
                frames[i] = new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        /// Removes a frame from the animation
        /// </summary>
        /// <param name="i">The index of the frame to remove</param>
        public void RemoveFrame(int i)
        {
            if (i < frameCount && frames[i] != null && frames[i] != Rectangle.Empty)
                frames[i] = Rectangle.Empty;
        }
        #endregion

        #region Animation Controls
        /// <summary>
        /// Plays the animation
        /// </summary>
        public void Play()
        {
            isPlaying = true;
            _timer = 0f;
        }

        /// <summary>
        /// Pauses the animation mid frame
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
        }

        /// <summary>
        /// Stops the animation and resets it to the first frame
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            Restart();
        }

        /// <summary>
        /// Restarts to the first frame
        /// </summary>
        public void Restart()
        {
            currentFrame = 0;
        }
        #endregion

        /// <summary>
        /// Update animation
        /// </summary>
        /// <param name="gameTime">Current GameTime object</param>
        public void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > frameLength)
                {
                    _timer = 0.0f;

                    if (!IsLoop)
                    {
                        if (currentFrame < frameCount - 1)
                        {
                            currentFrame++;
                        }
                        else
                        {
                            OnAnimationEnded(null);
                            Pause();
                        }
                    }
                    else
                        currentFrame = (currentFrame + 1) % frameCount;

                    // TODO: Move this to an event handler (loop counter)
                    // Updates the playcount
                    if (frameCount > 1 && currentFrame == 0)
                        playCount = (int)MathHelper.Min(playCount + 1, int.MaxValue);
                }
            }
        }

        /// <summary>
        /// Animation ended trigger
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAnimationEnded(EventArgs e)
        {
            if (AnimationEnded != null)
                AnimationEnded(this, e);
        }

        /// <summary>
        /// Clones the animation object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SpriteAnimation anim = new SpriteAnimation(Name, frameCount, frameLength);
            anim.IsLoop = IsLoop;
            anim.isPlaying = isPlaying;
            anim.frames = frames;
            anim.NextAnimation = NextAnimation;
            if (AnimationEnded != null)
                anim.AnimationEnded = AnimationEnded;

            return anim;
        }
    }
}
