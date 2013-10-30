using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Khv.Game.GameObjects;
using Khv.Game;

namespace Khv.Input
{
	public class InputControlSetup
    {
        #region Vars

        #endregion

        #region Constructor

        public InputControlSetup ()
		{
            Mapper = new InputMapper();
            Mapper.AddInputBindProvider(typeof(KeyInputBindProvider), new KeyInputBindProvider());
            Mapper.AddInputBindProvider(typeof(PadInputBindProvider), new PadInputBindProvider());
		}

        #endregion 

        #region Methods

        #endregion

        #region Properties

        public InputMapper Mapper
        {
            get;
            private set;
        }

        #endregion
    }
}

