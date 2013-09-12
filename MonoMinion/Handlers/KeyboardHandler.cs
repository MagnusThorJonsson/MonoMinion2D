using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoMinion.Handlers
{
    /// <summary>
    /// Input Handler Component
    /// </summary>
    public class KeyboardHandler : GameComponent
    {
        #region Variables and Properties
        private static KeyboardState _keyboardState;
        public static KeyboardState KeyboardState { get { return _keyboardState; } }
        private static KeyboardState _lastKeyboardState;
        public static KeyboardState LastKeyboardState { get { return _lastKeyboardState; } }
        #endregion

        /// <summary>
        /// Input Handler Constructor
        /// </summary>
        /// <param name="game">Instance of the Game object</param>
        public KeyboardHandler(Game game)
            : base(game)
        {
            _keyboardState = Keyboard.GetState();
        }

        #region Overrideable Functions
        /// <summary>
        /// Overrideable Update function
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Flushes the last keyboard state
        /// </summary>
        public static void Flush()
        {
            _lastKeyboardState = _keyboardState;
        }
        #endregion


        #region Keyboard Functions
        /// <summary>
        /// Checks if key has been released
        /// </summary>
        /// <param name="key">Key being pressed</param>
        /// <returns>boolean</returns>
        public static bool KeyReleased(Keys key)
        {
            return _keyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if key is being pressed
        /// </summary>
        /// <param name="key">Key being pressed</param>
        /// <returns>boolean</returns>
        public static bool KeyPressed(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a key is being held down
        /// </summary>
        /// <param name="key">Key being pressed</param>
        /// <returns>boolean</returns>
        public static bool KeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }
        #endregion
    }
}
