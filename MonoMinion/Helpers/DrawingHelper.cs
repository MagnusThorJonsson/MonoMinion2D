using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Helpers
{
    public class DrawingHelper
    {
        /// <summary>
        /// Draws a rectangular outline
        /// </summary>
        /// <param name="rect">Dimension and size of the rectangle to be drawn</param>
        /// <param name="borderWidth">The border size</param>
        /// <param name="color">The color of the border</param>
        public static void DrawRectangle(Rectangle rect, int borderWidth, Color color)
        {
            Minion.Instance.SpriteBatch.Draw(Minion.Instance.Texture1x1, new Rectangle(rect.Left, rect.Top, borderWidth, rect.Height), color); // Left
            Minion.Instance.SpriteBatch.Draw(Minion.Instance.Texture1x1, new Rectangle(rect.Right, rect.Top, borderWidth, rect.Height), color); // Right
            Minion.Instance.SpriteBatch.Draw(Minion.Instance.Texture1x1, new Rectangle(rect.Left, rect.Top, rect.Width, borderWidth), color); // Top
            Minion.Instance.SpriteBatch.Draw(Minion.Instance.Texture1x1, new Rectangle(rect.Left, rect.Bottom, rect.Width + borderWidth, borderWidth), null, color, 0, Vector2.Zero, SpriteEffects.None, 0f); // Bottom
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="from">Start vector</param>
        /// <param name="to">End vector</param>
        /// <param name="borderWidth">The width of the border</param>
        /// <param name="color">Color of the line</param>
        public static void DrawLine(Vector2 from, Vector2 to, int borderWidth, Color color)
        {
            float angle = (float)System.Math.Atan2(to.Y - from.Y, to.X - from.X);
            float length = Vector2.Distance(from, to);

            Minion.Instance.SpriteBatch.Draw(
                Minion.Instance.Texture1x1,
                from,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, borderWidth),
                SpriteEffects.None,
                0
            );
        }


        /// <summary>
        /// Crops a part of an image
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="area">Area to crop</param>
        /// <returns>Cropped part of the image</returns>
        public static Texture2D Crop(Texture2D source, Rectangle area)
        {
            if (source == null)
                return null;

            Texture2D cropped = new Texture2D(source.GraphicsDevice, area.Width, area.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] cropData = new Color[cropped.Width * cropped.Height];

            source.GetData<Color>(data);

            int index = 0;
            for (int y = area.Y; y < area.Y + area.Height; y++)
            {
                for (int x = area.X; x < area.X + area.Width; x++)
                {
                    cropData[index] = data[x + (y * source.Width)];
                    index++;
                }
            }

            cropped.SetData<Color>(cropData);

            return cropped;
        }
    }
}
