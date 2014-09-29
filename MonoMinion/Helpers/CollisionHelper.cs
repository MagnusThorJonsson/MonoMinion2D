using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MonoMinion.Helpers
{
    /// <summary>
    /// Static Helper class for Collision Detection
    /// </summary>
    public static class CollisionHelper
    {
        #region Bounding Collisions

        public static bool LineIntersectsRect(Vector2 p1, Vector2 p2, Rectangle r)
        {
            return LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y)) ||
                   (r.Contains(p1) && r.Contains(p2));
        }

        private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }


        public static bool LineIntersectsRect(Point p1, Point p2, Rectangle r)
        {
            return LineIntersectsLine(p1, p2, new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y + r.Height), new Point(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X, r.Y + r.Height), new Point(r.X, r.Y)) ||
                   (r.Contains(p1) && r.Contains(p2));
        }

        private static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if two rectangles intersect each other
        /// </summary>
        /// <param name="x1">Position of rectangle A on the X axis</param>
        /// <param name="y1">Position of rectangle A on the Y axis</param>
        /// <param name="width1">Width of rectangle A</param>
        /// <param name="height1">Height of rectangle A</param>
        /// <param name="x2">Position of rectangle B on the X axis</param>
        /// <param name="y2">Position of rectangle B on the Y axis</param>
        /// <param name="width2">Width of rectangle B</param>
        /// <param name="height2">Height of rectangle B</param>
        /// <returns>True if rectangles intersect</returns>
        public static bool DoRectsIntersect(int x1, int y1, int width1, int height1, int x2, int y2, int width2, int height2)
        {
            Rectangle rectangleA = new Rectangle((int)x1, (int)y1, width1, height1);
            Rectangle rectangleB = new Rectangle((int)x2, (int)y2, width2, height2);

            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            if (top >= bottom || left >= right)
                return false;

            return true;
        }

        /// <summary>
        /// Checks whether two rectangles intersect
        /// </summary>
        /// <param name="rectA">Rectangle A</param>
        /// <param name="rectB">Rectangle B</param>
        /// <returns>True if rectangles intersect</returns>
        public static bool DoRectsIntersect(Rectangle rectA, Rectangle rectB)
        {
            int top = Math.Max(rectA.Top, rectB.Top);
            int bottom = Math.Min(rectA.Bottom, rectB.Bottom);
            int left = Math.Max(rectA.Left, rectB.Left);
            int right = Math.Min(rectA.Right, rectB.Right);

            if (top >= bottom || left >= right)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if two circles intersect
        /// </summary>
        /// <param name="x1">Position on the X axis for Circle A</param>
        /// <param name="y1">Position on the Y axis for Circle A</param>
        /// <param name="radius1">Radius for Circle A</param>
        /// <param name="x2">Position on the X axis for Circle B</param>
        /// <param name="y2">Position on the Y axis for Circle B</param>
        /// <param name="radius2">Radius for Circle B</param>
        /// <returns>True if circles intersect</returns>
        public static bool DoCirclesIntersect(int x1, int y1, int radius1, int x2, int y2, int radius2)
        {
            Vector2 V1 = new Vector2(x1, y1);
            Vector2 V2 = new Vector2(x2, y2);

            Vector2 Distance = V1 - V2;

            if (Distance.Length() < radius1 + radius2)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if triangles intersect
        /// </summary>
        /// <param name="p1">Vector list for Triangle A</param>
        /// <param name="p2">Vector list for Triangle B</param>
        /// <returns>True if triangle intersect</returns>
        public static bool DoTrianglesIntersect(List<Vector2> p1, List<Vector2> p2)
        {
            for (int i = 0; i < 3; i++)
                if (_isPointInsideTriangle(p1, p2[i])) return true;

            for (int i = 0; i < 3; i++)
                if (_isPointInsideTriangle(p2, p1[i])) return true;
            return false;
        }

        /// <summary>
        /// Helper for DoTrianglesIntersect
        /// </summary>
        /// <param name="TrianglePoints">Vector list for the Triangle</param>
        /// <param name="p">Vector to check</param>
        /// <returns>True if the point is inside the triangle</returns>
        private static bool _isPointInsideTriangle(List<Vector2> TrianglePoints, Vector2 p)
        {
            // Translated to C# from: http://www.ddj.com/184404201
            Vector2 e0 = p - TrianglePoints[0];
            Vector2 e1 = TrianglePoints[1] - TrianglePoints[0];
            Vector2 e2 = TrianglePoints[2] - TrianglePoints[0];

            float u, v = 0;
            if (e1.X == 0)
            {
                if (e2.X == 0) return false;
                u = e0.X / e2.X;
                if (u < 0 || u > 1) return false;
                if (e1.Y == 0) return false;
                v = (e0.Y - e2.Y * u) / e1.Y;
                if (v < 0) return false;
            }
            else
            {
                float d = e2.Y * e1.X - e2.X * e1.Y;
                if (d == 0) return false;
                u = (e0.Y * e1.X - e0.X * e1.Y) / d;
                if (u < 0 || u > 1) return false;
                v = (e0.X - e2.X * u) / e1.X;
                if (v < 0) return false;
                if ((u + v) > 1) return false;
            }

            return true;
        }


        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width * 0.5f; // / 2.0f;
            float halfHeightA = rectA.Height * 0.5f; // / 2.0f;
            float halfWidthB = rectB.Width * 0.5f; // / 2.0f;
            float halfHeightB = rectB.Height * 0.5f; // / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static float GetHorizontalIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;

            // Calculate centers.
            float centerA = rectA.Left + halfWidthA;
            float centerB = rectB.Left + halfWidthB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX)
                return 0f;

            // Calculate and return intersection depths.
            return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        }

        public static float GetVerticalIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfHeightA = rectA.Height / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            float centerA = rectA.Top + halfHeightA;
            float centerB = rectB.Top + halfHeightB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceY = centerA - centerB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceY) >= minDistanceY)
                return 0f;

            // Calculate and return intersection depths.
            return distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
        }
        #endregion

        #region SAT Collisions

        #endregion
    }
}
