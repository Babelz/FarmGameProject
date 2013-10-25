using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Khv.Gui.Components.Navigation
{
    /// <summary>
    /// Luokka joka sisältää keyboard
    /// keyn ja pressed staten.
    /// </summary>
    public class KeyTrigger
    {
        #region Properties
        public Keys Key
        {
            get;
            set;
        }
        public PressedState CurrentState
        {
            get;
            set;
        }
        #endregion

        public KeyTrigger(Keys key)
        {
            Key = key;
            CurrentState = PressedState.None;
        }
    }
}
