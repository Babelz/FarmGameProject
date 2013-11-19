using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Engine
{
    public abstract class KhvGame : Microsoft.Xna.Framework.Game
    {
        #region Vars
        protected GameStateManager gameStateManager;
        #endregion

        #region Properties
        /// <summary>
        /// staattinen tekstuuri jota käytetään ympäri moottoria 
        /// kun ei haluta piirtää "mitään"
        /// </summary>
		public static Texture2D Temp
		{
			get;
			protected set;
		}
		public GameStateManager GameStateManager
        {
            get
            {
                return gameStateManager;
            }
        }
        public TestInputManager InputManager
        {
            get;
            private set;
        }
        public abstract GraphicsDeviceManager GraphicsDeviceManager
        {
            get;
        }
		#endregion
		
		#region Overrided members
        protected override void Initialize()
        {
            // hookataan input manageri että ei tarvi joka pelissä erikseen :)
            gameStateManager = new GameStateManager(this);
            InputManager inputManager = new InputManager(this);
            InputManager = new TestInputManager(this);
            Components.Add(InputManager);
            Components.Add(inputManager);
            Components.Add(gameStateManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Temp texture making
            KhvGame.Temp = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[KhvGame.Temp.Width * KhvGame.Temp.Height];
            for (int i = 0; i < data.Length; data[i++] = Color.White) ;
            KhvGame.Temp.SetData<Color>(data);
            #endregion

            base.LoadContent();
        }
		#endregion
    }
}
