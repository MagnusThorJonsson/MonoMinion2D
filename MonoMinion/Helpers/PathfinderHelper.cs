using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoMinion.Helpers
{
    #region Helper Classes
    /// <summary>
    /// Heuristics selector
    /// </summary>
    public enum Heuristics
    {
        Manhattan,
        Euclidian,
        Diagonal,
        Djikstra
    }

    /// <summary>
    /// Stores open and closed list nodes with their respective costs. 
    /// </summary>
    public class PathNode
    {
        public Point Position;
        public PathNode Parent;
        public int G;
        //public int H;
        public int Cost;
        public PathNode(Point pos, PathNode parent, int g, int h)
        {
            Position = pos;
            Parent = parent;
            G = g;
            //H = h;
            Cost = g + h;
        }
    }

    /// <summary>
    /// Inherit from this to build your map for the pathfinder
    /// then you can just drop in your array in the pathfinders
    /// constructor.
    /// </summary>
    public interface IMapNode
    {
        bool IsPassable { get; set; }
        Color Tint { get; set; }
        int PathCostModifier { get; }
        //void SetColor(Color c);
    }
    #endregion

    /// <summary>
    /// Helper class for pathfinding. 
    /// Uses A* pathfinding and has the option of 3 heuristics (Manhattan, Diagonal and Euclidian)
    /// </summary>
    public class PathfinderHelper
    {
        private IMapNode[,] _tiles;
        private Heuristics _heuristicsType;
        private int _maxClosedNodes = 512;

        /// <summary>
        /// Helper class that uses A* for pathfinding
        /// </summary>
        /// <param name="nodes">Node map to use as a base</param>
        /// <param name="type">Heuristics type used in the A* algorithm</param>
        /// <param name="maxNodes">Maximum amount of nodes allowed before the pathfinder gives up (resolves deadlocking)</param>
        public PathfinderHelper(IMapNode[,] nodes, Heuristics type = Heuristics.Manhattan, int maxNodes = 512)
        {
            // Set Map nodes - aka whatever your map is 
            _tiles = nodes;
            _heuristicsType = type;
            _maxClosedNodes = maxNodes;
        }


        /// <summary>
        /// Updates the nodemap used by the pathfinder
        /// </summary>
        /// <param name="nodes">Node map</param>
        public void UpdateNodes(IMapNode[,] nodes)
        {
            this._tiles = nodes;
        }

        /// <summary>
        /// Finds the best bath from point A to point B
        /// </summary>
        /// <param name="start">Starting tile</param>
        /// <param name="end">End tile</param>
        /// <returns>List of tiles representing the path found</returns>
        public List<Point> FindPath(Point start, Point end)
        {

#if DEBUG
            // Record the starting tickcount so we can take it away later to
            // find out how long the pathfinding tooking
            int startTime = Environment.TickCount;
#endif
            // Create open and closed lists
            PathNodeBinaryHeap openList = new PathNodeBinaryHeap(_tiles.GetLength(0), _tiles.GetLength(1));
            List<PathNode> closedList = new List<PathNode>();

            // Add first node(the starting point)
            PathNode current = new PathNode(start, null, 0, 0);
            openList.AddNode(current);

            // Create variables to be used in loop
            int xStart, yStart, xEnd, yEnd, width = _tiles.GetLength(0), height = _tiles.GetLength(1),
                gCost, hCost;

            Point position = Point.Zero;

            // Node searching loop, this is where the squares get searched to find the path
            while (true)
            {
                // Check adjacent squares
                xStart = Math.Max(current.Position.X - 1, 0);
                yStart = Math.Max(current.Position.Y - 1, 0);
                xEnd = Math.Min(current.Position.X + 1, width - 1);
                yEnd = Math.Min(current.Position.Y + 1, height - 1);

                for (int x = xStart; x <= xEnd; x++)
                    for (int y = yStart; y <= yEnd; y++)
                        if (!(x == current.Position.X && y == current.Position.Y))
                            if (_tiles[x, y].IsPassable)
                            {
                                gCost = current.G + _tiles[x, y].PathCostModifier;
                                // If movement is diagonal give it a higher G scoring to down play diagonals as good movement choices
                                if ((x == xStart && y == yStart) || (x == xEnd && y == xStart)
                                    || (x == xStart && y == yEnd) || (x == xEnd && y == yEnd))
                                    gCost += 14;
                                else
                                    gCost += 10;

                                position.X = x;
                                position.Y = y;

                                // Select the heuristics being used
                                if (_heuristicsType == Heuristics.Diagonal)
                                    hCost = DiagonalHeuristic(position, end);
                                // Euclidian
                                else if (_heuristicsType == Heuristics.Euclidian)
                                    hCost = EuclideanHeuristic(position, end);
                                // Djikstra
                                else if (_heuristicsType == Heuristics.Djikstra)
                                    hCost = 0;
                                // Defaults to Manhattan
                                else
                                    hCost = ManhattanHeuristic(position, end);

                                // Add nodes that are not impassible to open list
                                // Check node doesnt already exist
                                if (!NodeInList(position, closedList))
                                {
                                    PathNode node = openList.FindByPosition(position);
                                    // If node is not in open list add it
                                    if (node == null)
                                    {
                                        openList.AddNode(new PathNode(position, current, gCost, hCost));
#if DEBUG
                                        // Colour tile to show you've checked it - remove this in your actual game.
                                        //_tiles[position.X, position.Y].Tint = Color.LightBlue;
#endif
                                    }
                                    else
                                    {
                                        // Check to see if moving from current square back to an already checked square is lower cost G cost. 
                                        // If so recalculate cost for this node and resort it in heap to its proper position
                                        if (node.G > gCost)
                                        {
                                            // Reparent and change costs                                   
                                            node.Parent = current;
                                            node.G = gCost;
                                            node.Cost = gCost + hCost;
                                            openList.ResortNodeUp(node);
                                        }
                                    }
                                }
                            }

                // Drop current node from open list and add to closed
                openList.RemoveNode(current);
                closedList.Add(current);
#if DEBUG
                //_tiles[current.Position.X, current.Position.Y].Tint = Color.Blue;
#endif
                // Find lowest cost square and start again on that node
                if (openList.Count == 0)
                {
                    // No possible movement from start
                    return null;
                }

                current = openList.TopNode;

                // reached end
                if (current.Position == end)
                    break;

                // taking too long
                if (closedList.Count > _maxClosedNodes)
                    break;

            }

            // Work back through parents to find path
            List<Point> path = new List<Point>();
            while (current.Parent != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
#if DEBUG
                // Set colour of path, remove this in your actual game
                //_tiles[current.Position.X, current.Position.Y].Tint = Color.Green;
#endif
            }

            // Clear list resources used
            openList.Dispose();
            closedList.Clear();

#if DEBUG
            // Output time it took to calculate path in console
            Console.WriteLine("Path took: " + (Environment.TickCount - startTime));
#endif

            return path;
        }

        #region Heuristics Functions
        /// <summary>
        /// This will work out a hueristic based on orthagonal movement only. Which may slightly
        /// overestimate the route. However you can change it to any hueristic you like. 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int ManhattanHeuristic(Point current, Point end)
        {
            return 10 * (Math.Abs(current.X - end.X) + Math.Abs(current.Y - end.Y));
        }

        private int EuclideanHeuristic(Point current, Point end)
        {
            return 10 * (int)Math.Sqrt(Math.Pow(current.X - end.X, 2) + Math.Pow(current.Y - end.Y, 2));
        }

        private int DiagonalHeuristic(Point current, Point end)
        {
            return 10 * Math.Max(Math.Abs(end.X - current.X), Math.Abs(end.Y - current.Y));
        }
        #endregion


        /// <summary>
        /// Checks if a node is in a list by its position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private bool NodeInList(Point position, List<PathNode> nodes)
        {
            foreach (PathNode node in nodes)
            {
                if (node.Position == position)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns node if it is in the given list by searching for its position,
        /// if it does not exist in it then it will return null.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private PathNode FindNodeInList(Point position, List<PathNode> nodes)
        {
            foreach (PathNode node in nodes)
            {
                if (node.Position == position)
                    return node;
            }

            return null;
        }

    }
}
