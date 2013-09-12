using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics
{
    public class TileSheet
    {
        #region Variables
        public string Name;
        public Texture2D SpriteSheet;
        public List<Rectangle> Tiles;
        public Dictionary<string, int> TileKeys;
        public Dictionary<string, List<int>> TileGroups;
        private Random _random;
        #endregion

        /// <summary>
        /// A tilesheet class is used as a container for the tiles used by the tilemap
        /// </summary>
        /// <param name="name">The name of the tilesheet</param>
        /// <param name="texture">The texture that this tilesheet will use</param>
        /// <param name="tileCount">Number of tiles in the tilesheet</param>
        public TileSheet(string name, Texture2D texture)//, int tileCount)
        {
            Name = name;
            SpriteSheet = texture;
            Tiles = new List<Rectangle>();
            TileKeys = new Dictionary<string, int>();
            TileGroups = new Dictionary<string, List<int>>();
            _random = new Random();
        }

        /// <summary>
        /// Adds a tile to the tilesheet
        /// </summary>
        /// <param name="key">Key to associate with tile</param>
        /// <param name="frame">Frame location for the tile</param>
        public void AddTile(string key, Rectangle frame)
        {
            Tiles.Add(frame);
            TileKeys[key] = Tiles.Count - 1;
        }

        /// <summary>
        /// Adds a tile to the tilesheet and associates it with a group
        /// </summary>
        /// <param name="key">Key to associate with tile</param>
        /// <param name="group">The group to add the tile to</param>
        /// <param name="frame">Frame location for the tile</param>
        /// <returns>True if adding to group was successful</returns>
        public bool AddTile(string key, string group, Rectangle frame)
        {
            Tiles.Add(frame);
            TileKeys[key] = Tiles.Count - 1;

            // Add to group
            return GroupTile(key, group);
        }

        /// <summary>
        /// Adds a tile to a group
        /// </summary>
        /// <param name="key">Tile to add</param>
        /// <param name="group">Group to add to</param>
        /// <returns>True on success</returns>
        public bool GroupTile(string key, string group)
        {
            // Make sure tile key exists
            if (TileKeys.ContainsKey(key))
            {
                // Create the tile group if it doesn't exist already
                if (!TileGroups.ContainsKey(group))
                    TileGroups[group] = new List<int>();

                // Make sure TileGroup doesn't already contain tile
                if (!TileGroups[group].Contains(TileKeys[key]))
                {
                    TileGroups[group].Add(TileKeys[key]);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a random tile index from the specified group
        /// </summary>
        /// <param name="group">Group to get random tile from</param>
        /// <returns>A tile index</returns>
        public int GetRandomFromGroup(string group)
        {
            return TileGroups[group][_random.Next(0, TileGroups[group].Count - 1)];
        }
    }
}
