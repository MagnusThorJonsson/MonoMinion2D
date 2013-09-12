using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoMinion.Components
{
    public class FrameRateCounter : DrawableGameComponent
    {
        ContentManager content;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FrameRateCounter(Game game)
            : base(game)
        {
            content = Minion.Instance.Content;
        }


        protected override void LoadContent()
        {
            spriteFont = content.Load<SpriteFont>("UI\\Fonts\\FpsFont");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            spriteFont = null;
            content = null;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("FPS:{0}", frameRate);

            Minion.Instance.SpriteBatch.Begin();

            Minion.Instance.SpriteBatch.DrawString(spriteFont, fps, new Vector2(10, 5), Color.Black);
            Minion.Instance.SpriteBatch.DrawString(spriteFont, fps, new Vector2(9, 4), Color.White);

            Minion.Instance.SpriteBatch.End();
        }
    }
}
