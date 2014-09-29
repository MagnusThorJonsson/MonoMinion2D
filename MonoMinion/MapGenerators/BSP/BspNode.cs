using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoMinion.MapGenerators.BSP
{
    public class BspNode
    {
        #region Variables & Properties
        /// <summary>
        /// The leaf node size
        /// </summary>
        public Rectangle Size { get { return size; } }
        protected Rectangle size;

        /// <summary>
        /// Left child node
        /// </summary>
        public BspNode Left { get { return left; } }
        protected BspNode left;

        /// <summary>
        /// Right child node
        /// </summary>
        public BspNode Right { get { return right; } }
        protected BspNode right;

        /// <summary>
        /// The room rectangle
        /// </summary>
        public Rectangle Room { get { return room; } }
        protected Rectangle room;

        /// <summary>
        /// Gets the connecting halls
        /// </summary>
        public List<Rectangle> Halls { get { return halls; } }
        protected List<Rectangle> halls;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a BspNode using a Rectangle object
        /// </summary>
        /// <param name="size">The size rectangle</param>
        public BspNode(Rectangle size)
        {
            this.size = size;

            room = new Rectangle();
            halls = new List<Rectangle>();

            left = null;
            right = null;
        }

        /// <summary>
        /// Constructs a BspNode
        /// </summary>
        /// <param name="x">The position on the X axis</param>
        /// <param name="y">The position on the Y axis</param>
        /// <param name="width">The node width</param>
        /// <param name="height">The node height</param>
        public BspNode(int x, int y, int width, int height)
        {
            this.size = new Rectangle(x, y, width, height);

            left = null;
            right = null;
        }
        #endregion


        /// <summary>
        /// Divides a node 
        /// </summary>
        /// <param name="minLeafSize">The minimum leaf size</param>
        /// <param name="heightChance">The chance for whether a height axis will be used insead of the width</param>
        /// <param name="sizeMarginRatio">The ratio for how likely the width/height will be selected as the next node instead of the height chance will be used (0.25f is the suggested amount)</param>
        /// <returns>True if the node was successfully divided</returns>
        public bool Divide(int minLeafSize, float heightChance, float sizeMarginRatio = 0.25f)
        {
            // Check if we've already split the node
            if (Left != null || Right != null)
                return false;
            
            // We start of with a random chance of split and then we check
            // if the width is >X% larger than height, we split vertically
            // if the height is >X% larger than the width, we split horizontally
            bool isHeightSplit = Minion.Instance.Random.NextDouble() > heightChance;
            if (size.Width > size.Height && size.Height / size.Width >= sizeMarginRatio)
                isHeightSplit = false;
            else if (size.Height > size.Width && size.Width / size.Height >= sizeMarginRatio)
                isHeightSplit = true;

            // determine the maximum height or width and check if area is too small to split
            int max = (isHeightSplit ? size.Height : size.Width) - minLeafSize;
            if (max <= minLeafSize)
                return false;

            // create our left and right children based on the direction of the split
            int split = Minion.Instance.Random.Next(minLeafSize, max + 1);
            if (isHeightSplit)
            {
                left = new BspNode(new Rectangle(size.X, size.Y, size.Width, split));
                right = new BspNode(new Rectangle(size.X, size.Y + split, size.Width, size.Height - split));
            }
            else
            {
                left = new BspNode(new Rectangle(size.X, size.Y, split, size.Height));
                right = new BspNode(new Rectangle(size.X + split, size.Y, size.Width - split, size.Height));
            }

            return true;
        }

        /// <summary>
        /// Recursively generate rooms for all subnodes
        /// </summary>
        /// <param name="minRoomSize"></param>
        public void CreateRooms(int minRoomSize)
        {
            // this function generates all the rooms and hallways for this Leaf and all of its children.
            if (Left != null || Right != null)
            {
                // this leaf has been split, so go into the children leafs
                if (Left != null)
                {
                    Left.CreateRooms(minRoomSize);
                }
                if (Right != null)
                {
                    Right.CreateRooms(minRoomSize);
                }

                // if there are both left and right children in this Leaf, create a hallway between them
                if (Left != null && Right != null)
                {
                    CreateHall(Left.GetRandomRoom(), Right.GetRandomRoom());
                }

            }
            else
            {
                // this Leaf is the ready to make a room
                Point roomSize;
                Point roomPos;
                // the room can be between 3 x 3 tiles to the size of the leaf - 2
                roomSize = new Point(Minion.Instance.Random.Next(minRoomSize, size.Width - 1), Minion.Instance.Random.Next(minRoomSize, size.Height - 1));
                // place the room within the Leaf, but don't put it right 
                // against the side of the Leaf (that would merge rooms together)
                roomPos = new Point(Minion.Instance.Random.Next(1, size.Width - roomSize.X), Minion.Instance.Random.Next(1, size.Height - roomSize.Y));
                room = new Rectangle(size.X + roomPos.X, size.Y + roomPos.Y, roomSize.X, roomSize.Y);
            }
        }



        /// <summary>
        /// Gets a random room from this node or it's children recursively.
        /// </summary>
        /// <returns>A room
        /// .</returns>
        public Rectangle GetRandomRoom()
        {
            // iterate all the way through these leafs to find a room, if one exists.
            if (room != null)
                return room;
            else
            {
                Rectangle lRoom = Rectangle.Empty;
                Rectangle rRoom = Rectangle.Empty;

                if (Left != null)
                    lRoom = Left.GetRandomRoom();
                if (Right != null)
                    rRoom = Right.GetRandomRoom();

                if (lRoom.Equals(Rectangle.Empty) && rRoom.Equals(Rectangle.Empty))
                    return Rectangle.Empty;
                else if (rRoom.Equals(Rectangle.Empty))
                    return lRoom;
                else if (lRoom.Equals(Rectangle.Empty))
                    return rRoom;
                else if (Minion.Instance.Random.NextDouble() > .5)
                    return lRoom;

                return rRoom;
            }
        }


        public void CreateHall(Rectangle l, Rectangle r)
        {
            if (!l.Equals(Rectangle.Empty) && !r.Equals(Rectangle.Empty))
            {
                // now we connect these two rooms together with hallways.
                // this looks pretty complicated, but it's just trying to figure out which point is where and then either draw a straight line, or a pair of lines to make a right-angle to connect them.
                // you could do some extra logic to make your halls more bendy, or do some more advanced things if you wanted.
                Point point1 = new Point(Minion.Instance.Random.Next(l.X + 1, l.X + l.Width - 1), Minion.Instance.Random.Next(l.Y + 1, l.Y + l.Height - 1));
                Point point2 = new Point(Minion.Instance.Random.Next(r.X + 1, r.X + r.Width - 1), Minion.Instance.Random.Next(r.Y + 1, r.Y + r.Height - 1));

                float w = point2.X - point1.X;
                float h = point2.Y - point1.Y;

                if (w < 0)
                {
                    if (h < 0)
                    {
                        if (Minion.Instance.Random.NextDouble() > 0.5)
                        {
                            halls.Add(new Rectangle(point2.X, point1.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point2.X, point2.Y, 1, (int)Math.Abs(h)));
                        }
                        else
                        {
                            halls.Add(new Rectangle(point2.X, point2.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point1.X, point2.Y, 1, (int)Math.Abs(h)));
                        }
                    }
                    else if (h > 0)
                    {
                        if (Minion.Instance.Random.NextDouble() > 0.5)
                        {
                            halls.Add(new Rectangle(point2.X, point1.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point2.X, point1.Y, 1, (int)Math.Abs(h)));
                        }
                        else
                        {
                            halls.Add(new Rectangle(point2.X, point2.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point1.X, point1.Y, 1, (int)Math.Abs(h)));
                        }
                    }
                    else // if (h == 0)
                    {
                        halls.Add(new Rectangle(point2.X, point2.Y, (int)Math.Abs(w), 1));
                    }
                }
                else if (w > 0)
                {
                    if (h < 0)
                    {
                        if (Minion.Instance.Random.NextDouble() > 0.5)
                        {
                            halls.Add(new Rectangle(point1.X, point2.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point1.X, point2.Y, 1, (int)Math.Abs(h)));
                        }
                        else
                        {
                            halls.Add(new Rectangle(point1.X, point1.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point2.X, point2.Y, 1, (int)Math.Abs(h)));
                        }
                    }
                    else if (h > 0)
                    {
                        if (Minion.Instance.Random.NextDouble() > 0.5)
                        {
                            halls.Add(new Rectangle(point1.X, point1.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point2.X, point1.Y, 1, (int)Math.Abs(h)));
                        }
                        else
                        {
                            halls.Add(new Rectangle(point1.X, point2.Y, (int)Math.Abs(w), 1));
                            halls.Add(new Rectangle(point1.X, point1.Y, 1, (int)Math.Abs(h)));
                        }
                    }
                    else // if (h == 0)
                    {
                        halls.Add(new Rectangle(point1.X, point1.Y, (int)Math.Abs(w), 1));
                    }
                }
                else // if (w == 0)
                {
                    if (h < 0)
                    {
                        halls.Add(new Rectangle(point2.X, point2.Y, 1, (int)Math.Abs(h)));
                    }
                    else if (h > 0)
                    {
                        halls.Add(new Rectangle(point1.X, point1.Y, 1, (int)Math.Abs(h)));
                    }
                }
            }
        }
    }
}
