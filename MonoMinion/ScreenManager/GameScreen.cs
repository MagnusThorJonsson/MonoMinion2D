using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoMinion
{
    /// <summary>
    /// Lightweight Game Screen class used by the GameManager
    /// </summary>
    public class GameScreen : DrawableGameComponent
    {
        #region Variables and Properties
        public bool IsInitialized { get; set; }
        public bool IsTransparent { get; set; }
        public bool IsPaused { get; set; }

        private bool _isHud;
        public bool IsHud
        {
            get { return this._isHud; }
            set
            {
                if (value)
                {
                    this.IsTransparent = true;
                    this._isHud = true;
                }
            }
        }
        #endregion

        /// <summary>
        /// GameScreen constructor
        /// </summary>
        /// <param name="game">Instance of the current Game object</param>
        public GameScreen(Game game)
            : base(game)
        {
            this.IsInitialized = false;
            this.IsTransparent = false;
        }


        #region Drawable overrides
        /// <summary>
        /// Initialize override
        /// </summary>
        public override void Initialize()
        {
            this.IsInitialized = true;
            base.Initialize();
        }


        /// <summary>
        /// Update override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion


        /// <summary>
        /// Activate GameScreen
        /// </summary>
        public virtual void Activate()
        {
            this.Visible = true;
        }


        /// <summary>
        /// Deactivate GameScreen
        /// </summary>
        public virtual void Deactivate()
        {
            this.Visible = false;
        }

        /// <summary>
        /// Sorts the Entity list by Layers
        /// </summary>
        /// <param name="list">The list to be sorted</param>
        /// <param name="comparison">The variable to sort the list by</param>
        public static void LayerSort<T>(IList<T> list, Comparison<T> comparison)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (comparison == null)
                throw new ArgumentNullException("comparison");

            int count = list.Count;
            for (int j = 1; j < count; j++)
            {
                T key = list[j];

                int i = j - 1;
                for (; i >= 0 && comparison(list[i], key) > 0; i--)
                {
                    list[i + 1] = list[i];
                }
                list[i + 1] = key;
            }
        }
    }
}
