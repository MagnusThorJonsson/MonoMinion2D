using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMinion.Graphics;

namespace MonoMinion.TileEngine
{
    /// <summary>
    /// Struct for culling the tilemap
    /// </summary>
    public struct MapCulling
    {
        public int Top, Bottom, Left, Right;

        public MapCulling(int t, int b, int l, int r)
        {
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }
    }

    /// <summary>
    /// Main class for the Tile Engine the Tile Map is used to contain and render map data
    /// </summary>
    public class TileMap
    {
        public static readonly int BITWISE_TOP = 1;
        public static readonly int BITWISE_RIGHT = 2;
        public static readonly int BITWISE_BOTTOM = 4;
        public static readonly int BITWISE_LEFT = 8;

        protected string name;
        public string Name { get { return name; } }
        protected MapLayer[] layers;
        public MapCulling Culling;
        public Texture2D Background;

        #region Map Size variables
        private int _mapWidth;
        public int MapWidth { get { return _mapWidth; } }
        private int _mapHeight;
        public int MapHeight { get { return _mapHeight; } }

        private int _tileWidth;
        public int TileWidth { get { return _tileWidth; } }
        private int _tileHeight;
        public int TileHeight { get { return _tileHeight; } }
        #endregion

        
        #region Constructors
        /// <summary>
        /// Creates a tilemap
        /// </summary>
        /// <param name="name">The name of the map</param>
        /// <param name="mWidth">The width of the map in tiles</param>
        /// <param name="mHeight">The height of the map in tiles</param>
        /// <param name="tWidth">The base tile width</param>
        /// <param name="tHeight">The base tile height</param>
        /// <param name="layers">The number of layers</param>
        /// <param name="background">The background texture if any (defaults to null)</param>
        public TileMap(string name, int mWidth, int mHeight, int tWidth, int tHeight, int layers, Texture2D background = null)
        {
            this.name = name;
            _mapWidth = mWidth;
            _mapHeight = mHeight;
            _tileWidth = tWidth;
            _tileHeight = tHeight;

            this.layers = new MapLayer[layers];
            Background = background;

            Culling = new MapCulling(0, mHeight - 1, 0, mWidth - 1);
        }
        #endregion


        #region Map Layer Helpers
        /// <summary>
        /// Gets a specific layer by name
        /// </summary>
        /// <param name="name">Layer name</param>
        /// <returns>A MapLayer if found</returns>
        public MapLayer GetLayer(string name)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].Name == name)
                    return layers[i];
            }

            return null;
        }

        /// TODO: This function will probably never be used
        /// <summary>
        /// Gets a specific layer index by layer name
        /// </summary>
        /// <param name="name">Layer name</param>
        /// <returns>The index position in the Layer array</returns>
        public int GetLayerIndex(string name)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].Name == name)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Attempts to add a layer at a specific index in the Layer array
        /// </summary>
        /// <param name="name">Name of the layer</param>
        /// <param name="depth">The depth of the layer</param>
        /// <param name="tilesheet">The tilesheet to be used for the layer</param>
        /// <param name="index">The position in the layer array</param>
        /// <returns>true on success</returns>
        public bool AddLayer(string name, float depth, TileSheet tilesheet, int index)
        {
            if (layers[index] == null)
            {
                layers[index] = new MapLayer(name, _mapWidth, _mapHeight, _tileWidth, _tileHeight, depth, tilesheet);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a layer at a specific index in the Layer array
        /// </summary>
        /// <param name="layer">The map layer to add</param>
        /// <param name="index">The position in the layer array</param>
        /// <returns>true on success</returns>

        public bool AddLayer(MapLayer layer, int index)
        {
            if (layers[index] == null)
            {
                layers[index] = layer;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new layer at the first empty index in the layer array
        /// </summary>
        /// <param name="name">Name of the layer</param>
        /// <param name="depth">The depth of the layer</param>
        /// <param name="tilesheet">The tilesheet to be used for the layer</param>
        /// <returns>true on success</returns>
        public bool AddLayer(string name, float depth, TileSheet tilesheet)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] == null)
                {
                    layers[i] = new MapLayer(name, _mapWidth, _mapHeight, _tileWidth, _tileHeight, depth, tilesheet);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new layer at the first empty index in the layer array
        /// </summary>
        /// <param name="layer">The layer to add</param>
        /// <returns>true on success</returns>
        public bool AddLayer(MapLayer layer)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] == null)
                {
                    layers[i] = layer;
                    return true;
                }
            }

            return false;
        }
        #endregion


        #region Tile Helpers
        /// <summary>
        /// Adds a tile to a specific layer
        /// </summary>
        /// <param name="layer">The layer to add to</param>
        /// <param name="tile">The tile to add</param>
        /// <param name="x">The tile position on the X axis</param>
        /// <param name="y">The tile position on the Y axis</param>
        /// <returns></returns>
        public bool AddTile(int layer, Tile tile, int x, int y)
        {
            if (layer > layers.Length - 1)
                return false;

            return layers[layer].AddTile(tile, x, y);
        }


        /// <summary>
        /// Gets the position index for the tile that corrolates to the world location vector passed in
        /// </summary>
        /// <param name="position">World location position vector</param>
        /// <returns>Point that contains the position of the tile found at the position vector</returns>
        public Point GetTilePosition(Vector2 position)
        {
            Point pos = new Point(
                (int)(position.X / this._tileWidth),
                (int)(position.Y / this._tileHeight)
            );

            if (pos.X < 0) pos.X = 0;
            if (pos.X > _mapWidth - 1) pos.X = _mapWidth - 1;
            if (pos.Y < 0) pos.Y = 0;
            if (pos.Y > _mapHeight - 1) pos.Y = _mapHeight - 1;

            return pos;
        }

        /// <summary>
        /// Gets the tile that corrolates to the world location vector passed in
        /// </summary>
        /// <param name="layer">The index of the layer in the Layers array to get the tile from</param>
        /// <param name="position">World location position vector</param>
        /// <returns>The tile at the given world position</returns>
        public Tile GetTile(int layer, Vector2 position)
        {
            if (layer < layers.Length)
                return layers[layer].Grid[(int)Math.Floor(position.X / this._tileWidth)][(int)Math.Floor(position.Y / this._tileHeight)];

            return null;
        }


        /// <summary>
        /// Gets the tile that corrolates to the world location vector passed in
        /// </summary>
        /// <param name="layer">The name of the layer to get the tile from</param>
        /// <param name="position">World location position vector</param>
        /// <returns>The tile at the given world position</returns>
        public Tile GetTile(string layer, Vector2 position)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].Name == layer)
                    return layers[i].Grid[(int)Math.Floor(position.X / this._tileWidth)][(int)Math.Floor(position.Y / this._tileHeight)];
            }

            return null;
        }
        #endregion


        #region Update & Draw
        /// <summary>
        /// Updates the Tile Map
        /// </summary>
        /// <param name="gameTime">The current gametime object</param>
        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < layers.Length; i++)
                if (layers[i] != null)
                    layers[i].Update(gameTime);
        }


        /// <summary>
        /// Renders the Tile Map
        /// </summary>
        /// <param name="gameTime">The current gametime object</param>
        public virtual void Draw(GameTime gameTime)
        {
            // Background
            if (Background != null)
                Minion.Instance.SpriteBatch.Draw(
                    Background,
                    new Rectangle(0, 0, (_mapWidth * _tileWidth), (_mapHeight * _tileWidth)),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1.0f
                );

            // Tiles
            for (int l = layers.Length - 1; l >= 0; l--)
            {
                // Make sure the layer isn't empty
                if (layers[l] != null)
                {
                    for (int x = Culling.Left; x <= Culling.Right; x++)
                    {
                        for (int y = Culling.Top; y <= Culling.Bottom; y++)
                        {
                            // Make sure the grid position isn't empty and that the tile is visible
                            if (layers[l].Grid[x][y] != null && layers[l].Grid[x][y].IsVisible)
                            {
                                // TODO: Fix position (remove calculation per frame)
                                // Draw Tile
                                Minion.Instance.SpriteBatch.Draw(
                                    layers[l].TileSheet.Spritesheet,                            // Texture
                                    new Vector2(x * _tileWidth, y * _tileHeight),               // Position
                                    layers[l].TileSheet.Tiles[layers[l].Grid[x][y].BaseTile],   // Source Rect
                                    layers[l].Grid[x][y].Tint,                                  // Color
                                    0,                                                          // Rotation
                                    Vector2.Zero,                                               // Origin
                                    1f,                                                         // Scale
                                    SpriteEffects.None,                                         // Sprite Effects
                                    layers[l].Depth                                             // Layer Depth
                                );
                            }
                        }
                    }
                } // End IF layer
            }
        }
        #endregion

    }
}
