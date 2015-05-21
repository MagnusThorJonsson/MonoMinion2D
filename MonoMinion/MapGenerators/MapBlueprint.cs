using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoMinion.MapGenerators
{
    public class MapBlueprint
    {
        /// <summary>
        /// The map width.
        /// Number of tiles on the X axis.
        /// </summary>
        public int Width { get { return width; } }
        protected int width;

        /// <summary>
        /// The map height.
        /// Number of tiles on the Y axis.
        /// </summary>
        public int Height { get { return height; } }
        protected int height;

        public MapTileType[,] Map;

        public MapBlueprint(int width, int height)
        {
            this.width = width;
            this.height = height;

            Map = new MapTileType[width, height];
        }
    }
}
