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

