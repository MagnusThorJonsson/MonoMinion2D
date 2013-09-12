using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoMinion.Handlers;

namespace MonoMinion.GUI.Controls
{
    /// <summary>
    /// A clickable label button
    /// </summary>
    public class LabelButton : Label
    {
        #region Variables and Properties
        public Color SelectedColor { get; set; }
        #endregion

        /// <summary>
        /// A clickable label button
        /// </summary>
        /// <param name="name">Name of the label button</param>
        /// <param name="color">Color of the label font</param>
        /// <param name="selectedColor">Selected label font color</param>
        public LabelButton(string name, Color color, Color selectedColor)
            : base(name, color)
        {
            this.IsStatic = false;
            this.SelectedColor = selectedColor;
        }

        /// <summary>
        /// Draw the label button
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            if (this.HasFocus)
                Minion.Instance.SpriteBatch.DrawString(Font, Text, Position, SelectedColor);
            else
                Minion.Instance.SpriteBatch.DrawString(Font, Text, Position, BackgroundColor);
        }


        /// <summary>
        /// Handle label button clicks
        /// </summary>
        public override void HandleInput()
        {
            if (!this.HasFocus)
                return;

            if (KeyboardHandler.KeyReleased(Keys.Enter))
                base.OnSelected(null);
        }
    }
}