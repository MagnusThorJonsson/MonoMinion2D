using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMinion.Graphics;

namespace MonoMinion.IsoCubeEngine
{
    public class CubeMap
    {
        public const int TILE_WIDTH = 64;
        public const int TILE_HEIGHT = 64;
        public const float DEPTH_MOD = 0.000001f;

        #region Variables
        public string Name;

        public int Height;
        public int Width;
        public int Depth;

        //private TileMapSketch mapSketch;

        public MapCube[][][] Grid;
        public List<CubeScenery> Scenery;
        public TileSheet Tilesheet;

        public Rectangle ScreenCulling;
        #endregion


        #region Constructor
        public CubeMap(string name, int width, int height, int depth, TileSheet tilesheet)
        {
            Name = name;
            Width = width;
            Height = height;
            Depth = depth;

            // Prepare block grid
            Grid = new MapCube[Width][][];
            for (int i = 0; i < Width; i++)
            {
                Grid[i] = new MapCube[Height][];
                for (int j = 0; j < Height; j++)
                    Grid[i][j] = new MapCube[Depth];
            }
            Scenery = new List<CubeScenery>();

            ScreenCulling = new Rectangle();

            Tilesheet = tilesheet;
        }
        #endregion


        #region Grid Functions
        /// <summary>
        /// Creates and places a TileCube at a specific index in the map Grid
        /// </summary>
        /// <param name="id">The Cube identifier</param>
        /// <param name="tile">Id of the tilesheet index to be used</param>
        /// <param name="x">Grid index width axis</param>
        /// <param name="y">Grid index height axis</param>
        /// <param name="z">Grid index depth axis</param>
        /// <param name="doOverride">Whether to override the current TileCube at the given index (defaults to true)</param>
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

                MapCube cube = new MapCube(id, tile, x, y, z);
                // Cache position and layer depth
                cube.Position = new Vector2(
                    (z * TILE_WIDTH * 0.5f) + (x * TILE_WIDTH * 0.5f),
                    // +/- operands between axis transforms inverted due to +Y being down in XNA
                    (x * (TILE_HEIGHT * 0.5f) * 0.5f) - (z * (TILE_HEIGHT * 0.5f) * 0.5f) - (y * TILE_HEIGHT * 0.5f)
                );
                cube.LayerDepth = (x + y + (Depth - 1 - z)) * DEPTH_MOD;
                Grid[x][y][z] = cube;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Adds a TileCube that has already been instantiated to the Grid
        /// </summary>
        /// <param name="cube">The TileCube to add to the Grid</param>
        /// <param name="doOverride">Whether to override the current TileCube at the given index (defaults to true)</param>
        /// <returns>True on success</returns>
        public bool AddCube(MapCube cube, bool doOverride = true)
        {
            if ((cube.X >= 0 && cube.X < Width) &&
                (cube.Y >= 0 && cube.Y < Height) &&
                (cube.Z >= 0 && cube.Z < Depth))
            {
                // Make sure that we don't override a grid space if override is off
                if (doOverride == false && Grid[cube.X][cube.Y][cube.Z] != null)
                    return false;

                // Cache position and layer depth
                cube.Position = new Vector2(
                    (cube.Z * TILE_WIDTH * 0.5f) + (cube.X * TILE_WIDTH * 0.5f),
                    // +/- operands between axis transforms inverted due to +Y being down in XNA
                    (cube.X * (TILE_HEIGHT * 0.5f) * 0.5f) - (cube.Z * (TILE_HEIGHT * 0.5f) * 0.5f) - (cube.Y * TILE_HEIGHT * 0.5f)
                );
                cube.LayerDepth = (cube.X + cube.Y + (Depth - 1 - cube.Z)) * DEPTH_MOD;
                Grid[cube.X][cube.Y][cube.Z] = cube;

                return true;
            }

            return false;
        }

        public void RotateMap()
        {
            MapCube[][][] newGrid = new MapCube[Width][][];
            for (int i = 0; i < Width; i++)
            {
                newGrid[i] = new MapCube[Height][];
                for (int j = 0; j < Height; j++)
                    newGrid[i][j] = new MapCube[Depth];
            }

            for (int y = 0; y < Height; y++)
            {
                for (int i = 0; i < Width; ++i)
                {
                    for (int j = 0; j < Depth; ++j)
                    {
                        newGrid[i][y][j] = Grid[Depth - j - 1][y][i];
                    }
                }
            }

            Grid = newGrid;
        }
        #endregion

        #region Location Functions
        /// <summary>
        /// Turns an Iso vector to a Screen vector
        /// </summary>
        /// <param name="isoPos">The iso coordinates</param>
        /// <param name="heightPos">The Y (height) position</param>
        /// <returns>Screen Co-ordinates</returns>
        public Vector2 IsoToScreen(Vector2 isoPos, int heightPos = 0)
        {
            return new Vector2(
                (isoPos.Y * 0.5f) + (isoPos.X * 0.5f),
                ((isoPos.X * 0.5f) - (isoPos.Y * 0.5f) - (heightPos * 0.5f)) / 2
            );
        }

        /// <summary>
        /// Turns a screen vector to an Iso vector
        /// </summary>
        /// <param name="screenPos">The screen coordinates</param>
        /// <returns>Iso World Co-ordinates</returns>
        public Vector2 ScreenToIso(Vector2 screenPos)
        {
            return new Vector2(
                (screenPos.X + 2 * screenPos.Y),
                -(2 * screenPos.Y - screenPos.X)
            );
        }

        /// <summary>
        /// Returns the MapCube at the given screen position
        /// </summary>
        /// <param name="screenPos">The screen coordinates</param>
        /// <param name="heightPos">The Y (height) position</param>
        /// <returns>The MapCube found</returns>
        public MapCube ScreenToCube(Vector2 screenPos, int heightPos = 0)
        {
            // Calculate grid index
            int x = (int)(((screenPos.X + 2 * screenPos.Y) / TILE_WIDTH) - 0.5f);
            int z = (int)-(((2 * screenPos.Y - screenPos.X) / TILE_HEIGHT) - 0.5f);

            // If we're out of range
            if ((x < 0 || x > Width - 1) ||
                (z < 0 || z > Depth - 1))
                return null;

            return Grid[x][heightPos][z];
        }

        /// <summary>
        /// Returns the Grid Index at the given screen position
        /// </summary>
        /// <param name="screenPos">The screen coordinates</param>
        /// <param name="heightPos">The Y (height) position</param>
        /// <returns>The Grid Index found</returns>
        public Point ScreenToGridIndex(Vector2 screenPos, int heightPos = 0)
        {
            // Calculate grid index
            int x = (int)(((screenPos.X + 2 * screenPos.Y) / TILE_WIDTH) - 0.5f);
            int z = (int)-(((2 * screenPos.Y - screenPos.X) / TILE_HEIGHT) - 0.5f);

            // If we're out of range
            if (x < 0)
                x = 0;
            if (x > Width - 1)
                x = Width - 1;
            if (z < 0)
                z = 0;
            if (z > Depth - 1)
                z = Depth - 1;

            return new Point(x, z);
        }
        #endregion

        #region Update & Draw
        public void Draw(GameTime gameTime)
        {
            // Render Map
            for (int x = ScreenCulling.X; x <= ScreenCulling.Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = ScreenCulling.Height; z >= ScreenCulling.Y; z--)
                    {
                        // TODO: Remove the ContainsKey check at first possible opportunity
                        // If the tile is visible and the texture isn't wacky we draw
                        if (Grid[x][y][z] != null && Grid[x][y][z].IsVisible &&                    // Make sure Grid slot isn't null and the tile is visible
                            ((y + 1 < Height && Grid[x][y + 1][z] == null || y + 1 == Height) ||    // Make sure we only draw the topmost tile
                             (x + 1 < Width && Grid[x + 1][y][z] == null || x + 1 == Width) ||      // Make sure we only draw lower than the topmost if there is no tile next to this one on the X axis
                             (z + 1 < Depth && Grid[x][y][z + 1] == null || z == 0)))               // Make sure we only draw lower than the topmost if there is no tile next to this one on the Z axis
                        {
                            // Draw Tile
                            Minion.Instance.SpriteBatch.Draw(
                                Tilesheet.Spritesheet,                      // Texture
                                Grid[x][y][z].Position,                     // Position
                                Tilesheet.Tiles[Grid[x][y][z].BaseTile],    // Source Rect
                                Grid[x][y][z].Tint,                         // Color
                                0,                                          // Rotation
                                Vector2.Zero,                               // Origin
                                1f,                                         // Scale
                                SpriteEffects.None,                         // Sprite Effects
                                Grid[x][y][z].LayerDepth                    // Layer Depth
                            );

                        }

                    }
                }
            }

            // Render Scenery
            for (int i = 0; i < Scenery.Count; i++)
                Scenery[i].Draw(gameTime);
        }
        #endregion

    }
}
