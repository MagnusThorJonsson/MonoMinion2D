using Microsoft.Xna.Framework;
using MonoMinion.Helpers;
using System;
using System.Collections.Generic;

namespace MonoMinion.MapGenerators.BSP
{
    public class BspTree
    {
        #region Variables & Properties
        /// <summary>
        /// The root node
        /// </summary>
        public BspNode Root { get { return _root; } }
        private BspNode _root;

        private List<BspNode> _nodes;
        private int _minLeafSize;
        private int _maxLeafSize;
        #endregion

        #region Constructors
        public BspTree(int width, int height, int minLeafSize, int maxLeafSize)
        {
            _root = new BspNode(0, 0, width, height);
            _nodes = new List<BspNode>();
            _nodes.Add(_root);

            _minLeafSize = minLeafSize;
            _maxLeafSize = maxLeafSize;
        }
        #endregion

        public void Draw(GameTime gameTime, int scaleX, int scaleY)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                Rectangle nodeBox;

                if (_nodes[i].Halls != null)
                {
                    for (int j = 0; j < _nodes[i].Halls.Count; j++)
                    {
                        nodeBox = new Rectangle(
                            _nodes[i].Halls[j].X * scaleX,
                            _nodes[i].Halls[j].Y * scaleY,
                            _nodes[i].Halls[j].Width * scaleX - scaleX,
                            _nodes[i].Halls[j].Height * scaleY - scaleY
                        );

                        DrawingHelper.DrawRectangle(nodeBox, 1 * ((scaleX + scaleY) / 2), Color.LightGreen, 10);
                    }
                }

                nodeBox = new Rectangle(
                    _nodes[i].Size.X * scaleX,
                    _nodes[i].Size.Y * scaleY,
                    _nodes[i].Size.Width * scaleX - scaleX,
                    _nodes[i].Size.Height * scaleY - scaleY
                );
                DrawingHelper.DrawRectangle(nodeBox, 1 * ((scaleX + scaleY) / 2), Color.Blue, 10);
                if (!_nodes[i].Room.Equals(Rectangle.Empty))
                {
                    nodeBox = new Rectangle(
                        _nodes[i].Room.X * scaleX,
                        _nodes[i].Room.Y * scaleY,
                        _nodes[i].Room.Width * scaleX - scaleX,
                        _nodes[i].Room.Height * scaleY - scaleY
                    );
                    DrawingHelper.DrawRectangle(nodeBox, 1 * ((scaleX + scaleY) / 2), Color.Green, 10);
                }

            }
        }

        /// <summary>
        /// Populates the rooms for this BSP tree
        /// </summary>
        /// <param name="minRoomSize"></param>
        public void PopulateRooms(int minRoomSize)
        {
            _root.CreateRooms(minRoomSize);
        }

        #region Create Methods
        /// <summary>
        /// Create a new BSP tree
        /// </summary>
        /// <param name="width">The width of the tree</param>
        /// <param name="height">The height of the tree</param>
        /// <param name="minLeafSize">The minimum leaf size</param>
        /// <param name="maxLeafSize">The maximum leaf size</param>
        /// <param name="splitChance">The chance that a split will occur even if the leaf sizes are too big (0.75f is the suggested amount)</param>
        /// <param name="heightChance">The chance of the height axis will be selected vs the width axis (0.5f being an even chance)</param>
        /// <param name="sizeMarginRatio">The ratio for how likely the width/height will be selected as the next node instead of the height chance will be used (0.25f is the suggested amount)</param>
        /// <returns>A new BSP Tree object</returns>
        public static BspTree Create(int width, int height, int minLeafSize, int maxLeafSize, float splitChance, float heightChance, float sizeMarginRatio)
        {
            BspTree tree = new BspTree(width, height, minLeafSize, maxLeafSize);

            // we loop through every Leaf in our Vector over and over again, until no more Leafs can be split.
            bool didSplit = true;
            while (didSplit)
            {
                for (int i = 0; i < tree._nodes.Count; i++)
                {
                    // if this Leaf is not already split...
                    if (tree._nodes[i].Left == null && tree._nodes[i].Right == null)
                    {
                        // if this Leaf is too big, or X% chance...
                        if (tree._nodes[i].Size.Width > maxLeafSize || tree._nodes[i].Size.Height > maxLeafSize || Minion.Instance.Random.NextDouble() < splitChance)
                        {
                            // split the Leaf!
                            if (tree._nodes[i].Divide(minLeafSize, heightChance, sizeMarginRatio))
                            {
                                // if we did split, push the child leafs to the List so we can loop into them next
                                tree._nodes.Add(tree._nodes[i].Left);
                                tree._nodes.Add(tree._nodes[i].Right);
                            }
                            else
                                didSplit = false;
                        }
                    }
                }
            }

            return tree;
        }
        #endregion
    }
}
