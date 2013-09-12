using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics
{
    /// <summary>
    /// Contains a spritesheet to be used by Sprites
    /// </summary>
    public class SpriteSheet
    {
        public string Name;
        public Point Size;
        public Texture2D Texture;

        /// <summary>
        /// Constructs a spritesheet
        /// </summary>
        /// <param name="name">Name of the spritesheet</param>
        /// <param name="texture">The texture that is the base of the spritesheet</param>
        public SpriteSheet(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
            Size = new Point(texture.Width, texture.Height);
        }
    }
}
