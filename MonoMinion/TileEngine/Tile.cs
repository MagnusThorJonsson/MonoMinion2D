using System;
using Microsoft.Xna.Framework;
using MonoMinion.Collision;
using System.Collections.Generic;
using MonoMinion.Helpers;

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
    public class Tile : IMapNode
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

        /// <summary>
        /// The tile depth
        /// </summary>
        public float Depth { get { return depth; } }
        protected float depth;

        // Cached Values
        /// <summary>
        /// The Tile base graphic
        /// </summary>
        public int BaseTile;

        /// <summary>
        /// The Tile color tint
        /// </summary>
        public Color Tint 
        { 
            get { return tint; }
            set { tint = value; }
        }
        protected Color tint;

        /// <summary>
        /// Flags if the tile is visible
        /// </summary>
        public bool IsVisible;

        /// <summary>
        /// Flags if the tile is collidable
        /// </summary>
        public bool IsCollidable
        {
            get { return isCollidable; }
            set { isCollidable = value; }
        }
        protected bool isCollidable;

        /// <summary>
        /// The SAT collision shape
        /// </summary>
        public SATShape CollidableShape;

        /// <summary>
        /// Used to calculate path for the A* pathfinder helper
        /// </summary>
        public int PathCostModifier 
        {
            get { return pathCostModifier; } 
        }
        protected int pathCostModifier;

        protected Point size;

        protected List<Object> onTile;
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
        /// <param name="depth">Tile Drawing Depth</param>
        public Tile(int tile, int x, int y, int tw, int th, float depth)
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

            this.depth = depth;

            onTile = new List<object>();
        }

        /// <summary>
        /// Tile constructor with a custom collision shape
        /// </summary>
        /// <param name="tile">Tilesheet position id</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="tw">Tile Width</param>
        /// <param name="th">Tile Height</param>
        /// <param name="depth">Tile Drawing Depth</param>
        /// <param name="shape">Custom SAT collision shape</param>
        public Tile(int tile, int x, int y, int tw, int th, float depth, SATShape shape)
        {
            BaseTile = tile;

            position = new Point(x, y);
            size = new Point(tw, th);

            this.depth = depth;

            Tint = Color.White;
            IsVisible = true;
            IsCollidable = true;

            CollidableShape = shape;
            CollidableShape.Position = new Vector2(x * tw, y * th);

            onTile = new List<object>();
        }
        #endregion

        #region Methods for Objects on Tile
        /// <summary>
        /// Checks if a tile has any objects
        /// </summary>
        /// <returns>True if any objects found</returns>
        public bool HasObject()
        {
            if (onTile.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Adds an object to the tile
        /// </summary>
        /// <param name="obj">The object to add</param>
        /// <returns>True on success</returns>
        public bool AddObject(Object obj)
        {
            if (obj != null)
            {
                onTile.Add(obj);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an object from the tile
        /// </summary>
        /// <param name="index">The index of the object to remove</param>
        /// <returns>True on success</returns>
        public bool RemoveObject(int index)
        {
            if (index < onTile.Count)
            {
                onTile.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes an object from the tile
        /// </summary>
        /// <param name="obj">The object to remove from the tile</param>
        /// <returns>True on success</returns>
        public bool RemoveObject(Object obj)
        {
            if (onTile.Contains(obj))
            {
                return onTile.Remove(obj);
            }
            return false;
        }

        /// <summary>
        /// Gets an object
        /// </summary>
        /// <param name="index">The index of the tile</param>
        /// <returns>Null if no object is found</returns>
        public Object GetObject(int index)
        {
            if (index < onTile.Count)
            {
                return onTile[index];
            }

            return null;
        }

        /// <summary>
        /// Gets all objects
        /// </summary>
        /// <returns>An array of the current objects</returns>
        public Object[] GetObjects()
        {
            return onTile.ToArray();
        }
        #endregion
    }
}
