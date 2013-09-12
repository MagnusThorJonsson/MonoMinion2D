using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMinion.GUI;
using MonoMinion.Graphics.Sprites;

namespace MonoMinion.GUI.Controls
{
    public class ImageBox : Control
    {
        #region Variables and Properties
        protected Sprite image;
        public override Vector2 Position
        {
            get
            {
                if (image != null)
                    return image.Position;

                return Vector2.Zero;
            }
            set
            {
                if (image != null)
                    image.Position = value;

                position = value;
            }
        }
        #endregion

        #region Constructors
        public ImageBox(string name, Sprite image, Color tint)
            : base(name, tint)
        {
            this.image = image;
            this.Size = new Point(image.Width, image.Height);
        }
        #endregion


        #region Overrideable Functions
        public override void Update(GameTime gameTime)
        {
            image.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            image.Draw(gameTime);
        }

        public override void HandleInput() { }
        #endregion
    }
}
