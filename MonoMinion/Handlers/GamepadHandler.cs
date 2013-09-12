using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoMinion.Handlers
{
    /// <summary>
    /// Contains the available gamepad button states
    /// </summary>
    public enum GamePadButtonState
    {
        None,
        Clicked,
        Held,
        Released
    }

    /// <summary>
    /// A simple and generic gamepad handler
    /// </summary>
    public class GamepadHandler
    {
        // Setup
        protected PlayerIndex player;
        public PlayerIndex Player { get { return player; } }

        protected GamePadState currentState;
        public GamePadState CurrentState { get { return currentState; } }
        protected GamePadState previousState;
        public GamePadState PreviousState { get { return previousState; } }

        // Thumbstick
        public Vector2 LThumb { get { return currentState.ThumbSticks.Left; } }
        public Vector2 RThumb { get { return currentState.ThumbSticks.Right; } }
        protected GamePadButtonState thumb_Left;
        public GamePadButtonState LThumbButton { get { return thumb_Left; } }
        protected GamePadButtonState thumb_Right;
        public GamePadButtonState RThumbButton { get { return thumb_Right; } }

        // Triggers
        public float LTrigger { get { return currentState.Triggers.Left; } }
        public float RTrigger { get { return currentState.Triggers.Right; } }

        protected GamePadButtonState trigger_Left;
        public GamePadButtonState LTriggerButton { get { return trigger_Left; } }
        protected GamePadButtonState trigger_Right;
        public GamePadButtonState RTriggerButton { get { return trigger_Right; } }

        // Buttons - Shoulders
        protected GamePadButtonState shoulder_Left;
        public GamePadButtonState LShoulder { get { return shoulder_Left; } }
        protected GamePadButtonState shoulder_Right;
        public GamePadButtonState RShoulder { get { return shoulder_Right; } }

        // Buttons - Action
        protected GamePadButtonState button_A;
        public GamePadButtonState A { get { return button_A; } }
        protected GamePadButtonState button_B;
        public GamePadButtonState B { get { return button_B; } }
        protected GamePadButtonState button_X;
        public GamePadButtonState X { get { return button_X; } }
        protected GamePadButtonState button_Y;
        public GamePadButtonState Y { get { return button_Y; } }
        
        // Buttons - Control
        protected GamePadButtonState button_Select;
        public GamePadButtonState Select { get { return button_Select; } }
        protected GamePadButtonState button_Start;
        public GamePadButtonState Start { get { return button_Start; } }
        protected GamePadButtonState button_Guide;
        public GamePadButtonState Guide { get { return button_Guide; } }
        
        // D-Pad
        protected GamePadButtonState dpad_Up;
        public GamePadButtonState Up { get { return dpad_Up; } }
        protected GamePadButtonState dpad_Down;
        public GamePadButtonState Down { get { return dpad_Down; } }
        protected GamePadButtonState dpad_Left;
        public GamePadButtonState Left { get { return dpad_Left; } }
        protected GamePadButtonState dpad_Right;
        public GamePadButtonState Right { get { return dpad_Right; } }

        /// <summary>
        /// GamepadHandler constructor
        /// </summary>
        /// <param name="player">The player index of the gamepad being used by this handler</param>
        public GamepadHandler(PlayerIndex player)
        {
            this.player = player;
        }

        /// <summary>
        /// Call to update the gamepad handler
        /// </summary>
        /// <param name="gameTime">Current GameTime object</param>
        public virtual void Update(GameTime gameTime)
        {
            currentState = GamePad.GetState(player);

            if (currentState.IsConnected)
            {
                // todo: handle a if previousState == null scenario

                // THIS IS GOING TO GET UGLY
                // Buttons
                #region Left shoulder
                if (currentState.Buttons.LeftShoulder == ButtonState.Released &&
                    previousState.Buttons.LeftShoulder == ButtonState.Released)
                {
                    shoulder_Left = GamePadButtonState.None;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Pressed &&
                    previousState.Buttons.LeftShoulder == ButtonState.Released)
                {
                    shoulder_Left = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Pressed &&
                    previousState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    shoulder_Left = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Released &&
                    previousState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    shoulder_Left = GamePadButtonState.Released;
                }
                #endregion

                #region Right shoulder
                if (currentState.Buttons.RightShoulder == ButtonState.Released &&
                    previousState.Buttons.RightShoulder == ButtonState.Released)
                {
                    shoulder_Right = GamePadButtonState.None;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Pressed &&
                    previousState.Buttons.RightShoulder == ButtonState.Released)
                {
                    shoulder_Right = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Pressed &&
                    previousState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    shoulder_Right = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Released &&
                    previousState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    shoulder_Right = GamePadButtonState.Released;
                }
                #endregion

                #region Left Thumb Button
                if (currentState.Buttons.LeftStick == ButtonState.Released &&
                    previousState.Buttons.LeftStick == ButtonState.Released)
                {
                    thumb_Left = GamePadButtonState.None;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Pressed &&
                    previousState.Buttons.LeftStick == ButtonState.Released)
                {
                    thumb_Left = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Pressed &&
                    previousState.Buttons.LeftStick == ButtonState.Pressed)
                {
                    thumb_Left = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Released &&
                    previousState.Buttons.LeftStick == ButtonState.Pressed)
                {
                    thumb_Left = GamePadButtonState.Released;
                }
                #endregion

                #region Right Thumb Button
                if (currentState.Buttons.RightStick == ButtonState.Released &&
                    previousState.Buttons.RightStick == ButtonState.Released)
                {
                    thumb_Right = GamePadButtonState.None;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Pressed &&
                    previousState.Buttons.RightStick == ButtonState.Released)
                {
                    thumb_Right = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Pressed &&
                    previousState.Buttons.RightStick == ButtonState.Pressed)
                {
                    thumb_Right = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Released &&
                    previousState.Buttons.RightStick == ButtonState.Pressed)
                {
                    thumb_Right = GamePadButtonState.Released;
                }
                #endregion

                #region Left Trigger Button
                if (currentState.Triggers.Left <= 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = GamePadButtonState.None;
                }
                else if (currentState.Triggers.Left > 0f &&
                    previousState.Triggers.Left <= 0f)
                {
                    trigger_Left = GamePadButtonState.Clicked;
                }
                else if (currentState.Triggers.Left > 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = GamePadButtonState.Held;
                }
                else if (currentState.Triggers.Left <= 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = GamePadButtonState.Released;
                }
                #endregion

                #region Right Trigger Button
                if (currentState.Triggers.Right <= 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = GamePadButtonState.None;
                }
                else if (currentState.Triggers.Right > 0f &&
                    previousState.Triggers.Right <= 0f)
                {
                    trigger_Right = GamePadButtonState.Clicked;
                }
                else if (currentState.Triggers.Right > 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = GamePadButtonState.Held;
                }
                else if (currentState.Triggers.Right <= 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = GamePadButtonState.Released;
                }
                #endregion

                #region A Button
                if (currentState.Buttons.A == ButtonState.Released &&
                    previousState.Buttons.A == ButtonState.Released)
                {
                    button_A = GamePadButtonState.None;
                }
                else if (currentState.Buttons.A == ButtonState.Pressed &&
                    previousState.Buttons.A == ButtonState.Released)
                {
                    button_A = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.A == ButtonState.Pressed &&
                    previousState.Buttons.A == ButtonState.Pressed)
                {
                    button_A = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.A == ButtonState.Released &&
                    previousState.Buttons.A == ButtonState.Pressed)
                {
                    button_A = GamePadButtonState.Released;
                }
                #endregion

                #region B Button
                if (currentState.Buttons.B == ButtonState.Released &&
                    previousState.Buttons.B == ButtonState.Released)
                {
                    button_B = GamePadButtonState.None;
                }
                else if (currentState.Buttons.B == ButtonState.Pressed &&
                    previousState.Buttons.B == ButtonState.Released)
                {
                    button_B = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.B == ButtonState.Pressed &&
                    previousState.Buttons.B == ButtonState.Pressed)
                {
                    button_B = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.B == ButtonState.Released &&
                    previousState.Buttons.B == ButtonState.Pressed)
                {
                    button_B = GamePadButtonState.Released;
                }
                #endregion

                #region X Button
                if (currentState.Buttons.X == ButtonState.Released &&
                    previousState.Buttons.X == ButtonState.Released)
                {
                    button_X = GamePadButtonState.None;
                }
                else if (currentState.Buttons.X == ButtonState.Pressed &&
                    previousState.Buttons.X == ButtonState.Released)
                {
                    button_X = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.X == ButtonState.Pressed &&
                    previousState.Buttons.X == ButtonState.Pressed)
                {
                    button_X = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.X == ButtonState.Released &&
                    previousState.Buttons.X == ButtonState.Pressed)
                {
                    button_X = GamePadButtonState.Released;
                }
                #endregion

                #region Y Button
                if (currentState.Buttons.Y == ButtonState.Released &&
                    previousState.Buttons.Y == ButtonState.Released)
                {
                    button_Y = GamePadButtonState.None;
                }
                else if (currentState.Buttons.Y == ButtonState.Pressed &&
                    previousState.Buttons.Y == ButtonState.Released)
                {
                    button_Y = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.Y == ButtonState.Pressed &&
                    previousState.Buttons.Y == ButtonState.Pressed)
                {
                    button_Y = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.Y == ButtonState.Released &&
                    previousState.Buttons.Y == ButtonState.Pressed)
                {
                    button_Y = GamePadButtonState.Released;
                }
                #endregion

                #region Select Button
                if (currentState.Buttons.Back == ButtonState.Released &&
                    previousState.Buttons.Back == ButtonState.Released)
                {
                    button_Select = GamePadButtonState.None;
                }
                else if (currentState.Buttons.Back == ButtonState.Pressed &&
                    previousState.Buttons.Back == ButtonState.Released)
                {
                    button_Select = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.Back == ButtonState.Pressed &&
                    previousState.Buttons.Back == ButtonState.Pressed)
                {
                    button_Select = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.Back == ButtonState.Released &&
                    previousState.Buttons.Back == ButtonState.Pressed)
                {
                    button_Select = GamePadButtonState.Released;
                }
                #endregion

                #region Start Button
                if (currentState.Buttons.Start == ButtonState.Released &&
                    previousState.Buttons.Start == ButtonState.Released)
                {
                    button_Start = GamePadButtonState.None;
                }
                else if (currentState.Buttons.Start == ButtonState.Pressed &&
                    previousState.Buttons.Start == ButtonState.Released)
                {
                    button_Start = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.Start == ButtonState.Pressed &&
                    previousState.Buttons.Start == ButtonState.Pressed)
                {
                    button_Start = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.Start == ButtonState.Released &&
                    previousState.Buttons.Start == ButtonState.Pressed)
                {
                    button_Start = GamePadButtonState.Released;
                }
                #endregion

                #region Guide Button
                if (currentState.Buttons.BigButton == ButtonState.Released &&
                    previousState.Buttons.BigButton == ButtonState.Released)
                {
                    button_Guide = GamePadButtonState.None;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Pressed &&
                    previousState.Buttons.BigButton == ButtonState.Released)
                {
                    button_Guide = GamePadButtonState.Clicked;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Pressed &&
                    previousState.Buttons.BigButton == ButtonState.Pressed)
                {
                    button_Guide = GamePadButtonState.Held;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Released &&
                    previousState.Buttons.BigButton == ButtonState.Pressed)
                {
                    button_Guide = GamePadButtonState.Released;
                }
                #endregion

                // D-Pad
                #region Up
                if (currentState.DPad.Up == ButtonState.Released &&
                    previousState.DPad.Up == ButtonState.Released)
                {
                    dpad_Up = GamePadButtonState.None;
                }
                else if (currentState.DPad.Up == ButtonState.Pressed &&
                    previousState.DPad.Up == ButtonState.Released)
                {
                    dpad_Up = GamePadButtonState.Clicked;
                }
                else if (currentState.DPad.Up == ButtonState.Pressed &&
                    previousState.DPad.Up == ButtonState.Pressed)
                {
                    dpad_Up = GamePadButtonState.Held;
                }
                else if (currentState.DPad.Up == ButtonState.Released &&
                    previousState.DPad.Up == ButtonState.Pressed)
                {
                    dpad_Up = GamePadButtonState.Released;
                }
                #endregion

                #region Down
                if (currentState.DPad.Down == ButtonState.Released &&
                    previousState.DPad.Down == ButtonState.Released)
                {
                    dpad_Down = GamePadButtonState.None;
                }
                else if (currentState.DPad.Down == ButtonState.Pressed &&
                    previousState.DPad.Down == ButtonState.Released)
                {
                    dpad_Down = GamePadButtonState.Clicked;
                }
                else if (currentState.DPad.Down == ButtonState.Pressed &&
                    previousState.DPad.Down == ButtonState.Pressed)
                {
                    dpad_Down = GamePadButtonState.Held;
                }
                else if (currentState.DPad.Down == ButtonState.Released &&
                    previousState.DPad.Down == ButtonState.Pressed)
                {
                    dpad_Down = GamePadButtonState.Released;
                }
                #endregion

                #region Left
                if (currentState.DPad.Left == ButtonState.Released &&
                    previousState.DPad.Left == ButtonState.Released)
                {
                    dpad_Left = GamePadButtonState.None;
                }
                else if (currentState.DPad.Left == ButtonState.Pressed &&
                    previousState.DPad.Left == ButtonState.Released)
                {
                    dpad_Left = GamePadButtonState.Clicked;
                }
                else if (currentState.DPad.Left == ButtonState.Pressed &&
                    previousState.DPad.Left == ButtonState.Pressed)
                {
                    dpad_Left = GamePadButtonState.Held;
                }
                else if (currentState.DPad.Left == ButtonState.Released &&
                    previousState.DPad.Left == ButtonState.Pressed)
                {
                    dpad_Left = GamePadButtonState.Released;
                }
                #endregion

                #region Right
                if (currentState.DPad.Right == ButtonState.Released &&
                    previousState.DPad.Right == ButtonState.Released)
                {
                    dpad_Right = GamePadButtonState.None;
                }
                else if (currentState.DPad.Right == ButtonState.Pressed &&
                    previousState.DPad.Right == ButtonState.Released)
                {
                    dpad_Right = GamePadButtonState.Clicked;
                }
                else if (currentState.DPad.Right == ButtonState.Pressed &&
                    previousState.DPad.Right == ButtonState.Pressed)
                {
                    dpad_Right = GamePadButtonState.Held;
                }
                else if (currentState.DPad.Right == ButtonState.Released &&
                    previousState.DPad.Right == ButtonState.Pressed)
                {
                    dpad_Right = GamePadButtonState.Released;
                }
                #endregion

                previousState = currentState;
            }          
        }


        /// <summary>
        /// Sets the gamepad vibration amount
        /// </summary>
        /// <param name="left">Left motor vibration amount (0.0 .. 1.0)</param>
        /// <param name="right">Right motor vibration amount (0.0 .. 1.0)</param>
        public void SetVibration(float left, float right)
        {
            //GamePad.SetVibration(player, left, right);
        }
    }

}
