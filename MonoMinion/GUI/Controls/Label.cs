using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMinion.GUI;

namespace MonoMinion.GUI.Controls
{
    /// <summary>
    /// Text Label control
    /// </summary>
    public class Label : Control
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Vector2 size = Font.MeasureString(_text);
                Size = new Point((int)size.X, (int)size.Y);
            }
        }

        /// <summary>
        /// Constructor for Text Label
        /// </summary>
        /// <param name="name">Name of the label</param>
        /// <param name="color">Font color</param>
        public Label(string name, Color color)
            : base(name, color)
        {
            TabStop = false;
        }

        public override void Update(GameTime gameTime) { }

        /// <summary>
        /// Draw Label
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            Minion.Instance.SpriteBatch.DrawString(Font, Text, Position, BackgroundColor);
            //Minion.Instance.SpriteBatch.DrawString(Font, Text, Position, BackgroundColor, 0f, Vector2.Zero, 1.5f, new SpriteEffects(), 0f);
        }

        public override void HandleInput() { }
    }
}
