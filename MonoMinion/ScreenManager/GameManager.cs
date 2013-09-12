using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoMinion
{
    /// <summary>
    /// Lightweight Screen Manager class
    /// </summary>
    public class GameManager : DrawableGameComponent
    {
        private List<GameScreen> _gameScreens;
        public bool IsInitialized { get; set; }

        protected Color _backgroundColor;
        public Color BackgroundColor 
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; } 
        }

        /// <summary>
        /// Game Manager constructor
        /// </summary>
        /// <param name="game">Instance of the Game object</param>
        public GameManager(Game game)
            : base(game)
        {
            this._gameScreens = new List<GameScreen>();
            this.IsInitialized = false;
            this._backgroundColor = Color.CornflowerBlue;
        }

        #region Drawable overrides
        /// <summary>
        /// Initialize override
        /// </summary>
        public override void Initialize()
        {
            foreach (GameScreen screen in this._gameScreens)
                screen.Initialize();

            this.IsInitialized = true;
            base.Initialize();
        }


        /// <summary>
        /// Update override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            // Update all screens that are visible and aren't paused
            for (int i = 0; i < _gameScreens.Count; i++)
            {
                if (_gameScreens[i].Visible && !_gameScreens[i].IsPaused)
                    _gameScreens[i].Update(gameTime);
            }

            //if (this._gameScreens.Count > 0)
            //    this._gameScreens.Peek().Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this._backgroundColor);

            for (int i = _gameScreens.Count - 1; i >= 0; i--)
                if (_gameScreens[i].Visible)
                    _gameScreens[i].Draw(gameTime);

            base.Draw(gameTime);
        }
        #endregion


        #region Screen Manipulation
        /// <summary>
        /// Pushes a new screen on top of the stack
        /// </summary>
        /// <param name="screen">The GameScreen to be added</param>
        public void PushScreen(GameScreen screen)
        {
            if (this.IsInitialized && !screen.IsInitialized)
                screen.Initialize();

            this._gameScreens.Add(screen);
        }


        /// <summary>
        /// Pops the topmost screen of the stack
        /// </summary>
        /// <returns>The GameScreen popped off the stack</returns>
        public GameScreen PopScreen()
        {
            if (_gameScreens.Count == 0)
                return null;

            GameScreen screen = _gameScreens[_gameScreens.Count - 1];
            _gameScreens.RemoveAt(_gameScreens.Count - 1);
            screen.Deactivate();

            return screen;
        }


        /// <summary>
        /// Gets the current top screen on the stack
        /// </summary>
        /// <returns>The top GameScreen on the stack</returns>
        public GameScreen TopScreen()
        {
            if (_gameScreens.Count == 0)
                return null;

            return _gameScreens[_gameScreens.Count - 1];
        }
        #endregion
    }
}
