using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics
{
    public static class SpriteSheetManager
    {
        private static Dictionary<string, SpriteSheet> spritesheets = new Dictionary<string, SpriteSheet>();

        public static SpriteSheet Spritesheet(string name, string path)
        {
            // if it already exists we return the loaded one
            if (spritesheets.ContainsKey(name))
                return spritesheets[name];

            spritesheets[name] = new SpriteSheet(name, Minion.Instance.Content.Load<Texture2D>(path + name));
            return spritesheets[name];
        }
    }
}
