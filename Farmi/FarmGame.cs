#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using Farmi.Screens;
using Khv.Engine;
using Khv.Input;
using Khv.Scripts.CSharpScriptEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Farmi.Repositories;
using Farmi.Datasets;
using Farmi.Calendar;
#endregion

namespace Farmi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    internal sealed class FarmGame : KhvGame
    {
        #region Vars
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        #endregion

        public FarmGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            InputManager.AddStateProvider(typeof(KeyboardStateProvider), new KeyboardStateProvider());
            InputManager.AddStateProvider(typeof(GamepadStateProvider), new GamepadStateProvider());
            InputManager.Mapper.AddInputBindProvider(typeof(KeyInputBindProvider), new KeyInputBindProvider());
            InputManager.Mapper.AddInputBindProvider(typeof(PadInputBindProvider), new PadInputBindProvider());

            CalendarSystem calendar = new CalendarSystem(this, 15);
            Components.Add(calendar);

            calendar.OnYearChanged += new CalendarEventHandler(calendar_OnYearChanged);
            calendar.OnSeasonChanged += new CalendarEventHandler(calendar_OnSeasonChanged);
            calendar.OnDayChanged += new CalendarEventHandler(calendar_OnDayChanged);
            calendar.OnClockTick += new CalendarEventHandler(calendar_OnTimeTick);
            
            // TODO: Debug logger.
            ScriptEngine engine = new ScriptEngine(this, Path.Combine("cfg", "sengine.cfg"));
            engine.LoggingMethod = LoggingMethod.Console;
            Components.Add(engine);

            InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("Debug exit", Keys.Escape), (triggered, args) => Exit() 
                );
            InputManager.Mapper.GetInputBindProvider<PadInputBindProvider>().Map(
                new ButtonTrigger("Debug exit", Buttons.Back), (triggered, args) => Exit()
                );
            
            Components.Add(new RepositoryManager(@"\dat\repos",
                           new string[] { "Farmi.Datasets." },
                           new string[] { "Farmi.Repositories." }));
            GameStateManager.ChangeState(new GameplayScreen());

            
        }

        void calendar_OnDayChanged(object sender, CalendarEventArgs e)
        {
            Console.WriteLine("Päivä vaihtu...");
        }

        void calendar_OnSeasonChanged(object sender, CalendarEventArgs e)
        {
            Console.WriteLine("Season vaihtu..");
        }

        void calendar_OnTimeTick(object sender, CalendarEventArgs e)
        {
            Console.WriteLine("Aika muuttu...");
        }

        void calendar_OnYearChanged(object sender, CalendarEventArgs e)
        {
            Console.WriteLine("Vuosi vaihtu...");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            base.LoadContent();
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {    
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
