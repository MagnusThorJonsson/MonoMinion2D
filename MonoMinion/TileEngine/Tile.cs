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

    public class Tile
    {
        #region Variables
        protected Point size;
        protected Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                position = value;
                CollidableShape.Position = new Vector2(position.X * size.X, position.Y * size.Y);
            }
        }

        // Cached Values
        public int BaseTile;
        public Color Tint;

        public bool IsVisible;
        public bool IsCollidable;

        public SATShape CollidableShape;

        // Debug
        public Rectangle BoundingBox
        {
            get
            {
                return CollidableShape.Rectangle;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Tile constructor
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
        #endregion
    }
}
