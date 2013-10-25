using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Khv.Engine;

namespace Khv.Gui.Components.Navigation
{
    /// <summary>
    /// luokka joka helpottaa navigoinnin toteutusta 
    /// käyttöliittymässä
    /// </summary>
    public class NavigationTriggers
    {
        #region Vars
        private List<NavigationKeyTrigger> triggers;
        #endregion

        #region Properties
        public NavigationKeyTrigger Up
        {
            get;
            private set;
        }
        public NavigationKeyTrigger Down
        {
            get;
            private set;
        }
        public NavigationKeyTrigger Left
        {
            get;
            private set;
        }
        public NavigationKeyTrigger Right
        {
            get;
            private set;
        }
        #endregion

        public NavigationTriggers()
        {
            triggers = new List<NavigationKeyTrigger>();
            triggers.Add(Up = new NavigationKeyTrigger(Keys.Up));
            triggers.Add(Down = new NavigationKeyTrigger(Keys.Down));
            triggers.Add(Left = new NavigationKeyTrigger(Keys.Left));
            triggers.Add(Right = new NavigationKeyTrigger(Keys.Right));
        }
        /// <summary>
        /// päivittää luokan triggereiden tilaa sitä
        /// mukaa kun triggerin näppäintä painetaan
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].lastState = triggers[i].CurrentState;
                if (InputManager.Current == triggers[i].Key)
                {
                    if (triggers[i].CurrentState == PressedState.None)
                    {
                        triggers[i].CurrentState = PressedState.Pressed;
                    }
                }
                else if (triggers[i].CurrentState == PressedState.Pressed)
                {
                    triggers[i].CurrentState = PressedState.Released;
                }
                else
                {
                    triggers[i].CurrentState = PressedState.None;
                }
            }
        }
    }
}
