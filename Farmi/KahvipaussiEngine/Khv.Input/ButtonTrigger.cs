using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Khv.Input
{
    public class ButtonTrigger : ITrigger
    {
        #region Properties
        public string Name { get; private set; }
        public Buttons[] AlternateButtons { get; private set; }
        public Buttons Button { get; private set; }
        #endregion

        public ButtonTrigger(string name, Buttons button, params Buttons[] alts)
        {
            Name = name;
            Button = button;
            if (alts == null)
                alts = new Buttons[0];
            AlternateButtons = alts;
        }
    }
}
