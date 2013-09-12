using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMinion.GUI;

namespace MonoMinion.GUI.Controls
{
    /// <summary>
    /// Blank colored panel 
    /// </summary>
    public class Panel : Control
    {
        protected Texture2D backgroundTexture;
        protected Rectangle panelSize;

        /// <summary>
        /// Constructor for Panel
        /// </summary>
        /// <param name="name">Panel name</param>
        /// <param name="color">Panel color</param>
        /// <param name="size">Panel size</param>
        public Panel(string name, Color color, Rectangle size)
            : base(name, color)
        {
            this.panelSize = size;
            this.Position = new Vector2(size.X, size.Y);

            // Create a blank 5x5 texture
            this.backgroundTexture = new Texture2D(Minion.Instance.GraphicsDevice, 5, 5);
            Color[] colorArr = new Color[25];
            for (int i = 0; i < 25; i++)
                colorArr[i] = Color.White;
            this.backgroundTexture.SetData(colorArr);
        }

        /// <summary>
        /// Update override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            Minion.Instance.SpriteBatch.Draw(this.backgroundTexture, panelSize, BackgroundColor);
        }

        /// <summary>
        /// Handle Input override 
        /// </summary>
        public override void HandleInput()
        {

        }
    }
}
