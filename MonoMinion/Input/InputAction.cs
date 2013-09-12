using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoMinion.Input
{
    public sealed class InputAction
    {
        #region Variables & Properties
        private Buttons? button;
        public Buttons? Button { get { return button; } }

        private Keys key;
        public Keys Key { get { return key; } }
        #endregion


        #region Constructors
        public InputAction(Buttons? button)
        {
            this.button = button;
            key = Keys.None;
        }

        public InputAction(Buttons? button, Keys key)
        {
            this.button = button;
            this.key = key;
        }

        public InputAction(Keys key)
        {
            this.key = key;
            button = null;
        }
        #endregion
    }
}
