using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Khv.Gui.Components.Navigation
{
    public class NavigationKeyTrigger : KeyTrigger
    {
        #region Vars
        public PressedState lastState;
        #endregion

        public NavigationKeyTrigger(Keys key)
            : base(key)
        {
            // suora super kutsu
        }
        // palauttaa booleanin pressedstaten perusteella
        public bool IsPressed()
        {
            if (CurrentState == PressedState.Pressed && lastState != CurrentState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
