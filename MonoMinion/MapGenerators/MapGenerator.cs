using System;
using System.Collections.Generic;

namespace MonoMinion.MapGenerators
{
    public enum MapGeneratorType
    {
        CellularAutomata,
        BSP,
        Delaunay,
        Voronoi
    }

    public enum MapTileType
    {
        Wall,
        Ground
    }

    public class MapGenerator
    {
        protected MapTileType[,] map;


        public MapGenerator(int width, int height)
        {
            map = new MapTileType[width, height];
        }

    }
}
