using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonoMinion.Graphics.Sprites
{
    /// <summary>
    /// Generic sprite class that supports animation
    /// </summary>
    public class Sprite : ICloneable
    {
        #region Variables
        public int Width;
        public int Height;

        protected SpriteSheet spriteSheet;
        public SpriteSheet SpriteSheet { get { return spriteSheet; } }

        protected bool isStatic;
        public bool IsStatic { get { return isStatic; } }

        protected Dictionary<string, SpriteAnimation> animations;
        protected SpriteAnimation currentAnimation;

        public bool IsVisible;
        public Vector2 position;
        public virtual Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Origin;
        public float Rotation;
        public float Scale;
        public float Depth;

        public SpriteEffects SpriteEffect;

        public Color Tint;
        #endregion

        public bool IsPlaying
        {
            get
            {
                if (currentAnimation != null)
                    return currentAnimation.IsPlaying;

                return false;
            }
        }

        public Sprite(int w, int h, SpriteSheet sh, bool isStatic)
        {
            Width = w;
            Height = h;
            spriteSheet = sh;
            this.isStatic = isStatic;
            IsVisible = true;

            Position = Vector2.Zero;
            Origin = Vector2.Zero;
            Rotation = 0f;
            Scale = 1f;
            Depth = 0.5f;
            Tint = Color.White;

            animations = new Dictionary<string, SpriteAnimation>();
            currentAnimation = null;

            SpriteEffect = SpriteEffects.None;
        }

        #region Animation Creation Helpers
        /// <summary>
        /// Adds an animation to a sprite
        /// </summary>
        /// <param name="name">Name of the animation</param>
        /// <param name="animation">The prepared animation object</param>
        public void AddAnimation(string name, SpriteAnimation animation)
        {
            if (!animations.ContainsKey(name))
            {
                // Add Event Handlers before adding to list
                animation.AnimationEnded += new SpriteAnimation.SpriteAnimationEventHandler(AnimationEnded);
                animations.Add(name, animation);
            }
        }

        /// <summary>
        /// Sets the sprite to prepare a specific animation for running
        /// </summary>
        /// <param name="name">Name of the animation sequence</param>
        public void SetAnimation(string name)
        {
            if (!IsStatic && animations.ContainsKey(name))
                currentAnimation = animations[name];
        }

        public SpriteAnimation GetAnimation(string name)
        {
            if (animations.ContainsKey(name))
                return animations[name];

            return null;
        }
        #endregion

        #region Animation Controls
        /// <summary>
        /// Plays an animation
        /// </summary>
        /// <param name="animation">Optional name of an animation you want to substitute out for the current running one</param>
        public void Play(string animation = null)
        {
            if (isStatic)
                return;

            if (!String.IsNullOrEmpty(animation) && animations.ContainsKey(animation))
            {
                currentAnimation = animations[animation];
                currentAnimation.Restart();
            }

            if (currentAnimation != null)
                currentAnimation.Play();
        }

        /// <summary>
        /// Pauses an animation mid frame
        /// </summary>
        public void Pause()
        {
            if (isStatic)
                return;

            if (currentAnimation != null)
                currentAnimation.Pause();
        }

        /// <summary>
        /// Stops an animation and resets it to the first frame
        /// </summary>
        public void Stop()
        {
            if (isStatic)
                return;

            if (currentAnimation != null)
                currentAnimation.Stop();
        }
        #endregion

        #region Update & Draw
        public virtual void Update(GameTime gameTime)
        {
            if (!isStatic && currentAnimation != null)
                currentAnimation.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (IsVisible && ((currentAnimation != null && currentAnimation.CurrentFrame != Rectangle.Empty) || isStatic))
            {
                if (isStatic)
                {
                    Minion.Instance.SpriteBatch.Draw(
                        spriteSheet.Texture,
                        Position,
                        null,
                        Tint,
                        Rotation,
                        Origin,
                        Scale,
                        SpriteEffect,
                        Depth
                    );
                }
                else
                {
                    Minion.Instance.SpriteBatch.Draw(
                        spriteSheet.Texture,
                        Position,
                        currentAnimation.CurrentFrame,
                        Tint,
                        Rotation,
                        Origin,
                        Scale,
                        SpriteEffect,
                        Depth
                    );
                }
            }
        }
        #endregion


        #region Event Handlers
        /// <summary>
        /// Animation ends event handler for the SpriteAnimation class
        /// </summary>
        /// <param name="animation">SpriteAnimation that triggered the event</param>
        /// <param name="e">Event arguments</param>
        public void AnimationEnded(SpriteAnimation animation, EventArgs e)
        {
            // See if we need to switch animations
            if (!String.IsNullOrEmpty(animation.NextAnimation) && animations.ContainsKey(animation.NextAnimation))
            {
                currentAnimation = animations[animation.NextAnimation];
                currentAnimation.Stop();
            }
        }
        #endregion


        /// <summary>
        /// Clones the sprite object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Sprite sprite = new Sprite(Width, Height, spriteSheet, isStatic);
            sprite.IsVisible = IsVisible;
            sprite.Position = Position;
            sprite.Origin = Origin;
            sprite.Rotation = Rotation;
            sprite.Scale = Scale;
            sprite.Depth = Depth;
            sprite.Tint = Tint;

            foreach (KeyValuePair<string, SpriteAnimation> anim in animations)
                sprite.animations.Add(anim.Key, (SpriteAnimation)anim.Value.Clone());

            if (currentAnimation != null)
                sprite.currentAnimation = sprite.animations[currentAnimation.Name];

            return sprite;
        }
    }
}
