using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoMinion.Input.enums;
using MonoMinion.Input.Handlers;

namespace MonoMinion.Input
{
    /// <summary>
    /// Handles mapping of actions to Keyboard and GamePad input
    /// </summary>
    public class InputMapper
    {
        #region Variables & Properties
        private static Dictionary<string, InputAction> actions = new Dictionary<string, InputAction>();

        private GamepadHandler gamepadHandler;
        #endregion


        #region Constructors
#if !XBOX
        /// <summary>
        /// Only used for keyboards
        /// </summary>
        public InputMapper()
        {
            this.gamepadHandler = null;
        }
#endif

        /// <summary>
        /// Keyboard and Gamepad
        /// </summary>
        /// <param name="gamepad">The GamePad handler</param>
        public InputMapper(GamepadHandler gamepadHandler)
        {
            this.gamepadHandler = gamepadHandler;
        }
        #endregion


        #region Gamepad Helpers
        /// <summary>
        /// Attaches a gamepad handler to the input mapper
        /// </summary>
        /// <param name="gamepad">The Gamepad handler</param>
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


        #region Action helpers
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
                    if (a.Value.Button == button)
                        return false;
                }

                actions.Add(action, new InputAction(button));
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
                    if (a.Value.Key == key)
                        return false;
                }

                actions.Add(action, new InputAction(key));
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
                    if (a.Value.Key == key || a.Value.Button == button)
                        return false;
                }

                actions.Add(action, new InputAction(button, key));
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
                actions[action] = new InputAction(button, actions[action].Key);
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
                actions[action] = new InputAction(actions[action].Button, key);
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
                actions[action] = new InputAction(button, key);
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




        /// <summary>
        /// Gets the button state for a specific action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <param name="device">The input device to poll</param>
        /// <returns>The current button state</returns>
        public InputButtonState getState(string action, InputDeviceType device)
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

                    return KeyboardHandler.GetKeyState(actions[action].Key);
#endif
            }

            // No input device found
            throw new Exception("No valid input device selected.");
        }
         
    }
}
