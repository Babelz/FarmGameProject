using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.World;
using Khv.Engine.State;
using Microsoft.Xna.Framework;

namespace Farmi.Screens
{
    class GameplayScreen : GameState
    {
        #region Properties

        public FarmWorld World
        {
            get;
            private set;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            World = new FarmWorld(Game);
            World.Initialize();
        }

        public override void LoadContent()
        {
            
        }

        public override void Draw()
        {
            SpriteBatch.GraphicsDevice.Clear(Color.Coral);
        }

        #endregion
    }
}
