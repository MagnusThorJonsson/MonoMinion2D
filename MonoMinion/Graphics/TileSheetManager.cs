using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics
{
    public static class TileSheetManager
    {
        private static Dictionary<string, TileSheet> tilesheets = new Dictionary<string, TileSheet>();

        public static TileSheet Tilesheet(string name)
        {
            // if it already exists we return the loaded one
            if (tilesheets.ContainsKey(name))
                return tilesheets[name];

            tilesheets[name] = new TileSheet(name, Minion.Instance.Content.Load<Texture2D>("Graphics\\Tilesheets\\" + name));
            return tilesheets[name];
        }
    }
}
