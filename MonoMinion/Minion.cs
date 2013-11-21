#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using MonoMinion.Input.Handlers;
using MonoMinion.Components;
#endregion

namespace MonoMinion
{
    /// <summary>
    /// Minion is a lightweight and simple 2D engine
    /// </summary> 
    public abstract class Minion : Game
    {
        #region Config
        public static readonly string TILESHEET_PATH = "Content\\World\\Tilesets\\";
        #endregion

        #region Variables and Properties
        public static Minion Instance;
        
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected Point screenSize;

        /// <summary>
        /// The GUI font
        /// </summary>
        public SpriteFont FontGui
        {
            get
            {
                return this.fontGui;
            }
        }
        protected SpriteFont fontGui;
        
        /// <summary>
        /// The SpriteBatch
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        protected SpriteBatch spriteBatch;
        
        protected GameScreen startScreen;
        public GameManager GameManager { get; set; }
        public TimerManager TimerManager { get; internal set; }

#if !XBOX
        public KeyboardHandler InputHandler { get; set; }
        public MouseHandler MouseHandler { get; set; }
#endif
        
        /// <summary>
        /// A 1x1 texture block
        /// </summary>
        public Texture2D Texture1x1 { get { return this._texture1x1; } }
        private Texture2D _texture1x1;
        #endregion

        /// <summary>
        /// Minion 2D constructor
        /// </summary>
        /// <param name="screenSize">A Point representation of the initial screen size</param>
        public Minion(Point screenSize)
            : base()
        {
            this.content = Content;
            this.graphics = new GraphicsDeviceManager(this);

            this.screenSize = screenSize;
            this.IsFixedTimeStep = true;
            Minion.Instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = screenSize.X;
            this.graphics.PreferredBackBufferHeight = screenSize.Y;
            this.graphics.ApplyChanges();

            // Add and initialize game components
            this.Components.Add(this.GameManager = new GameManager(this));
            this.Components.Add(this.TimerManager = new TimerManager(this));
#if !XBOX
            this.Components.Add(this.MouseHandler = new MouseHandler(this));
            this.Components.Add(this.InputHandler = new KeyboardHandler(this));
#endif

#if DEBUG
            this.Components.Add(new FrameRateCounter(this));
#endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create 1x1 texture
            _texture1x1 = new Texture2D(Minion.Instance.GraphicsDevice, 1, 1);
            _texture1x1.SetData(new[] { Color.White });

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.spriteBatch = null;

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets the background color for the screen refresh
        /// </summary>
        /// <param name="color">The color to set as background</param>
        protected void SetBackgroundColor(Color color)
        {
            if (GameManager != null)
                GameManager.BackgroundColor = color;
        }
    }
}
