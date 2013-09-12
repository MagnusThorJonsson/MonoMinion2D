using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoMinion.Handlers
{
    /// <summary>
    /// Handles mouse input
    /// </summary>
    public class MouseHandler : GameComponent
    {
        private const double TIMEHELD_BUFFER = 120;
        private const int MOUSE_HEIGHT = 5;
        private const int MOUSE_WIDTH = 5;

        #region Variables and Properties
        private static MouseState _currentState;
        private static MouseState _oldState;

        private static double[] _timeHeld;

        public MouseState CurrentState { get { return MouseHandler._currentState; } }
        public MouseState LastState { get { return MouseHandler._oldState; } }

        public static Vector2 Position
        {
            get
            {
                return new Vector2(MouseHandler._currentState.X, MouseHandler._currentState.Y);
            }
        }

        /// <summary>
        /// the current mouse velocity.
        /// </summary>
        public static Vector2 Velocity
        {
            get
            {
                return
                (
                    new Vector2(MouseHandler._currentState.X, MouseHandler._currentState.Y) -
                    new Vector2(MouseHandler._oldState.X, MouseHandler._oldState.Y)
                );
            }
        }
        #endregion

        #region Event Handlers
        public event EventHandler LeftClicked;
        public event EventHandler LeftHeld;
        public event EventHandler MiddleClicked;
        public event EventHandler MiddleHeld;
        public event EventHandler RightClicked;
        public event EventHandler RightHeld;
        #endregion

        public enum Buttons
        {
            Left,
            Middle,
            Right,
            X1,
            X2
        }

        /// <summary>
        /// MouseHandler constructor
        /// </summary>
        /// <param name="game">Game object instance</param>
        public MouseHandler(Game game)
            : base(game)
        {
            // Add 
            MouseHandler._timeHeld = new double[(int)(Buttons.X2) + 1];
            game.IsMouseVisible = true;
            MouseHandler._currentState = Mouse.GetState();
        }


        #region Overrideable Functions
        /// <summary>
        /// Initialize MouseHandler
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }


        /// <summary>
        /// Update MouseHandler
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            MouseHandler._oldState = MouseHandler._currentState;
            // Start timing the click
            if (MouseHandler.IsPressed(Buttons.Left))
                MouseHandler._timeHeld[(int)Buttons.Left] = MouseHandler._timeHeld[(int)Buttons.Left] + gameTime.ElapsedGameTime.TotalMilliseconds;
            if (MouseHandler.IsPressed(Buttons.Middle))
                MouseHandler._timeHeld[(int)Buttons.Middle] += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (MouseHandler.IsPressed(Buttons.Right))
                MouseHandler._timeHeld[(int)Buttons.Right] += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Clear all timers
            if (MouseHandler.IsReleased(Buttons.Left))
                MouseHandler._timeHeld[(int)Buttons.Left] = 0;
            if (MouseHandler.IsReleased(Buttons.Middle))
                MouseHandler._timeHeld[(int)Buttons.Middle] = 0;
            if (MouseHandler.IsReleased(Buttons.Right))
                MouseHandler._timeHeld[(int)Buttons.Right] = 0;

            // Fire of Left Button Events if applicable
            if (MouseHandler.IsMouseUp(Buttons.Left))
                this.OnLeftClicked(null);
            if (MouseHandler.IsHeld(Buttons.Left))
                this.OnLeftHeld(null);

            // Fire of Right Button Events if applicable
            if (MouseHandler.IsMouseUp(Buttons.Right))
                this.OnRightClicked(null);
            if (MouseHandler.IsHeld(Buttons.Right))
                this.OnRightHeld(null);

            // Fire of Middle Button Events if applicable
            if (MouseHandler.IsMouseUp(Buttons.Middle))
                this.OnMiddleClicked(null);
            if (MouseHandler.IsHeld(Buttons.Middle))
                this.OnMiddleHeld(null);

            MouseHandler._currentState = Mouse.GetState();

            base.Update(gameTime);
        }
        #endregion


        #region Static Functions
        /// <summary>
        /// Gets the position Rectangle for the mouse cursor
        /// </summary>
        /// <returns>Rectangle from the current mouse cursor position</returns>
        public static Rectangle PositionRect
        {
            get
            {
                Rectangle position = new Rectangle(
                    MouseHandler._currentState.X,
                    MouseHandler._currentState.Y,
                    MOUSE_WIDTH,
                    MOUSE_HEIGHT
                );

                return position;
            }
        }

        /// <summary>
        /// Checks if a mousebutton is in a pressed state (Will always return true when button is being held or pressed)
        /// </summary>
        /// <param name="button">Button to check for</param>
        /// <returns>boolean</returns>
        public static bool IsPressed(Buttons button)
        {
            switch (button)
            {
                case Buttons.Left:
                    if (MouseHandler._currentState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;

                case Buttons.Right:
                    if (MouseHandler._currentState.RightButton == ButtonState.Pressed)
                        return true;
                    break;

                case Buttons.Middle:
                    if (MouseHandler._currentState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
            }

            return false;
        }


        /// <summary>
        /// Checks if a mousebutton is in a released state (Will always return true when button is not being held or pressed)
        /// </summary>
        /// <param name="button">Button to check for</param>
        /// <returns>boolean</returns>
        public static bool IsReleased(Buttons button)
        {
            switch (button)
            {
                case Buttons.Left:
                    if (MouseHandler._currentState.LeftButton == ButtonState.Released)
                        return true;
                    break;

                case Buttons.Right:
                    if (MouseHandler._currentState.RightButton == ButtonState.Released)
                        return true;
                    break;

                case Buttons.Middle:
                    if (MouseHandler._currentState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
            }

            return false;
        }


        /// <summary>
        /// Checks if a mousebutton is released
        /// </summary>
        /// <param name="button">Button to check for</param>
        /// <returns>boolean</returns>
        public static bool IsMouseUp(Buttons button)
        {
            switch (button)
            {
                case Buttons.Left:
                    if (MouseHandler._currentState.LeftButton == ButtonState.Released &&
                        MouseHandler._oldState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;

                case Buttons.Right:
                    if (MouseHandler._currentState.RightButton == ButtonState.Released &&
                        MouseHandler._oldState.RightButton == ButtonState.Pressed)
                        return true;
                    break;

                case Buttons.Middle:
                    if (MouseHandler._currentState.MiddleButton == ButtonState.Released &&
                        MouseHandler._oldState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
            }

            return false;
        }


        /// <summary>
        /// Checks if a mousebutton pressed down
        /// </summary>
        /// <param name="button">Button to check for</param>
        /// <returns>boolean</returns>
        public static bool IsMouseDown(Buttons button)
        {
            switch (button)
            {
                case Buttons.Left:
                    if (MouseHandler._currentState.LeftButton == ButtonState.Pressed &&
                        MouseHandler._oldState.LeftButton == ButtonState.Released)
                        return true;
                    break;

                case Buttons.Right:
                    if (MouseHandler._currentState.RightButton == ButtonState.Pressed &&
                        MouseHandler._oldState.RightButton == ButtonState.Released)
                        return true;
                    break;

                case Buttons.Middle:
                    if (MouseHandler._currentState.MiddleButton == ButtonState.Pressed &&
                        MouseHandler._oldState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
            }

            return false;
        }


        /// <summary>
        /// Check if button is being held
        /// </summary>
        /// <param name="button">Button to check for</param>
        /// <returns>boolean</returns>
        public static bool IsHeld(Buttons button)
        {
            if (MouseHandler._timeHeld[(int)button] > TIMEHELD_BUFFER)
            {
                switch (button)
                {
                    case Buttons.Left:
                        if (MouseHandler._currentState.LeftButton == ButtonState.Pressed &&
                            MouseHandler._oldState.LeftButton == ButtonState.Pressed)
                            return true;
                        break;

                    case Buttons.Right:
                        if (MouseHandler._currentState.RightButton == ButtonState.Pressed &&
                            MouseHandler._oldState.RightButton == ButtonState.Pressed)
                            return true;
                        break;

                    case Buttons.Middle:
                        if (MouseHandler._currentState.MiddleButton == ButtonState.Pressed &&
                            MouseHandler._oldState.MiddleButton == ButtonState.Pressed)
                            return true;
                        break;
                }
            }

            return false;
        }
        #endregion


        #region Event Functions
        /// <summary>
        /// Fire off Left button clicked
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLeftClicked(EventArgs e)
        {
            if (LeftClicked != null)
                LeftClicked(this, e);
        }
        /// <summary>
        /// Fire off Left button held event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLeftHeld(EventArgs e)
        {
            if (LeftHeld != null)
                LeftHeld(this, e);
        }
        /// <summary>
        /// Fire off Right button clicked event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRightClicked(EventArgs e)
        {
            if (RightClicked != null)
                RightClicked(this, e);
        }
        /// <summary>
        /// Fire off Right button held event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRightHeld(EventArgs e)
        {
            if (RightHeld != null)
                RightHeld(this, e);
        }
        /// <summary>
        /// Fire off Middle button clicked event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMiddleClicked(EventArgs e)
        {
            if (MiddleClicked != null)
                MiddleClicked(this, e);
        }
        /// <summary>
        /// Fire off Middle button held event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMiddleHeld(EventArgs e)
        {
            if (MiddleHeld != null)
                MiddleHeld(this, e);
        }
        #endregion
    }
}
