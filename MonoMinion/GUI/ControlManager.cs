using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoMinion.Helpers;
using MonoMinion.Input.Handlers;

namespace MonoMinion.GUI
{
    /// <summary>
    /// Control Manager for GUI
    /// </summary>
    public class ControlManager : List<Control>
    {
        #region Variables and Properties
        private int _selected = 0;
        public static SpriteFont SpriteFont { get; set; }

        public event EventHandler FocusChanged;
        #endregion

        /// <summary>
        /// Base constructor for the Control Manager
        /// </summary>
        /// <param name="spriteFont">Main font to be used</param>
        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            ControlManager.SpriteFont = spriteFont;
        }

        /// <summary>
        /// Numerable constructor for the Control Manager
        /// </summary>
        /// <param name="spriteFont">Main font to be used</param>
        /// <param name="collection">IEnumberable collection of controls</param>
        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection)
            : base(collection)
        {
            ControlManager.SpriteFont = spriteFont;
        }

        #region Main Functions
        /// <summary>
        /// Update all controls
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.Count == 0)
                return;

            foreach (Control control in this)
            {
                if (!control.IsStatic)
                {
                    if (control.IsEnabled)
                        control.Update(gameTime);

                    if (control.HasFocus)
                        control.HandleInput();

                    // If either mouse button is down
                    if (MouseHandler.IsMouseDown(MouseHandler.Buttons.Left) || MouseHandler.IsMouseDown(MouseHandler.Buttons.Right))
                    {
                        // Check for intersect
                        if (control.BoundingBox.Intersects(MouseHandler.PositionRect))
                        {
                            // If this control doesn't already have focus
                            if (!control.HasFocus)
                            {
                                // Clear focus for every control
                                foreach (Control cntrl in this)
                                    cntrl.HasFocus = false;
                                // Set focus and handle input for this click (workaround for first click not registering)
                                control.HasFocus = true;
                                control.HandleInput();
                            }
                        }
                    }
                }
            }

            if (KeyboardHandler.KeyPressed(Keys.Up))
                Previous();
            if (KeyboardHandler.KeyPressed(Keys.Down))
                Next();
        }


        /// <summary>
        /// Draws all controls
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Draw(GameTime gameTime)
        {
            foreach (Control control in this)
            {
                if (control.IsVisible)
                    control.Draw(gameTime);
            }
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Next control in list
        /// </summary>
        public void Next()
        {
            if (Count == 0)
                return;

            int currentControl = _selected;
            this[_selected].HasFocus = false;
            do
            {
                _selected++;
                if (_selected == Count)
                    _selected = 0;

                if (!this[_selected].IsStatic && this[_selected].TabStop && this[_selected].IsEnabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selected], null);
                    break;
                }

            } while (currentControl != _selected);
            this[_selected].HasFocus = true;
        }


        /// <summary>
        /// Previous control in list
        /// </summary>
        public void Previous()
        {
            if (Count == 0)
                return;

            int currentControl = _selected;
            this[_selected].HasFocus = false;
            do
            {
                _selected--;
                if (_selected < 0)
                    _selected = Count - 1;

                if (!this[_selected].IsStatic && this[_selected].TabStop && this[_selected].IsEnabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selected], null);
                    break;
                }

            } while (currentControl != _selected);
            this[_selected].HasFocus = true;
        }
        #endregion
    }
}
