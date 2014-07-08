using MonoMinion.Graphics;
using MonoMinion.Collision;
using Microsoft.Xna.Framework;
using System;

namespace MonoMinion.TileEngine
{
    /// <summary>
    /// Used as a layer container for the Tile Map Engine
    /// </summary>
    public class MapLayer
    {
        private string _name;
        public string Name { get { return _name; } }

        private TileSheet _tilesheet;
        public TileSheet TileSheet { get { return _tilesheet; } }
        public Tile[][] Grid;
        public bool IsVisible;
        public float Depth;

        public int TileWidth { get { return tileWidth; } }
        protected int tileWidth;

        public int TileHeight { get { return tileHeight; } }
        protected int tileHeight;

        // Events
        public event TileEventHandler TileDestroyed;
        public delegate void TileEventHandler(MapLayer layer, TileEventArgs e);

        /// <summary>
        /// Tile Map Layer constructor
        /// </summary>
        /// <param name="name">Name of layer</param>
        /// <param name="mWidth">Map width</param>
        /// <param name="mHeight">Map height</param>
        /// <param name="depth">The depth of the layer</param>
        /// <param name="tilesheet">The tilesheet to be used</param>
        public MapLayer(string name, int mWidth, int mHeight, int tWidth, int tHeight, float depth, TileSheet tilesheet)
        {
            _name = name;

            // Prepare tile grid 
            Grid = new Tile[mWidth][];
            for (int i = 0; i < mWidth; i++)
            {
                Grid[i] = new Tile[mHeight];
            }
            Depth = depth;
            _tilesheet = tilesheet;
            IsVisible = true;

            tileWidth = tWidth;
            tileHeight = tHeight;
        }

        #region Tile Methods
        /// <summary>
        /// Attempts to add a tile at a specific index
        /// </summary>
        /// <param name="tile">Tile to add to the map</param>
        /// <param name="x">Grid index X</param>
        /// <param name="y">Grid index Y</param>
        /// <returns>True on success</returns>
        public bool AddTile(Tile tile, int x, int y)
        {
            if (Grid[x][y] == null)
            {
                Grid[x][y] = tile;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to create a tile at a specific index
        /// </summary>
        /// <param name="baseTile">The base tile sprite to use</param>
        /// <param name="x">Grid index X</param>
        /// <param name="y">Grid index Y</param>
        /// <param name="tw">Tile Width</param>
        /// <param name="th">Tile Height</param>
        /// <returns>True on success</returns>
        public bool CreateTile(int baseTile, int x, int y, int tw, int th)
        {
            if (Grid[x][y] == null)
            {
                Grid[x][y] = new Tile(baseTile, x, y, tw, th);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to create a tile at a specific index using a random tile from the specified group
        /// </summary>
        /// <param name="group">The terrain group</param>
        /// <param name="x">Grid index X</param>
        /// <param name="y">Grid index Y</param>
        /// <param name="isCollidable">Flag for whether tile is collidable or not (defaults to true)</param>
        /// <param name="tw">Tile Width</param>
        /// <param name="th">Tile Height</param>
        /// <returns>True on success</returns>
        public bool CreateTile(string group, int x, int y, int tw, int th, bool isCollidable = true)
        {
            if (Grid[x][y] == null)
            {
                Grid[x][y] = new Tile(_tilesheet.GetRandomFromGroup(group), x, y, tw, th);
                Grid[x][y].IsCollidable = isCollidable;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Changes the base tile for a specific grid position
        /// </summary>
        /// <param name="position">The position of the tile to change</param>
        /// <param name="group">The group name to change the tile into</param>
        /// <returns>True on success</returns>
        public bool ChangeTile(Point position, string group)
        {
            if (Grid[position.X][position.Y] != null)
            {
                Grid[position.X][position.Y].BaseTile = _tilesheet.GetRandomFromGroup(group);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Changes the base tile for a specific grid position
        /// </summary>
        /// <param name="x">The X position of the tile to change</param>
        /// <param name="y">The Y position of the tile to change</param>
        /// <param name="group">The group name to change the tile into</param>
        /// <returns>True on success</returns>
        public bool ChangeTile(int x, int y, string group)
        {
            if (Grid[x][y] != null)
            {
                Grid[x][y].BaseTile = _tilesheet.GetRandomFromGroup(group);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Gets the tiles collidable shape
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <returns>Returns the collidable SAT shape for the tile if found</returns>
        public SATShape GetTileBounds(int x, int y)
        {
            if (x > 0 && x < Grid.Length && y > 0 && y < Grid[x].Length && Grid[x][y] != null)
            {
                return Grid[x][y].CollidableShape;
            }

            return null;
        }
        #endregion


        public virtual void Update(GameTime gameTime)
        {
        }

        #region Event Handlers
        /// <summary>
        /// On Tile Destroyed event handler
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnTileDestroyed(TileEventArgs e)
        {
            if (TileDestroyed != null)
                TileDestroyed(this, e);
        }


        /// <summary>
        /// Destroys the given tile
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <param name="isRecursive">Flags whether to call the event handler connected to tile destruction, defaults to true</param>
        public virtual void DestroyTile(int x, int y, bool isRecursive = true)
        {
            Grid[x][y] = null;
            if (isRecursive)
                OnTileDestroyed(new TileEventArgs(x, y));
        }
        #endregion
    }
}
