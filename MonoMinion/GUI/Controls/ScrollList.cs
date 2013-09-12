using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMinion.Input.Handlers;
using MonoMinion.Helpers;

namespace MonoMinion.GUI.Controls
{
    /// <summary>
    /// A Scroll List is a scrollable item holder
    /// </summary>
    public class ScrollList : Control
    {
        #region Variables and Properties
        protected Texture2D buttonPrevious;
        protected Texture2D buttonNext;
        protected Point buttonSize;

        private int _page;
        /// <summary>
        /// Returns the current Page being viewed
        /// </summary>
        public int Page
        {
            get { return this._page; }
            set
            {

                // Make sure we never go over the item count or under 1
                if (value > Pages)
                    this._page = 1;
                else if (value < 1)
                    if (this.items.Count > 0)
                        this._page = Pages;
                    else
                        this._page = 1;
                else
                    this._page = value;

            }
        }

        /// <summary>
        /// Number of pages within the Scroll List
        /// </summary>
        public int Pages
        {
            get
            {
                int pages = 1;
                if (this.items.Count > 0 && this.pageSize > 0)
                    pages = (int)Math.Ceiling((double)this.items.Count / this.pageSize);

                return pages;
            }
        }

        protected bool isVertical;
        public bool IsVertical
        {
            get { return this.isVertical; }
            set
            {
                this.isVertical = value;
                this.UpdateScreenPositions();
            }
        }

        protected List<ScrollItem> items;
        public List<ScrollItem> Items { get { return this.items; } }

        /// <summary>
        /// Returns the size of the Scroll List
        /// </summary>
        public override Point Size
        {
            get
            {
                Point sizeVector = new Point(itemSize.X, itemSize.Y);
                if (IsVertical)
                    sizeVector.Y = ((buttonSize.Y + Margin) * 2) + itemSize.Y * Math.Min(items.Count, this.pageSize) + (Math.Min(items.Count, this.pageSize) * Margin);
                else
                    sizeVector.X = ((buttonSize.X + Margin) * 2) + itemSize.X * Math.Min(items.Count, this.pageSize) + (Math.Min(items.Count, this.pageSize) * Margin);

                return sizeVector;
            }
        }
        protected Point itemSize;
        protected int pageSize;
        protected int selectedItem;

        public int Margin;
        public Color ColorHover;
        public Color ColorOn;
        public Color ColorOff;

        new public event EventHandler Selected;
        #endregion

        #region ScrollItem Class
        /// <summary>
        /// The Scroll Item class is a holder class for items added to the scroll list 
        /// </summary>
        public class ScrollItem
        {
            public string Name;
            public Texture2D Thumbnail;
            public object Data;
            public Vector2 Position;

            //public event EventHandler Selected;

            /// <summary>
            /// ScrollItem Constructor
            /// </summary>
            /// <param name="name">Name of ScrollItem</param>
            /// <param name="thumbnail">Thumbnail for ScrollItem</param>
            /// <param name="data">Data object to include with ScrollItem</param>
            public ScrollItem(string name, Texture2D thumbnail, object data)
            {
                this.Name = name;
                this.Thumbnail = thumbnail;
                this.Data = data;
                this.Position = Vector2.Zero;
            }
            /*
            /// <summary>
            /// On Selected event trigger
            /// </summary>
            /// <param name="e">Event Arguments</param>
            public virtual void OnSelected(EventArgs e)
            {
                if (Selected != null)
                    Selected(this, e);
            }
            */
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a Scroll List
        /// </summary>
        /// <param name="name">Name of ScrollList</param>
        /// <param name="color">Background Color</param>
        /// <param name="previousBtn">Texture for the Previous button</param>
        /// <param name="nextBtn">Texture for the Next button</param>
        /// <param name="btnSize">Size of each button</param>
        /// <param name="itemSize">Size of each item in the list</param>
        public ScrollList(string name, Color color, Texture2D previousBtn, Texture2D nextBtn, Point btnSize, Point itemSize)
            : base(name, color)
        {
            this.buttonSize = btnSize;
            this.buttonPrevious = previousBtn;
            this.buttonNext = nextBtn;

            this.itemSize = itemSize;
            this.items = new List<ScrollItem>();
            this.IsVertical = true;
            this.pageSize = 6;
            this.Page = 1;
            this.selectedItem = 0;
            this.Margin = 10;
            this.ColorOn = color;
            this.ColorOff = Color.White;
            this.ColorHover = Color.Red;
        }

        /// <summary>
        /// Create a Scroll List
        /// </summary>
        /// <param name="name">Name of ScrollList</param>
        /// <param name="color">Background Color</param>
        /// <param name="previousBtn">Texture for the Previous button</param>
        /// <param name="nextBtn">Texture for the Next button</param>
        /// <param name="btnSize">Size of each button</param>
        /// <param name="itemSize">Size of each item in the list</param>
        /// <param name="numberOfItems">Number of items per page</param>
        public ScrollList(string name, Color color, Texture2D previousBtn, Texture2D nextBtn, Point btnSize, Point itemSize, int numberOfItems)
            : this(name, color, previousBtn, nextBtn, btnSize, itemSize)
        {
            this.pageSize = numberOfItems;
        }

        /// <summary>
        /// Create a Scroll List
        /// </summary>
        /// <param name="name">Name of ScrollList</param>
        /// <param name="color">Background Color</param>
        /// <param name="previousBtn">Texture for the Previous button</param>
        /// <param name="nextBtn">Texture for the Next button</param>
        /// <param name="btnSize">Size of each button</param>
        /// <param name="itemSize">Size of each item in the list</param>
        /// <param name="numberOfItems">Number of items per page</param>
        /// <param name="hover">Tint for when item is being hovered over</param>
        /// <param name="on">Tint for when item is selected</param>
        /// <param name="off">Tint for items that are not being hovered over or selected</param>
        public ScrollList(string name, Color color, Texture2D previousBtn, Texture2D nextBtn, Point btnSize, Point itemSize, int numberOfItems, Color hover, Color on, Color off)
            : this(name, color, previousBtn, nextBtn, btnSize, itemSize, numberOfItems)
        {
            this.ColorHover = hover;
            this.ColorOn = on;
            this.ColorOff = off;
        }
        #endregion


        #region Overrideable Functions
        /// <summary>
        /// Update override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {

        }


        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            // First render the Previous and Next buttons
            this.drawNavigation();

            // Crawl backwards through list
            for (int i = Math.Min(this.items.Count, this.pageSize * this.Page); i > this.pageSize * (this.Page - 1); i--)
                Minion.Instance.SpriteBatch.Draw(this.items[i - 1].Thumbnail, this.items[i - 1].Position, this.ColorOff);
        }


        /// <summary>
        /// HandleInput override
        /// </summary>
        public override void HandleInput()
        {
            // TODO: Make button check more modular (Left/right/middle/etc)
            // If Left mouse button is clicked
            bool isLeftMouseBtnClicked = MouseHandler.IsMouseDown(MouseHandler.Buttons.Left);
            if (isLeftMouseBtnClicked || MouseHandler.IsHeld(MouseHandler.Buttons.Left))
            {
                // Check if either the next or previous buttons are being clicked
                if (isLeftMouseBtnClicked)
                    if (this._checkMouseClick())
                        return; // No need to continue

                // Scroll through each item in the list
                for (int i = 0; i < this.items.Count; i++)
                {
                    // And check if any one of the intersects the mouse cursor bounding box
                    Rectangle position = new Rectangle((int)this.items[i].Position.X, (int)this.items[i].Position.Y, this.itemSize.X, this.itemSize.Y);
                    if (CollisionHelper.DoRectsIntersect(position, MouseHandler.PositionRect))
                    {
                        // Set the current selected item, fire off selected event and break out of the for loop
                        this.selectedItem = i;
                        this.OnSelected(null);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Overrides OnSelected handler
        /// </summary>
        /// <param name="e">Event Arguments</param>
        protected override void OnSelected(EventArgs e)
        {
            if (Selected != null)
                Selected(this.items[this.selectedItem], e);
        }
        #endregion

        #region ScrollList Add/Remove Functions
        /// <summary>
        /// Add an item to the ScrollList
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="thumb">Thumnail for item</param>
        /// <param name="data">Data object for item</param>
        public void AddItem(string name, Texture2D thumb, object data)
        {
            // Add and update positions
            this.items.Add(new ScrollItem(name, thumb, data));
            this.UpdateScreenPositions();
        }

        /// <summary>
        /// Remove item from ScrollList
        /// </summary>
        /// <param name="position">Position of the item to be removed</param>
        public void RemoveItem(int position)
        {
            // Remove and update positions
            this.items.RemoveAt(position);
            this.UpdateScreenPositions();
        }
        #endregion

        #region ScrollList Helpers
        /// <summary>
        /// Updates the on Screen Positions of Items
        /// </summary>
        protected virtual void UpdateScreenPositions()
        {
            // Let's make sure that neither of these variables is set to zero
            if (this.Page > 0 && this.pageSize > 0)
            {
                // Offset for rendering position
                int offset = 0;

                // Crawl through list and render the maximum amount of items available or allowed
                for (int i = (this.pageSize * (this.Page - 1)); i < Math.Min(this.items.Count, this.pageSize * this.Page); i++)
                {
                    if (this.IsVertical)
                    {
                        // Vertical List Rendering
                        this.items[i].Position = new Vector2(
                            this.Position.X,
                            (this.Position.Y + this.buttonSize.Y + this.Margin) + (offset * (this.itemSize.Y + this.Margin))
                        );
                    }
                    else
                    {
                        // Horizontal List Rendering
                        this.items[i].Position = new Vector2(
                            (this.Position.X + this.buttonSize.X + this.Margin) + (offset * (this.itemSize.X + this.Margin)),
                            this.Position.Y
                        );
                    }
                    offset++;
                }
            }
        }

        /// <summary>
        /// Renders navigation buttons
        /// </summary>
        protected virtual void drawNavigation()
        {
            // Previous
            Minion.Instance.SpriteBatch.Draw(
                this.buttonPrevious,
                new Rectangle((int)this.Position.X, (int)this.Position.Y, this.buttonSize.X, this.buttonSize.Y),
                this.ColorOff
            );

            if (this.isVertical)
            {
                // Next
                Minion.Instance.SpriteBatch.Draw(
                    this.buttonNext,
                    new Rectangle(
                        (int)this.Position.X,
                        (int)(this.Position.Y + this.buttonSize.Y + this.Margin) + ((this.itemSize.Y + this.Margin) * this.pageSize),
                        this.buttonSize.X,
                        this.buttonSize.Y),
                    this.ColorOff
                );
            }
            else
            {
                // Next
                Minion.Instance.SpriteBatch.Draw(
                    this.buttonNext,
                    new Rectangle(
                        (int)(this.Position.X + this.buttonSize.X + this.Margin) + ((this.itemSize.X + this.Margin) * this.pageSize),
                        (int)this.Position.Y,
                        this.buttonSize.X,
                        this.buttonSize.Y),
                    this.ColorOff
                );
            }
        }

        /// <summary>
        /// Wrapper for Mouse Click checking
        /// </summary>
        /// <returns>True if any click was found</returns>
        private bool _checkMouseClick()
        {
            // Next
            Rectangle tempBtnRect;
            if (this.isVertical)
            {
                tempBtnRect = new Rectangle(
                    (int)this.Position.X,
                    (int)(this.Position.Y + this.buttonSize.Y + this.Margin) + ((this.itemSize.Y + this.Margin) * this.pageSize),
                    this.buttonNext.Width,
                    this.buttonNext.Height
                );
            }
            else
            {
                tempBtnRect = new Rectangle(
                    (int)(this.Position.X + this.buttonSize.X + this.Margin) + ((this.itemSize.X + this.Margin) * this.pageSize),
                    (int)this.Position.Y,
                    this.buttonNext.Width,
                    this.buttonNext.Height
                );
            }
            if (CollisionHelper.DoRectsIntersect(tempBtnRect, MouseHandler.PositionRect))
            {
                this.Page++;
                this.UpdateScreenPositions();

                return true;
            }

            // Previous
            tempBtnRect = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                this.buttonPrevious.Width,
                this.buttonPrevious.Height
            );
            if (CollisionHelper.DoRectsIntersect(tempBtnRect, MouseHandler.PositionRect))
            {
                this.Page--;
                this.UpdateScreenPositions();

                return true;
            }

            return false;
        }
        #endregion
    }
}
