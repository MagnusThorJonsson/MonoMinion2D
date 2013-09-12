using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMinion.Input.Handlers;
using MonoMinion.Helpers;

namespace MonoMinion.GUI.Controls
{
    /// <summary>
    /// Container for drag & drop items
    /// </summary>
    public class DropContainer : Control
    {
        #region Variables and Properties
        protected Texture2D backgroundImage;

        protected DropItem item;
        public DropItem Item { get { return this.item; } }
        #endregion

        #region DropItem Class
        /// <summary>
        /// Wrapper class for items to be dropped into the DropContainer
        /// </summary>
        public class DropItem
        {
            #region DropItem Variables
            protected string id;
            public string Id { get { return this.id; } }

            protected string type;
            public string Type { get { return this.type; } }

            protected Texture2D thumbnail;
            public Texture2D Thumbnail { get { return this.thumbnail; } }

            protected object data;
            public object Data { get { return this.data; } }
            #endregion

            /// <summary>
            /// Constructor for DropItem class
            /// </summary>
            /// <param name="id">Identifier for object</param>
            /// <param name="thumb">Thumbnail for display</param>
            /// <param name="data">Object wrapper for data to be contained in DropItem</param>
            public DropItem(string id, string type, Texture2D thumb, object data)
            {
                this.id = id;
                this.type = type;
                this.thumbnail = thumb;
                this.data = data;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// DropContainer constructor
        /// </summary>
        /// <param name="name">Name of DropContainer</param>
        /// <param name="color">Color tint for background image</param>
        /// <param name="type">Type of items the DropContainer can handle</param>
        /// <param name="size">Size of DropContainer</param>
        /// <param name="background">Background texture</param>
        public DropContainer(string name, Color color, string type, Point size, Texture2D background)
            : base(name, color)
        {
            this.Type = type;
            this.backgroundImage = background;
            this.Size = size;
            this.item = null;
        }
        #endregion

        #region Overrideable Functions
        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Draw background image
            Minion.Instance.SpriteBatch.Draw(
                this.backgroundImage,
                new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Size.X, this.Size.Y),
                new Rectangle(0, 0, this.backgroundImage.Width, this.backgroundImage.Height),
                this.BackgroundColor
            );

            // If there is an item in the container we draw that item
            if (this.item != null && this.item is DropItem)
            {
                Minion.Instance.SpriteBatch.Draw(
                    this.item.Thumbnail,
                    new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Size.X, this.Size.Y),
                    new Rectangle(0, 0, this.item.Thumbnail.Width, this.item.Thumbnail.Height),
                    this.BackgroundColor
                );
            }
        }

        /// <summary>
        /// Overrides the automatic input handling
        /// </summary>
        public override void HandleInput()
        {
            if (this.item != null &&
                MouseHandler.IsMouseDown(MouseHandler.Buttons.Left) &&
                this.BoundingBox.Intersects(MouseHandler.PositionRect))
            {
                Console.WriteLine(this.item.Id + "<--");
            }
        }

        /// <summary>
        /// Returns a copy of the current object
        /// </summary>
        /// <returns>DropContainer copy</returns>
        public virtual DropContainer Copy()
        {
            DropContainer copy = new DropContainer(this.Name, this.BackgroundColor, this.Type, this.Size, this.backgroundImage);
            copy.Position = this.Position;
            copy.item = this.item;
            return copy;
        }
        #endregion

        #region DropItem Helpers
        /// <summary>
        /// Adds a new item to DropItem container
        /// </summary>
        /// <param name="id">Identifier for item</param>
        /// <param name="type">Type of item</param>
        /// <param name="thumb">Thumbnail for item</param>
        /// <param name="data">Data hooked to item</param>
        public void AddItem(string id, string type, Texture2D thumb, object data)
        {
            this.item = new DropItem(id, type, thumb, data);
        }

        /// <summary>
        /// Adds a new item to DropItem container
        /// </summary>
        /// <param name="item">DropItem to add to the container</param>
        public void AddItem(DropItem item)
        {
            this.item = item;
        }

        /// <summary>
        /// Clears the DropItem from the Container
        /// </summary>
        public void Clear()
        {
            this.item = null;
        }
        #endregion
    }
}
