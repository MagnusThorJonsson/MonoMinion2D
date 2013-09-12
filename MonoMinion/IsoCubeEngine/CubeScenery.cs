using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMinion.Graphics;

namespace MonoMinion.IsoCubeEngine
{
    public class CubeScenery
    {
        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;
        public const float DEPTH_MOD = 0.0000005f;

        #region Variables
        public string Name;

        public int Height;
        public int Width;
        public int Depth;

        private int _parentSizeModifier;
        private float _parentLayerDepth;
        public int MapIndexX;
        public int MapIndexY;
        public int MapIndexZ;

        // Cache
        private Vector2 _mapOffset;

        public SceneryCube[][][] Grid;
        public TileSheet Tilesheet;

        public bool IsCollidable;
        #endregion

        #region Constructor
        public CubeScenery(string name, int width, int height, int depth, int pSize, float pDepth, int iX, int iY, int iZ, TileSheet tilesheet)
        {
            Name = name;
            Width = width;
            Height = height;
            Depth = depth;

            // Parent cell offset
            _parentSizeModifier = pSize;
            _parentLayerDepth = pDepth;
            MapIndexX = iX;
            MapIndexY = iY;
            MapIndexZ = iZ;

            // Prepare block grid
            Grid = new SceneryCube[Width][][];
            for (int i = 0; i < Width; i++)
            {
                Grid[i] = new SceneryCube[Height][];
                for (int j = 0; j < Height; j++)
                    Grid[i][j] = new SceneryCube[Depth];
            }

            // TODO: Make sure to update when scenery is moved between cells
            // Map Index Offset
            _mapOffset = new Vector2(
                (MapIndexZ * (TILE_WIDTH * _parentSizeModifier) * 0.5f) + (MapIndexX * (TILE_WIDTH * _parentSizeModifier) * 0.5f),
                // +/- operands between axis transforms inverted due to +Y being down in XNA
                (MapIndexX * ((TILE_HEIGHT * _parentSizeModifier) * 0.5f) * 0.5f) - (MapIndexZ * ((TILE_HEIGHT * _parentSizeModifier) * 0.5f) * 0.5f) - (MapIndexY * (TILE_HEIGHT * _parentSizeModifier) * 0.5f)
            );

            Tilesheet = tilesheet;
            IsCollidable = true;
        }
        #endregion

        #region Grid Functions
        /// <summary>
        /// Creates and places a SceneryCube at a specific index in the map Grid
        /// </summary>
        /// <param name="id">The Cube identifier</param>
        /// <param name="tile">Id of the tilesheet index to be used</param>
        /// <param name="x">Grid index width axis</param>
        /// <param name="y">Grid index height axis</param>
        /// <param name="z">Grid index depth axis</param>
        /// <param name="doOverride">Whether to override the current SceneryCube at the given index (defaults to true)</param>
        /// <returns>True on success</returns>
        public bool CreateCube(string id, int tile, int x, int y, int z, bool doOverride = true)
        {
            if ((x >= 0 && x < Width) &&
                (y >= 0 && y < Height) &&
                (z >= 0 && z < Depth))
            {
                // Make sure that we don't override a grid space if override is off
                if (doOverride == false && Grid[x][y][z] != null)
                    return false;

                SceneryCube cube = new SceneryCube(id, tile, x, y, z);
                // Cache position and layer depth
                cube.Position = new Vector2(
                    (z * TILE_WIDTH * 0.5f) + (x * TILE_WIDTH * 0.5f),
                    // +/- operands between axis transforms inverted due to +Y being down in XNA
                    (x * (TILE_HEIGHT * 0.5f) * 0.5f) - (z * (TILE_HEIGHT * 0.5f) * 0.5f) - (y * TILE_HEIGHT * 0.5f)
                );
                cube.LayerDepth = (x + (y + 1) + (Depth - 1 - z)) * DEPTH_MOD;
                Grid[x][y][z] = cube;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Adds a SceneryCube that has already been instantiated to the Grid
        /// </summary>
        /// <param name="cube">The SceneryCube to add to the Grid</param>
        /// <param name="doOverride">Whether to override the current SceneryCube at the given index (defaults to true)</param>
        /// <returns>True on success</returns>
        public bool AddCube(SceneryCube cube, bool doOverride = true)
        {
            if ((cube.X >= 0 && cube.X < Width) &&
                (cube.Y >= 0 && cube.Y < Height) &&
                (cube.Z >= 0 && cube.Z < Depth))
            {
                // Make sure that we don't override a grid space if override is off
                if (doOverride == false && Grid[cube.X][cube.Y][cube.Z] != null)
                    return false;

                Grid[cube.X][cube.Y][cube.Z] = cube;
                // Cache position and layer depth
                cube.Position = new Vector2(
                    (cube.Z * TILE_WIDTH * 0.5f) + (cube.X * TILE_WIDTH * 0.5f),
                    // +/- operands between axis transforms inverted due to +Y being down in XNA
                    (cube.X * (TILE_HEIGHT * 0.5f) * 0.5f) - (cube.Z * (TILE_HEIGHT * 0.5f) * 0.5f) - (cube.Y * TILE_HEIGHT * 0.5f)
                );
                cube.LayerDepth = (cube.X + (cube.Y + 1) + (Depth - 1 - cube.Z)) * DEPTH_MOD;

                return true;
            }

            return false;
        }
        #endregion

        #region Update & Draw
        public void Draw(GameTime gameTime)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = Depth - 1; z >= 0; z--)
                    {
                        // TODO: Remove the ContainsKey check at first possible opportunity
                        // If the tile is visible and the texture isn't wacky we draw
                        if (Grid[x][y][z] != null && Grid[x][y][z].IsVisible)// && Tilesheet.Tiles.ContainsKey(Grid[x][y][z].BaseTile))
                        {
                            // Draw Tile
                            Minion.Instance.SpriteBatch.Draw(
                                Tilesheet.SpriteSheet,                      // Texture
                                _mapOffset + Grid[x][y][z].Position,        // Position
                                Tilesheet.Tiles[Grid[x][y][z].BaseTile],    // Source Rect
                                Grid[x][y][z].Tint,                         // Color
                                0,                                          // Rotation
                                Vector2.Zero,                               // Origin
                                1f,                                         // Scale
                                SpriteEffects.None,                         // Sprite Effects
                                // Layer Depth
                                _parentLayerDepth + Grid[x][y][z].LayerDepth + DEPTH_MOD
                            );

                        }

                    }
                }
            }
        }
        #endregion
    }
}
