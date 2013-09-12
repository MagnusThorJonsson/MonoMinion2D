using System;
using Microsoft.Xna.Framework;

namespace MonoMinion.Collision
{
    public class SATShape
    {
        #region Variables
        // Positional variables
        private Vector2 _position;
        protected Vector2 offset;
        protected Vector2 origin;
        protected float rotation;
        protected Vector2 rotationOffset;
        // Shape variables
        protected Rectangle rectangle;
        protected Vector2[] vertices;
        // Cached variables
        private Vector2[] _cacheVertices;
        private Vector2 _cachePosition;
        private float _cacheRotation;
        private Vector2[] _cacheEdges;

        public bool IsStatic;
        #endregion

        #region Properties
        /// <summary>
        /// Current position of the shape
        /// </summary>
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                updateBoundingBox();
            }
        }

        /// <summary>
        /// Shape edges
        /// </summary>
        public Vector2[] Edges
        {
            set { _cacheEdges = value; }
            get { return _cacheEdges; }
        }

        /// <summary>
        /// The position offset
        /// </summary>
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        /// <summary>
        /// Current rotation of the shape
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                updateBoundingBox();
            }
        }

        /// <summary>
        /// Offset for shape rotation center
        /// </summary>
        public Vector2 RotationOffset
        {
            get { return rotationOffset; }
            set { rotationOffset = value; }
        }

        /// <summary>
        /// Number of vertices in the shape
        /// </summary>
        public int VertexCount
        {
            get
            {
                return vertices.Length;
            }
        }

        /// <summary>
        /// Shape origin
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// The center of the shape
        /// </summary>
        public Vector2 Center
        {
            get { return origin + Position + Offset; }
        }

        /// <summary>
        /// A rectangle containing the shape.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
        }
        #endregion

        /// <summary>
        /// Separating Axis Thereom Shape Constructor
        /// </summary>
        public SATShape()
        {
            _position = Vector2.Zero;
            _cachePosition = Vector2.Zero;
            Offset = Vector2.Zero;
            origin = Vector2.Zero;
            RotationOffset = Vector2.Zero;
            rotation = 0f;
            _cacheRotation = 0f;
            rectangle = Rectangle.Empty;
            vertices = null;
            _cacheVertices = null;
            _cacheEdges = null;

            IsStatic = true;
        }



        #region Update & Draw
        /// <summary>
        /// Updates the shape
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public virtual void Update(GameTime gameTime)
        {
            // TODO: Maybe find a better place for this
            // Regenerate and project axis from cache
            //CacheVertices();
        }
        #endregion

        #region Shape Creation Methods
        /// <summary>
        /// Sets the SAT shape to a rectangle
        /// </summary>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        public void CreateRectangle(int width, int height)
        {
            Vector2[] shape = new Vector2[4];
            shape[0] = new Vector2(0, 0);
            shape[1] = new Vector2(width, 0);
            shape[2] = new Vector2(width, height);
            shape[3] = new Vector2(0, height);
            this.SetShape(shape);
        }
        #endregion

        #region Shape Manipulation Methods
        /// <summary>
        /// Sets vertex data to the shape
        /// </summary>
        /// <param name="shape">The vertex data that defines the shape</param>
        /// <returns>True if shape was valid, false if not</returns>
        public bool SetShape(Vector2[] shape, bool isStatic = true)
        {
            if (shape.Length < 2)
                return false;

            // We clone the collections to avoid strange referencing issues.
            vertices = new Vector2[shape.Length];
            _cacheVertices = new Vector2[shape.Length];

            int minX = int.MaxValue; int minY = int.MaxValue;
            int maxX = int.MinValue; int maxY = int.MinValue;

            for (int i = 0; i < shape.Length; i++)
            {
                vertices[i] = shape[i];
                origin += vertices[i];

                _cacheVertices[i] = _rotateVertex(vertices[i] + Position + Offset);

                minX = (int)Math.Min(minX, shape[i].X);
                maxX = (int)Math.Max(maxX, shape[i].X);

                minY = (int)Math.Min(minY, shape[i].Y);
                maxY = (int)Math.Max(maxY, shape[i].Y);
            }

            origin /= (float)(shape.Length);

            IsStatic = isStatic;

            // Calculate edges
            _cacheEdges = calculateProjectionAxes(vertices);

            // Create AABB rectangle
            rectangle = new Rectangle(
                minX, minY,
                maxX - minX, maxY - minY
            );

            return true;
        }

        /// <summary>
        /// Changes a current vertex into something new
        /// </summary>
        /// <param name="index">index of the vertex</param>
        /// <param name="vertex">The vector for the vertex</param>
        /// <returns>True if change was successful</returns>
        public bool ChangeVertex(int index, Vector2 vertex)
        {
            if (vertices != null || index > vertices.Length)
            {
                vertices[index] = vertex;
                // Do recaching and recalculate edges
                for (int i = 0; i < vertices.Length; i++)
                    _cacheVertices[i] = _rotateVertex(vertices[i] + Position + Offset);
                _cacheEdges = calculateProjectionAxes(GetVertices(Vector2.Zero));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the translated vertices of the shape.
        /// </summary>
        /// <returns>A vector with the vertices at an updated position</returns>
        public Vector2[] GetVertices(Vector2 velocity)
        {
            // TODO: Cache
            //if (!IsStatic)
            for (int i = 0; i < vertices.Length; i++)
                _cacheVertices[i] = _rotateVertex(vertices[i] + Position + Offset + velocity);

            return _cacheVertices;
        }
        #endregion

        #region Collision Methods
        /// <summary>
        /// Detects and resolves the collision between two shapes
        /// </summary>
        /// <param name="shape">The shape to test for</param>
        /// <param name="velocity">The velocity of the moving</param>
        /// <param name="mtv">The Mininum Transfer Vector, i.e. the depth of the intersect</param>
        /// <returns>True if a collision is detected</returns>
        public bool Intersect(SATShape shape, Vector2 velocity, ref Vector2 mtv)
        {
            // Broad phase...
            Rectangle aabb = this.Rectangle;
            aabb.X = (int)(aabb.X + velocity.X);
            aabb.Y = (int)(aabb.Y + velocity.Y);
            if (!shape.Rectangle.Intersects(aabb))
                return false;

            // Get vertices and calculate edges
            Vector2[] shapeA = this.GetVertices(velocity);
            Vector2[] shapeB = shape.GetVertices(Vector2.Zero);
            Vector2[] axis = new Vector2[this.VertexCount + shape.VertexCount];
            Array.Copy(calculateProjectionAxes(shapeA), axis, this.VertexCount);

            // If the shape is static we don't need to recalculate projection axes
            if (!shape.IsStatic)
                shape.Edges = calculateProjectionAxes(shapeB);
            Array.Copy(shape.Edges, 0, axis, this.VertexCount, shape.VertexCount);

            float intervalWidth = 0;
            float minimumDistance = float.MaxValue;
            for (int i = 0; i < axis.Length; i++)
            {
                // Test to see if this axis seperates shapes A and B.
                if (!areAxesSeparated(axis[i], shapeA, shapeB, ref intervalWidth))
                {
                    intervalWidth *= 1.01f;

                    // Find the shortest distance needed to move to resolve collision.
                    if (intervalWidth * intervalWidth < minimumDistance * minimumDistance)
                    {
                        minimumDistance = intervalWidth;
                        mtv = axis[i] * minimumDistance;
                    }
                }
                // We found a separation, no need to continue
                else
                    return false;
            }

            // Make sure we are trying to push this shape away and not pull it into the passed in shape.
            if (mtv != Vector2.Zero)
            {
                Vector2 direction = new Vector2(shape.Center.X, shape.Center.Y) - this.Center;
                if (Vector2.Dot(direction, mtv) < 0.0f)
                    mtv = -mtv;
            }

            return true;
        }


        /// <summary>
        /// Updates the bounding box rectangle for this shape
        /// </summary>
        /// <returns>A vector with the vertices at an updated position</returns>
        protected void updateBoundingBox()
        {
            // If position or rotation haven't changed we don't have to regenerate the cache
            if (vertices != null && (!(Rotation == _cacheRotation) || (Vector2.DistanceSquared(_cachePosition, Position) > (float.Epsilon * float.Epsilon))))
            {
                float maxX = 0, maxY = 0;
                float minX = 0, minY = 0;
                Vector2 tmpVert = Vector2.Zero;
                for (int i = 0; i < vertices.Length; i++)
                {
                    tmpVert = _rotateVertex(vertices[i] + Position + Offset);

                    // Save AABB rectangle size
                    if ((tmpVert.X - Position.X + Offset.X) > maxX)
                        maxX = tmpVert.X - Position.X + Offset.X;
                    else if ((tmpVert.X - Position.X + Offset.X) < minX)
                        minX = tmpVert.X - Position.X + Offset.X;
                    if ((tmpVert.Y - Position.Y + Offset.Y) > maxY)
                        maxY = tmpVert.Y - Position.Y + Offset.Y;
                    else if ((tmpVert.Y - Position.Y + Offset.Y) < minY)
                        minY = tmpVert.Y - Position.Y + Offset.Y;
                }

                // Update cache
                _cacheRotation = Rotation;
                _cachePosition = Position;

                // Update bounding rectangle cache
                rectangle.X = (int)(minX + Position.X - Offset.X);
                rectangle.Y = (int)(minY + Position.Y - Offset.Y);
                rectangle.Width = (int)(maxX - minX);
                rectangle.Height = (int)(maxY - minY);
            }
        }

        /// <summary>
        /// Caching helper.
        /// Rotates a vertex based on the shape rotation radian and center 
        /// </summary>
        /// <param name="vertex">The vertex to rotate</param>
        /// <returns>The position of rotated vertex</returns>
        private Vector2 _rotateVertex(Vector2 vertex)
        {
            float cosRadians = (float)Math.Cos(Rotation);
            float sinRadians = (float)Math.Sin(Rotation);

            return new Vector2(
                (vertex.X - (Center.X + rotationOffset.X)) * cosRadians - (vertex.Y - (Center.Y + rotationOffset.Y)) * sinRadians + Center.X + rotationOffset.X, // Translate X * rotation
                (vertex.X - (Center.X + rotationOffset.X)) * sinRadians + (vertex.Y - (Center.Y + rotationOffset.Y)) * cosRadians + Center.Y + rotationOffset.Y  // Translate Y * rotation
            );
        }
        #endregion


        #region Static Collision Helpers
        /// <summary>
        /// Tests if an axis seperates the passed in shape from this one.
        /// </summary>
        /// <param name="axis">The axis to test</param>
        /// <param name="shapeA">The first shape to test</param>
        /// <param name="shapeB">The second shape to test</param>
        /// <param name="intervalWidth">The current internal width of the axis</param>
        /// <returns>False if an axis is not separating a shape</returns>
        protected static bool areAxesSeparated(Vector2 axis, Vector2[] shapeA, Vector2[] shapeB, ref float intervalWidth)
        {
            float minA = 0, maxA = 0;
            float minB = 0, maxB = 0;

            projectOntoAxis(axis, shapeA, ref minA, ref maxA);
            projectOntoAxis(axis, shapeB, ref minB, ref maxB);

            // If we're not intersecting we bail
            if (minB > maxA || minA > maxB)
            {
                intervalWidth = 0;
                return true;
            }

            intervalWidth = MathHelper.Min(maxA - minB, maxB - minA);

            return false;
        }


        /// <summary>
        /// Projects the shape onto an axis.
        /// </summary>
        /// <param name="axis">The axis to project the shape onto</param>
        /// <param name="shape">The shape to project</param>
        /// <param name="min">The minimum projection on the axis</param>
        /// <param name="max">The maximum projection on the axis</param>
        protected static void projectOntoAxis(Vector2 axis, Vector2[] shape, ref float min, ref float max)
        {
            float interval = Vector2.Dot(axis, shape[0]);
            min = max = interval;
            for (int i = 0; i < shape.Length; i++)
            {
                interval = Vector2.Dot(axis, shape[i]);

                if (interval < min)
                    min = interval;
                else if (interval > max)
                    max = interval;
            }
        }

        /// <summary>
        /// Calculates the edges for a given shape
        /// </summary>
        /// <param name="verts">The shape to calculate from</param>
        /// <returns>A vector array of edges</returns>
        protected static Vector2[] calculateProjectionAxes(Vector2[] verts)
        {
            Vector2[] edges = new Vector2[verts.Length];
            int nextIndex = 0;
            for (int i = 0; i < verts.Length; i++)
            {
                // Last index is the first one so we close the circle
                nextIndex = i + 1;
                if (nextIndex >= verts.Length)
                    nextIndex = 0;

                // Find the normal of the seperation axis.
                edges[i] = Vector2.Normalize(verts[i] - verts[nextIndex]);
                edges[i] = new Vector2(-edges[i].Y, edges[i].X);
            }

            return edges;
        }
        #endregion

        /// <summary>
        /// Copies SAT shape
        /// </summary>
        /// <returns>A copy of the shape</returns>
        public SATShape Copy()
        {
            SATShape shape = new SATShape();
            shape._position = _position;
            shape._cachePosition = _cachePosition;
            shape.Offset = Offset;
            shape.origin = origin;
            shape.RotationOffset = RotationOffset;
            shape.rotation = rotation;
            shape._cacheRotation = _cacheRotation;
            shape.rectangle = rectangle;
            shape.vertices = vertices;
            shape._cacheVertices = _cacheVertices;
            shape._cacheEdges = _cacheEdges;
            shape.IsStatic = IsStatic;

            return shape;
        }
    }
}
