using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoMinion.Input
{
    public sealed class InputAction
    {
        #region Variables & Properties
        private string name;
        public string Name { get { return name; } }

        private Buttons? button;
        public Buttons? Button { get { return button; } }

        private Keys key;
        public Keys Key { get { return key; } }
        #endregion


        #region Constructors
        /// <summary>
        /// Create an Input Action, connected to a GamePad button, for use by the InputMapper.
        /// </summary>
        /// <param name="name">The name of the InputAction</param>
        /// <param name="button">GamePad button that triggers action</param>
        public InputAction(string name, Buttons? button)
        {
            this.name = name;
            this.button = button;
            key = Keys.None;
        }


        /// <summary>
        /// Create an Input Action, connected to a GamePad & Keyboard button, for use by the InputMapper.
        /// </summary>
        /// <param name="name">The name of the InputAction</param>
        /// <param name="button">GamePad button that triggers action</param>
        /// <param name="key">Keyboard button that triggers action</param>
        public InputAction(string name, Buttons? button, Keys key)
        {
            this.name = name;
            this.button = button;
            this.key = key;
        }


        /// <summary>
        /// Create an Input Action, connected to a Keyboard button, for use by the InputMapper.
        /// </summary>
        /// <param name="name">The name of the InputAction</param>
        /// <param name="key">Keyboard button that triggers action</param>
        public InputAction(string name, Keys key)
        {
            this.name = name;
            this.key = key;
            button = null;
        }
        #endregion
    }
}
