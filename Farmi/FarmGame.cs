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
#endregion

namespace Farmi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FarmGame : KhvGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

            InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("Debug exit", Keys.Escape), (triggered, args) => Exit() 
                );
            InputManager.Mapper.GetInputBindProvider<PadInputBindProvider>().Map(
                new ButtonTrigger("Debug exit", Buttons.Back), (triggered, args) => Exit()
                );

            ScriptEngine engine = new ScriptEngine(this, Path.Combine("cfg", "sengine.cfg"));
            Components.Add(engine);

            GameStateManager.ChangeState(new GameplayScreen());

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
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

    }
}
