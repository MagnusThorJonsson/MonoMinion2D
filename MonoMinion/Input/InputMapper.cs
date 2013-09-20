using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoMinion.Input.enums;
using MonoMinion.Input.Handlers;

namespace MonoMinion.Input
{
    /// <summary>
    /// A delegate for any action trigger used in the Input Mapper
    /// </summary>
    /// <param name="action"></param>
    public delegate void InputMapperActionTrigger(InputAction action);

    /// <summary>
    /// Handles mapping of actions to Keyboard and GamePad input
    /// </summary>
    public class InputMapper
    {
        #region Variables & Properties
        private static Dictionary<string, InputAction> actions = new Dictionary<string, InputAction>();

        private GamepadHandler gamepadHandler;
#if !XBOX
        private KeyboardHandler keyboardHandler;
#endif

        public event InputMapperActionTrigger ActionEvent;
        #endregion


        #region Constructors
#if !XBOX
        /// <summary>
        /// Keyboard only
        /// </summary>
        /// <param name="keyboardHandler">The keyboard handler</param>
        public InputMapper(KeyboardHandler keyboardHandler)
        {
            this.keyboardHandler = keyboardHandler;
            this.gamepadHandler = null;
        }

        /// <summary>
        /// Keyboard and Gamepad
        /// </summary>
        /// <param name="keyboardHandler">The keyboard handler</param>
        /// <param name="gamepad">The GamePad handler</param>
        public InputMapper(KeyboardHandler keyboardHandler, GamepadHandler gamepadHandler)
        {
            this.keyboardHandler = keyboardHandler;
            this.gamepadHandler = gamepadHandler;
        }
#endif

        /// <summary>
        /// Gamepad only
        /// </summary>
        /// <param name="gamepad">The GamePad handler</param>
        public InputMapper(GamepadHandler gamepadHandler)
        {
            this.keyboardHandler = null;
            this.gamepadHandler = gamepadHandler;
        }
        #endregion


        #region Gamepad Helpers
        /// <summary>
        /// Attaches a gamepad handler to the input mapper
        /// </summary>
        /// <param name="gamepadHandler">The Gamepad handler</param>
        public void AttachGamepad(GamepadHandler gamepadHandler)
        {
            this.gamepadHandler = gamepadHandler;
        }

        /// <summary>
        /// Detaches a gamepad handler from the input mapper
        /// </summary>
        public void DetachGamepad()
        {
            this.gamepadHandler = null;
        }
        #endregion


        #region Keyboard Helpers
        /// <summary>
        /// Attaches a keyboard handler to the input mapper
        /// </summary>
        /// <param name="keyboardHandler">The Keyboard handler</param>
        public void AttachKeyboard(KeyboardHandler keyboardHandler)
        {
            this.keyboardHandler = keyboardHandler;
        }

        /// <summary>
        /// Detaches a keyboard handler from the input mapper
        /// </summary>
        public void DetachKeyboard()
        {
            this.keyboardHandler = null;
        }
        #endregion


        #region Action helpers
        /// <summary>
        /// Gets an InputAction object by action name
        /// </summary>
        /// <param name="action">The name of the action to find</param>
        /// <returns>The InputAction object found</returns>
        public static InputAction GetAction(string action)
        {
            if (actions.ContainsKey(action))
            {
                return actions[action];
            }

            return null;
        }


        /// <summary>
        /// Adds an GamePad action to the mapper
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="button">The GamePad button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool AddAction(string action, Buttons button)
        {
            if (!actions.ContainsKey(action))
            {
                // Make sure the button hasn't already been applied
                foreach (KeyValuePair<string, InputAction> a in actions)
                {
                    if ((a.Value.Button & button) != 0)
                        return false;
                }

                actions.Add(action, new InputAction(action, button));
                return true;
            }

            return false;
        }

#if !XBOX
        /// <summary>
        /// Adds an Keyboard action to the mapper
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="key">The Keyboard button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool AddAction(string action, Keys key)
        {
            if (!actions.ContainsKey(action))
            {
                // Make sure the key hasn't already been applied
                foreach (KeyValuePair<string, InputAction> a in actions)
                {
                    if ((a.Value.Key & key) != 0)
                        return false;
                }

                actions.Add(action, new InputAction(action, key));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds an action to the mapper
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="button">The GamePad button that invokes the action</param>
        /// <param name="key">The Keyboard button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool AddAction(string action, Buttons button, Keys key)
        {
            if (!actions.ContainsKey(action))
            {
                // Make sure the key or button hasn't already been applied
                foreach (KeyValuePair<string, InputAction> a in actions)
                {
                    if ((a.Value.Key & key) != 0 || (a.Value.Button & button) != 0)
                        return false;
                }

                actions.Add(action, new InputAction(action, button, key));
                return true;
            }

            return false;
        }
#endif


        /// <summary>
        /// Edits an GamePad button on an action that already exists
        /// </summary>
        /// <param name="action">The action to edit</param>
        /// <param name="button">The GamePad button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool EditAction(string action, Buttons button)
        {
            if (actions.ContainsKey(action))
            {
                actions[action] = new InputAction(action, button, actions[action].Key);
                return true;
            }

            return false;
        }

#if !XBOX
        /// <summary>
        /// Edits an Keyboard button on an action that already exists
        /// </summary>
        /// <param name="action">The action to edit</param>
        /// <param name="key">The GamePad button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool EditAction(string action, Keys key)
        {
            if (actions.ContainsKey(action))
            {
                actions[action] = new InputAction(action, actions[action].Button, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Edits an action that already exists
        /// </summary>
        /// <param name="action">The action to edit</param>
        /// <param name="button">The GamePad button that invokes the action</param>
        /// <param name="key">The Keyboard button that invokes the action</param>
        /// <returns>True on success</returns>
        public static bool EditAction(string action, Buttons button, Keys key)
        {
            if (actions.ContainsKey(action))
            {
                actions[action] = new InputAction(action, button, key);
                return true;
            }

            return false;
        }
#endif


        /// <summary>
        /// Removes an action from the mapper
        /// </summary>
        /// <param name="action">The name of the action to remove</param>
        /// <returns>True on success</returns>
        public static bool RemoveAction(string action)
        {
            if (actions.ContainsKey(action))
                return actions.Remove(action);

            return false;
        }
        #endregion


        #region Main
        /// <summary>
        /// Updates the controllers attached to this input mapper 
        /// </summary>
        /// <param name="gameTime">The current GameTime object</param>
        public void ControllersUpdate(GameTime gameTime)
        {
            if (gamepadHandler != null)
                gamepadHandler.UpdateState(gameTime);

#if !XBOX
            // TODO: Find a clean way to make sure that keyboardHandler is only updated once a frame, not per InputMapper instance
            if (keyboardHandler != null)
                keyboardHandler.Update(gameTime);
#endif
        }

        /// <summary>
        /// Saves the current controller state for use in subsequent frames
        /// </summary>
        public void ControllersSaveState()
        {
            // Save the previous GamePad state
            if (gamepadHandler != null)
                gamepadHandler.SaveState();
        }

        /// <summary>
        /// Handles updates of event delegates attached to this input mapper
        /// </summary>
        /// <param name="gameTime">The current GameTime Object</param>
        public void UpdateEvents(GameTime gameTime)
        {

            // Loop through saved actions
            foreach (KeyValuePair<string, InputAction> action in actions)
            {
                // Check GamePad
                if (gamepadHandler != null && ActionEvent != null && action.Value.Button != null &&
                    gamepadHandler.GetButtonState((Buttons)action.Value.Button) == InputButtonState.Clicked)
                {
                    ActionEvent(action.Value);
                }

#if !XBOX
                // Check Keyboard
                if (keyboardHandler != null && ActionEvent != null && action.Value.Key != Keys.None &&
                    KeyboardHandler.GetKeyState(action.Value.Key) == InputButtonState.Clicked)
                {
                    ActionEvent(action.Value);
                }
#endif
            }
        }
        #endregion


        /// <summary>
        /// Gets the button state for a specific action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <param name="device">The input device to poll</param>
        /// <returns>The current button state</returns>
        public InputButtonState getActionState(string action, InputDeviceType device)
        {
            if (!actions.ContainsKey(action))
                throw new Exception("No action registered with the name '" + action + "'");

            switch (device)
            {
                case InputDeviceType.GamePad:
                    if (actions[action].Button == null)
                        throw new Exception("No GamePad button bound to the '" + action + "' action.");

                    if (gamepadHandler == null)
                        throw new Exception("No GamePad handler bound to this Input Mapper.");
                    
                    return gamepadHandler.GetButtonState((Buttons)actions[action].Button);

#if !XBOX
                case InputDeviceType.Keyboard:
                    if (actions[action].Key == Keys.None)
                        throw new Exception("No keyboard button bound to the '" + action + "' action.");

                    if (keyboardHandler == null)
                        throw new Exception("No Keyboard handler bound to this Input Mapper.");

                    return KeyboardHandler.GetKeyState(actions[action].Key);
#endif
            }

            // No input device found
            throw new Exception("No valid input device selected.");
        }


#if !XBOX
        /// <summary>
        /// Gets the keyboard button state for a specific action 
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>The button state</returns>
        public InputButtonState getActionStateKB(string action)
        {
            return getActionState(action, InputDeviceType.GamePad);
        }
#endif


        /// <summary>
        /// Gets the gamepad button state for a specific action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>The button state</returns>
        public InputButtonState getActionStateGP(string action)
        {
            return getActionState(action, InputDeviceType.GamePad);
        }
         
    }
}
