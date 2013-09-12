using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoMinion.GUI
{
    /// <summary>
    /// Abstract Control class for GUI
    /// </summary>
    public abstract class Control
    {
        #region Variables and Properties
        public string Name { get; set; }
        protected Point size;
        public virtual Point Size
        {
            get { return size; }
            set
            {
                size = value;
                boundingBox.Width = size.X;
                boundingBox.Height = size.Y;
            }
        }
        protected Vector2 position;
        public virtual Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                boundingBox.X = (int)position.X;
                boundingBox.Y = (int)position.Y;
            }
        }
        public object Value { get; set; }

        public bool TabStop { get; set; }

        private bool _hasFocus;
        public virtual bool HasFocus
        {
            get { return _hasFocus; }
            set { _hasFocus = value; }
        }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public bool IsStatic { get; set; }

        public SpriteFont Font { get; set; }
        public Color BackgroundColor { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// Returns a positional Rectangle for collision detection
        /// </summary>
        public virtual Rectangle BoundingBox
        {
            get
            {
                boundingBox.X = (int)Position.X;
                boundingBox.Y = (int)Position.Y;
                return boundingBox;
            }
        }
        protected Rectangle boundingBox;

        public event EventHandler Selected;
        #endregion

        /// <summary>
        /// Control constructor
        /// </summary>
        /// <param name="name">Name of the control</param>
        /// <param name="color">Background color for the control</param>
        public Control(string name, Color color)
        {
            this.Name = name;
            this.BackgroundColor = color;
            this.IsEnabled = true;
            this.IsVisible = true;
            this.TabStop = false;
            this.HasFocus = false;
            this.IsStatic = true;

            this.boundingBox = Rectangle.Empty;
            this.Size = Point.Zero;
            this.Position = Vector2.Zero;
            this.Value = null;

            this.Font = ControlManager.SpriteFont;
        }

        #region Abstract Functions
        /// <summary>
        /// Update all game entities
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw all entities
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Handle all control input
        /// </summary>
        public abstract void HandleInput();
        #endregion

        #region Virtual Functions
        /// <summary>
        /// Overridable function trigger for Selected
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
                Selected(this, e);
        }
        #endregion
    }
}
