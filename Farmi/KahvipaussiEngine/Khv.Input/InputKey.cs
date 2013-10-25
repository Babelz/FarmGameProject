using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Khv.Engine.Args;
using Khv.Engine;
using Khv.Game.GameObjects;
using Khv.Game;

namespace Khv.Input
{
	public class InputKey
    {
        #region Vars
        // primary napit
        private Keys key;
        private Buttons button;

        // vaihtoehtoiset napit
        private Keys[] alternateKeys;
        private Buttons[] alternateButtons;
		private string name;

        #endregion

        #region Events
   
        /// <summary>
        /// Kutsutaan kun nappia on painettu
        /// </summary>
		public event KeyPressedHandler OnKeyPressed;

        #endregion

        #region Constructor

        /// <summary>
        /// Luo uuden InputKeyn ensisijaisella napilla ja gamepad napilla
        /// </summary>
        /// <param name="name">Napin nimi</param>
        /// <param name="key">Ensisijainen näppäin</param>
        /// <param name="button">Ensisijainen gamepad näppäin</param>
        public InputKey(string name, Keys key, Buttons button) : this(name, key, button, null, null)
        {
        }

        /// <summary>
        /// Luo uuden InputKeyn ensisijaisella napilla, gamepad napilla
        /// ja mahdollisilla vaihtoehtoisilla napeilla
        /// </summary>
        /// <param name="name">Napin nimi</param>
        /// <param name="key">Ensisijainen näppäin</param>
        /// <param name="button">Ensisijainen gamepad näppäin</param>
        /// <param name="alternateKeys">Vaihtoehtoiset näppäimet</param>
        /// <param name="alternateButtons">Vaihtoehtoiset gamepad näppäimet</param>
		public InputKey (string name, Keys key, Buttons button, Keys[] alternateKeys, Buttons[] alternateButtons)
		{
			this.key = key;
			this.name = name;
            this.button = button;
            this.alternateButtons = alternateButtons;
            this.alternateKeys = alternateKeys;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Tarkastaa onko ensisijaisia nappeja tai vaihtoehtoisia nappeja
        /// painettu
        /// </summary>
        /// <param name="index">Gamepad pelaajan ohjain indeksi, jos ei ole gampedia voi olla null</param>
        /// <returns>true jos jokin napeista on painettu, false muuten</returns>
        public bool IsPressed(Microsoft.Xna.Framework.PlayerIndex? index)
		{
            if (InputManager.KeyDown(key))
                    return true;
            if (alternateKeys != null)
            {
                foreach (Keys newKey in alternateKeys)
                {
                    if (InputManager.KeyDown(newKey))
                        return true;
                }
            }
            if (index.HasValue)
            {
                               
                if (InputManager.IsButtonDown(button, index.Value))
                    return true;
                
                if (alternateButtons != null)
                {
                    foreach (Buttons newButton in alternateButtons)
                    {
                        if (InputManager.IsButtonDown(newButton, index.Value))
                            return true;
                    }
                }
            } 

            return false;
		}
        /// <summary>
        /// Tarkastelee onko nappia painettu, jos on niin
        /// laukaisee OnKeyPressed
        /// </summary>
        /// <param name="time">Time.</param>
        /// <param name="target">Kenen controller</param>
		public void Update(GameTime time, Player target)
		{

            
            if (IsPressed(target.PlayerIndex))
            {
                if (OnKeyPressed != null)
                    OnKeyPressed(this, new KeyPressEventArgs(time));
            }
        }

        #endregion
    }
    public class KeyPressEventArgs : GameEventArgs
    {
        #region Properties
        public GameTime GameTime
        {
            get;
            private set;
        }
        #endregion

        public KeyPressEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }

    public delegate void KeyPressedHandler(object sender, KeyPressEventArgs e);
}

