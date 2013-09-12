using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoMinion.IsoCubeEngine
{
    public abstract class Cube
    {
        #region Variables
        protected string id;
        public string Id { get { return id; } }

        public int X;
        public int Y;
        public int Z;

        // Cached Values
        public float LayerDepth;
        public Vector2 Position;

        //protected string baseTile;
        //public string BaseTile { get { return baseTile; } }
        public int BaseTile;
        public Color Tint;

        public bool IsVisible;
        public bool IsCollidable;
        #endregion

        #region Constructor
        public Cube(string id, int tile, int x, int y, int z)
        {
            this.id = id;
            BaseTile = tile;

            X = x;
            Y = y;
            Z = z;

            Position = Vector2.Zero;

            Tint = Color.White;
            IsVisible = true;
            IsCollidable = true;
        }
        #endregion
    }
}
