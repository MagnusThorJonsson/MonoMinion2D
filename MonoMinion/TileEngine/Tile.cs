using System;
using Microsoft.Xna.Framework;
using MonoMinion.Collision;

namespace MonoMinion.TileEngine
{
    public class TileEventArgs : EventArgs
    {
        public int X;
        public int Y;

        public TileEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    } 

    /// <summary>
    /// Tile class for the TileMap
    /// </summary>
    public class Tile
    {
        #region Variables & Properties
        /// <summary>
        /// The tile position in the TileMap array
        /// </summary>
        public Point Position
        {
            get { return position; }
            set
            {
                position = value;
                CollidableShape.Position = new Vector2(position.X * size.X, position.Y * size.Y);
            }
        }
        protected Point position;

        // Cached Values
        /// <summary>
        /// The Tile base graphic
        /// </summary>
        public int BaseTile;

        /// <summary>
        /// The Tile color tint
        /// </summary>
        public Color Tint;

        /// <summary>
        /// Flags if the tile is visible
        /// </summary>
        public bool IsVisible;

        /// <summary>
        /// Flags if the tile is collidable
        /// </summary>
        public bool IsCollidable;

        /// <summary>
        /// The SAT collision shape
        /// </summary>
        public SATShape CollidableShape;

        protected Point size;
        #endregion

        #region Constructor
        /// <summary>
        /// Tile constructor with a square collision shape
        /// </summary>
        /// <param name="tile">Tilesheet position id</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="tw">Tile Width</param>
        /// <param name="th">Tile Height</param>
        public Tile(int tile, int x, int y, int tw, int th)
        {
            BaseTile = tile;

            position = new Point(x, y);
            size = new Point(tw, th);

            Tint = Color.White;
            IsVisible = true;
            IsCollidable = true;

            CollidableShape = new SATShape();
            Vector2[] shape = new Vector2[4];
            shape[0] = new Vector2(0, 0);
            shape[1] = new Vector2(tw, 0);
            shape[2] = new Vector2(tw, th);
            shape[3] = new Vector2(0, th);
            CollidableShape.SetShape(shape);
            CollidableShape.Position = new Vector2(x * tw, y * th);
        }

        /// <summary>
        /// Tile constructor with a custom collision shape
        /// </summary>
        /// <param name="tile">Tilesheet position id</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="tw">Tile Width</param>
        /// <param name="th">Tile Height</param>
        /// <param name="shape">Custom SAT collision shape</param>
        public Tile(int tile, int x, int y, int tw, int th, SATShape shape)
        {
            BaseTile = tile;

            position = new Point(x, y);
            size = new Point(tw, th);

            Tint = Color.White;
            IsVisible = true;
            IsCollidable = true;

            CollidableShape = shape;
            CollidableShape.Position = new Vector2(x * tw, y * th);
        }
        #endregion
    }
}
