using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoMinion.Input.enums;
using System;
using System.Collections.Generic;

namespace MonoMinion.Input.Handlers
{
    /// <summary>
    /// A simple and generic gamepad handler
    /// </summary>
    public class GamepadHandler
    {
        #region Variables & Properties
        // Setup
        protected PlayerIndex player;
        public PlayerIndex Player { get { return player; } }

        protected GamePadState currentState;
        public GamePadState CurrentState { get { return currentState; } }
        protected GamePadState previousState;
        public GamePadState PreviousState { get { return previousState; } }

        // Thumbstick
        public Vector2 RThumb { get { return currentState.ThumbSticks.Right; } }
        public Vector2 LThumb { get { return currentState.ThumbSticks.Left; } }
        public Vector2 LThumbInvertY 
        { 
            get 
            { 
                return new Vector2(
                    currentState.ThumbSticks.Left.X,
                    currentState.ThumbSticks.Left.Y * -1
                ); 
            }
        }
        protected InputButtonState thumb_Right = InputButtonState.None;
        public InputButtonState RThumbButton { get { return thumb_Right; } }
        protected InputButtonState thumb_Left = InputButtonState.None;
        public InputButtonState LThumbButton { get { return thumb_Left; } }

        // Triggers
        public float LTrigger { get { return currentState.Triggers.Left; } }
        public float RTrigger { get { return currentState.Triggers.Right; } }

        protected InputButtonState trigger_Left = InputButtonState.None;
        public InputButtonState LTriggerButton { get { return trigger_Left; } }
        protected InputButtonState trigger_Right = InputButtonState.None;
        public InputButtonState RTriggerButton { get { return trigger_Right; } }

        // Buttons - Shoulders
        protected InputButtonState shoulder_Left = InputButtonState.None;
        public InputButtonState LShoulder { get { return shoulder_Left; } }
        protected InputButtonState shoulder_Right = InputButtonState.None;
        public InputButtonState RShoulder { get { return shoulder_Right; } }

        // Buttons - Action
        protected InputButtonState button_A = InputButtonState.None;
        public InputButtonState A { get { return button_A; } }
        protected InputButtonState button_B = InputButtonState.None;
        public InputButtonState B { get { return button_B; } }
        protected InputButtonState button_X = InputButtonState.None;
        public InputButtonState X { get { return button_X; } }
        protected InputButtonState button_Y = InputButtonState.None;
        public InputButtonState Y { get { return button_Y; } }
        
        // Buttons - Control
        protected InputButtonState button_Select = InputButtonState.None;
        public InputButtonState Select { get { return button_Select; } }
        protected InputButtonState button_Start = InputButtonState.None;
        public InputButtonState Start { get { return button_Start; } }
        protected InputButtonState button_Guide = InputButtonState.None;
        public InputButtonState Guide { get { return button_Guide; } }
        
        // D-Pad
        protected InputButtonState dpad_Up = InputButtonState.None;
        public InputButtonState Up { get { return dpad_Up; } }
        protected InputButtonState dpad_Down = InputButtonState.None;
        public InputButtonState Down { get { return dpad_Down; } }
        protected InputButtonState dpad_Left = InputButtonState.None;
        public InputButtonState Left { get { return dpad_Left; } }
        protected InputButtonState dpad_Right = InputButtonState.None;
        public InputButtonState Right { get { return dpad_Right; } }
        #endregion


        #region Main (Constructor, Update, etc)
        /// <summary>
        /// GamepadHandler constructor
        /// </summary>
        /// <param name="player">The player index of the gamepad being used by this handler</param>
        public GamepadHandler(PlayerIndex player)
        {
            this.player = player;
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            SaveState();
        }

        /// <summary>
        /// Call to update the gamepad handler
        /// </summary>
        /// <param name="gameTime">Current GameTime object</param>
        public void UpdateState(GameTime gameTime)
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
                    shoulder_Left = InputButtonState.None;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Pressed &&
                    previousState.Buttons.LeftShoulder == ButtonState.Released)
                {
                    shoulder_Left = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Pressed &&
                    previousState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    shoulder_Left = InputButtonState.Held;
                }
                else if (currentState.Buttons.LeftShoulder == ButtonState.Released &&
                    previousState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    shoulder_Left = InputButtonState.Released;
                }
                #endregion

                #region Right shoulder
                if (currentState.Buttons.RightShoulder == ButtonState.Released &&
                    previousState.Buttons.RightShoulder == ButtonState.Released)
                {
                    shoulder_Right = InputButtonState.None;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Pressed &&
                    previousState.Buttons.RightShoulder == ButtonState.Released)
                {
                    shoulder_Right = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Pressed &&
                    previousState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    shoulder_Right = InputButtonState.Held;
                }
                else if (currentState.Buttons.RightShoulder == ButtonState.Released &&
                    previousState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    shoulder_Right = InputButtonState.Released;
                }
                #endregion

                #region Left Thumb Button
                if (currentState.Buttons.LeftStick == ButtonState.Released &&
                    previousState.Buttons.LeftStick == ButtonState.Released)
                {
                    thumb_Left = InputButtonState.None;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Pressed &&
                    previousState.Buttons.LeftStick == ButtonState.Released)
                {
                    thumb_Left = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Pressed &&
                    previousState.Buttons.LeftStick == ButtonState.Pressed)
                {
                    thumb_Left = InputButtonState.Held;
                }
                else if (currentState.Buttons.LeftStick == ButtonState.Released &&
                    previousState.Buttons.LeftStick == ButtonState.Pressed)
                {
                    thumb_Left = InputButtonState.Released;
                }
                #endregion

                #region Right Thumb Button
                if (currentState.Buttons.RightStick == ButtonState.Released &&
                    previousState.Buttons.RightStick == ButtonState.Released)
                {
                    thumb_Right = InputButtonState.None;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Pressed &&
                    previousState.Buttons.RightStick == ButtonState.Released)
                {
                    thumb_Right = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Pressed &&
                    previousState.Buttons.RightStick == ButtonState.Pressed)
                {
                    thumb_Right = InputButtonState.Held;
                }
                else if (currentState.Buttons.RightStick == ButtonState.Released &&
                    previousState.Buttons.RightStick == ButtonState.Pressed)
                {
                    thumb_Right = InputButtonState.Released;
                }
                #endregion

                #region Left Trigger Button
                if (currentState.Triggers.Left <= 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = InputButtonState.None;
                }
                else if (currentState.Triggers.Left > 0f &&
                    previousState.Triggers.Left <= 0f)
                {
                    trigger_Left = InputButtonState.Clicked;
                }
                else if (currentState.Triggers.Left > 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = InputButtonState.Held;
                }
                else if (currentState.Triggers.Left <= 0f &&
                    previousState.Triggers.Left > 0f)
                {
                    trigger_Left = InputButtonState.Released;
                }
                #endregion
                
                #region Right Trigger Button
                if (currentState.Triggers.Right <= 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = InputButtonState.None;
                }
                else if (currentState.Triggers.Right > 0f &&
                    previousState.Triggers.Right <= 0f)
                {
                    trigger_Right = InputButtonState.Clicked;
                }
                else if (currentState.Triggers.Right > 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = InputButtonState.Held;
                }
                else if (currentState.Triggers.Right <= 0f &&
                    previousState.Triggers.Right > 0f)
                {
                    trigger_Right = InputButtonState.Released;
                }
                #endregion

                #region A Button
                if (currentState.Buttons.A == ButtonState.Released &&
                    previousState.Buttons.A == ButtonState.Released)
                {
                    button_A = InputButtonState.None;
                }
                else if (currentState.Buttons.A == ButtonState.Pressed &&
                    previousState.Buttons.A == ButtonState.Released)
                {
                    button_A = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.A == ButtonState.Pressed &&
                    previousState.Buttons.A == ButtonState.Pressed)
                {
                    button_A = InputButtonState.Held;
                }
                else if (currentState.Buttons.A == ButtonState.Released &&
                    previousState.Buttons.A == ButtonState.Pressed)
                {
                    button_A = InputButtonState.Released;
                }
                #endregion

                #region B Button
                if (currentState.Buttons.B == ButtonState.Released &&
                    previousState.Buttons.B == ButtonState.Released)
                {
                    button_B = InputButtonState.None;
                }
                else if (currentState.Buttons.B == ButtonState.Pressed &&
                    previousState.Buttons.B == ButtonState.Released)
                {
                    button_B = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.B == ButtonState.Pressed &&
                    previousState.Buttons.B == ButtonState.Pressed)
                {
                    button_B = InputButtonState.Held;
                }
                else if (currentState.Buttons.B == ButtonState.Released &&
                    previousState.Buttons.B == ButtonState.Pressed)
                {
                    button_B = InputButtonState.Released;
                }
                #endregion

                #region X Button
                if (currentState.Buttons.X == ButtonState.Released &&
                    previousState.Buttons.X == ButtonState.Released)
                {
                    button_X = InputButtonState.None;
                }
                else if (currentState.Buttons.X == ButtonState.Pressed &&
                    previousState.Buttons.X == ButtonState.Released)
                {
                    button_X = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.X == ButtonState.Pressed &&
                    previousState.Buttons.X == ButtonState.Pressed)
                {
                    button_X = InputButtonState.Held;
                }
                else if (currentState.Buttons.X == ButtonState.Released &&
                    previousState.Buttons.X == ButtonState.Pressed)
                {
                    button_X = InputButtonState.Released;
                }
                #endregion

                #region Y Button
                if (currentState.Buttons.Y == ButtonState.Released &&
                    previousState.Buttons.Y == ButtonState.Released)
                {
                    button_Y = InputButtonState.None;
                }
                else if (currentState.Buttons.Y == ButtonState.Pressed &&
                    previousState.Buttons.Y == ButtonState.Released)
                {
                    button_Y = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.Y == ButtonState.Pressed &&
                    previousState.Buttons.Y == ButtonState.Pressed)
                {
                    button_Y = InputButtonState.Held;
                }
                else if (currentState.Buttons.Y == ButtonState.Released &&
                    previousState.Buttons.Y == ButtonState.Pressed)
                {
                    button_Y = InputButtonState.Released;
                }
                #endregion

                #region Select Button
                if (currentState.Buttons.Back == ButtonState.Released &&
                    previousState.Buttons.Back == ButtonState.Released)
                {
                    button_Select = InputButtonState.None;
                }
                else if (currentState.Buttons.Back == ButtonState.Pressed &&
                    previousState.Buttons.Back == ButtonState.Released)
                {
                    button_Select = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.Back == ButtonState.Pressed &&
                    previousState.Buttons.Back == ButtonState.Pressed)
                {
                    button_Select = InputButtonState.Held;
                }
                else if (currentState.Buttons.Back == ButtonState.Released &&
                    previousState.Buttons.Back == ButtonState.Pressed)
                {
                    button_Select = InputButtonState.Released;
                }
                #endregion

                #region Start Button
                if (currentState.Buttons.Start == ButtonState.Released &&
                    previousState.Buttons.Start == ButtonState.Released)
                {
                    button_Start = InputButtonState.None;
                }
                else if (currentState.Buttons.Start == ButtonState.Pressed &&
                    previousState.Buttons.Start == ButtonState.Released)
                {
                    button_Start = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.Start == ButtonState.Pressed &&
                    previousState.Buttons.Start == ButtonState.Pressed)
                {
                    button_Start = InputButtonState.Held;
                }
                else if (currentState.Buttons.Start == ButtonState.Released &&
                    previousState.Buttons.Start == ButtonState.Pressed)
                {
                    button_Start = InputButtonState.Released;
                }
                #endregion

                #region Guide Button
                if (currentState.Buttons.BigButton == ButtonState.Released &&
                    previousState.Buttons.BigButton == ButtonState.Released)
                {
                    button_Guide = InputButtonState.None;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Pressed &&
                    previousState.Buttons.BigButton == ButtonState.Released)
                {
                    button_Guide = InputButtonState.Clicked;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Pressed &&
                    previousState.Buttons.BigButton == ButtonState.Pressed)
                {
                    button_Guide = InputButtonState.Held;
                }
                else if (currentState.Buttons.BigButton == ButtonState.Released &&
                    previousState.Buttons.BigButton == ButtonState.Pressed)
                {
                    button_Guide = InputButtonState.Released;
                }
                #endregion

                // D-Pad
                #region Up
                if (currentState.DPad.Up == ButtonState.Released &&
                    previousState.DPad.Up == ButtonState.Released)
                {
                    dpad_Up = InputButtonState.None;
                }
                else if (currentState.DPad.Up == ButtonState.Pressed &&
                    previousState.DPad.Up == ButtonState.Released)
                {
                    dpad_Up = InputButtonState.Clicked;
                }
                else if (currentState.DPad.Up == ButtonState.Pressed &&
                    previousState.DPad.Up == ButtonState.Pressed)
                {
                    dpad_Up = InputButtonState.Held;
                }
                else if (currentState.DPad.Up == ButtonState.Released &&
                    previousState.DPad.Up == ButtonState.Pressed)
                {
                    dpad_Up = InputButtonState.Released;
                }
                #endregion

                #region Down
                if (currentState.DPad.Down == ButtonState.Released &&
                    previousState.DPad.Down == ButtonState.Released)
                {
                    dpad_Down = InputButtonState.None;
                }
                else if (currentState.DPad.Down == ButtonState.Pressed &&
                    previousState.DPad.Down == ButtonState.Released)
                {
                    dpad_Down = InputButtonState.Clicked;
                }
                else if (currentState.DPad.Down == ButtonState.Pressed &&
                    previousState.DPad.Down == ButtonState.Pressed)
                {
                    dpad_Down = InputButtonState.Held;
                }
                else if (currentState.DPad.Down == ButtonState.Released &&
                    previousState.DPad.Down == ButtonState.Pressed)
                {
                    dpad_Down = InputButtonState.Released;
                }
                #endregion

                #region Left
                if (currentState.DPad.Left == ButtonState.Released &&
                    previousState.DPad.Left == ButtonState.Released)
                {
                    dpad_Left = InputButtonState.None;
                }
                else if (currentState.DPad.Left == ButtonState.Pressed &&
                    previousState.DPad.Left == ButtonState.Released)
                {
                    dpad_Left = InputButtonState.Clicked;
                }
                else if (currentState.DPad.Left == ButtonState.Pressed &&
                    previousState.DPad.Left == ButtonState.Pressed)
                {
                    dpad_Left = InputButtonState.Held;
                }
                else if (currentState.DPad.Left == ButtonState.Released &&
                    previousState.DPad.Left == ButtonState.Pressed)
                {
                    dpad_Left = InputButtonState.Released;
                }
                #endregion

                #region Right
                if (currentState.DPad.Right == ButtonState.Released &&
                    previousState.DPad.Right == ButtonState.Released)
                {
                    dpad_Right = InputButtonState.None;
                }
                else if (currentState.DPad.Right == ButtonState.Pressed &&
                    previousState.DPad.Right == ButtonState.Released)
                {
                    dpad_Right = InputButtonState.Clicked;
                }
                else if (currentState.DPad.Right == ButtonState.Pressed &&
                    previousState.DPad.Right == ButtonState.Pressed)
                {
                    dpad_Right = InputButtonState.Held;
                }
                else if (currentState.DPad.Right == ButtonState.Released &&
                    previousState.DPad.Right == ButtonState.Pressed)
                {
                    dpad_Right = InputButtonState.Released;
                }
                #endregion
            }          
        }

        public void SaveState()
        {
            previousState = currentState;
        }
        #endregion


        #region Helpers
        /// <summary>
        /// Sets the gamepad vibration amount
        /// </summary>
        /// <param name="left">Left motor vibration amount (0.0 .. 1.0)</param>
        /// <param name="right">Right motor vibration amount (0.0 .. 1.0)</param>
        public void SetVibration(float left, float right)
        {
            // TODO: Implement vibration functionality
            //GamePad.SetVibration(player, left, right);
        }


        /// <summary>
        /// Gets the button state for a specific GamePad button
        /// </summary>
        /// <param name="button">The GamePad button to check</param>
        /// <returns>The current gamepad button state</returns>
        public InputButtonState GetButtonState(Buttons button)
        {
            if (currentState.IsButtonDown(button) &&
                previousState.IsButtonUp(button))
            {
                return InputButtonState.Clicked;
            }
            else if (currentState.IsButtonDown(button) &&
                previousState.IsButtonDown(button))
            {
                return InputButtonState.Held;
            }
            else if (currentState.IsButtonUp(button) &&
                previousState.IsButtonDown(button))
            {
                return InputButtonState.Released;
            }

            return InputButtonState.None;
        }
        #endregion
    }

}
