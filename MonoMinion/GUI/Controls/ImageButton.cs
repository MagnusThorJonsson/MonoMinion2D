using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMinion.Graphics.Sprites;
using MonoMinion.Input.Handlers;

namespace MonoMinion.GUI.Controls
{
    public class ImageButton : ImageBox
    {
        protected bool isOn;
        public virtual bool IsOn
        {
            get { return isOn; }
            set
            {
                isOn = value;
                if (image != null)
                {
                    if (isOn)
                        image.SetAnimation("On");
                    else
                        image.SetAnimation("Off");
                }
            }
        }

        public ImageButton(string name, Sprite image, Color tint)
            : base(name, image, tint)
        {
            IsStatic = false;
            IsOn = false;
            image.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void HandleInput()
        {
            if (!this.HasFocus)
                return;

            if (MouseHandler.IsMouseUp(MouseHandler.Buttons.Left) && BoundingBox.Intersects(MouseHandler.PositionRect))
            {
                IsOn = !IsOn;
                base.OnSelected(null);
            }
        }
    }
}
