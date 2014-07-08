using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoMinion.Graphics
{
    public class Camera2D
    {
        #region Variables
        protected float zoom;
        protected float rotation;
        protected Vector2 position;
        protected Vector2 oldPosition;
        protected Matrix transform;
        private GraphicsDevice graphicsDevice;

        private Rectangle cacheRect;
        private Rectangle clamp;
        #endregion

        #region Properties
        public float Zoom
        {
            get { return this.zoom; }
            set
            {
                if (value < 0.1f)
                    this.zoom = 0.1f;
                else
                    this.zoom = value;

                this.Transformation();
            }
        }

        public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.oldPosition = this.position;
                this.position = value;

                if (this.position.X < clamp.X)
                    this.position.X = clamp.X;
                if (this.position.X > clamp.Width)
                    this.position.X = clamp.Width;
                if (this.position.Y < clamp.Y)
                    this.position.Y = clamp.Y;
                if (this.position.Y > clamp.Height)
                    this.position.Y = clamp.Height;

                this.Transformation();
            }
        }

        public Rectangle Clamp
        {
            get { return this.clamp; }
            set
            {
                this.clamp = value;
            }
        }

        public Matrix Transform { get { return this.transform; } }
        #endregion

        #region Constructor
        public Camera2D(Vector2 position, Rectangle clamp, GraphicsDevice device)
        {
            this.zoom = 1f;
            this.rotation = 0f;
            this.position = position;
            this.oldPosition = position + new Vector2(1, 1);
            this.cacheRect = new Rectangle();
            this.graphicsDevice = device;

            this.Clamp = clamp;

            // Do initial transform
            this.Transformation();
        }
        #endregion


        public void Move(Vector2 amount)
        {
            this.Position += amount;
        }

        protected void Transformation()
        {
            this.transform =
                Matrix.Identity *
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        }

        /// <summary>
        /// Gets the world position for a given vector
        /// </summary>
        /// <param name="position">Screen vector to change to world position</param>
        /// <returns>A vector containing the world position</returns>
        public Vector2 GetWorldPosition(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(this.transform));
        }

        /// <summary>
        /// Gets the world position for a given vector
        /// </summary>
        /// <param name="position">Screen rectangle to change to world position</param>
        /// <returns>A rectangle containing the world position</returns>
        public Rectangle GetWorldPositionRect(Rectangle position)
        {
            Vector2 worldPos = Vector2.Transform(new Vector2(position.X, position.Y), Matrix.Invert(this.transform));
            cacheRect.X = (int)worldPos.X;
            cacheRect.Y = (int)worldPos.Y;
            cacheRect.Width = position.Width;
            cacheRect.Height = position.Height;

            return cacheRect;
        }

        /// <summary>
        /// Gets a vectors screen position
        /// </summary>
        /// <param name="position">The vector to change to local position</param>
        /// <returns>The local position vector</returns>
        public Vector2 GetScreenPosition(Vector2 position)
        {
            return Vector2.Transform(position, this.transform);
        }
    }
}
