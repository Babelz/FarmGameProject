using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Khv.Engine
{
    [System.Obsolete("deprecated")]
	public class InputManager : GameComponent
    {
        #region Vars
        // nykyinen state
        private static KeyboardState newState;
        // viime state
		private static KeyboardState oldState;

        // nykyiset ohjaimien statet
        private static GamePadState[] newGamePadStates;
        // viime ohjaimien statet
        private static GamePadState[] oldGamePadStates;

        #endregion

        #region Constructor
        /// <summary>
        /// Luo uuden InputManagerin GameComponenttina
        /// </summary>
        /// <param name="game">Peli.</param>
        public InputManager(KhvGame game)
            : base(game)
		{
            Current = Keys.None;
            Last = Current;
			newState = Keyboard.GetState ();
            // pit‰‰ katsoa ett‰ saadaan kaikkien pelaajien ohjaimet
            newGamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];
            
            // t‰ytet‰‰n
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                newGamePadStates[(int)index] = GamePad.GetState(index);
            }
		}
        #endregion

        #region Override
        /// <summary>
        /// P‰ivitt‰‰ gamepadin ja n‰pp‰imistˆn stateja
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update (GameTime gameTime)
		{
            Last = Current;


            oldGamePadStates = (GamePadState[])newGamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                newGamePadStates[(int)index] = GamePad.GetState(index);
            }


            // Keyboardin staten luku.
			oldState = newState;
			newState = Keyboard.GetState ();

            if (newState.GetPressedKeys().Length > 0)
            {
                Current = newState.GetPressedKeys()[0];
            }
            else
            {
                Current = Keys.None;
            }

            // Mouse staten luku.
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            MousePosition = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            

			base.Update (gameTime);
		}
        #endregion

        #region Static methods
        /// <summary>
        /// Hakee nykyisen tilanteen
        /// </summary>
        public static void Flush()
        {
            newState = Keyboard.GetState();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                newGamePadStates[(int)index] = GamePad.GetState(index);
            }
            oldGamePadStates = (GamePadState[])newGamePadStates.Clone();
            oldState = newState;
        }

        /// <summary>
        /// Tarkistaa onko nappia painettu
        /// </summary>
        /// <param name="key">Nappi.</param>
        /// <returns>True jos nappia on painettu, false muuten</returns>
		public static bool IsKeyPressed(Keys key)
		{
			return newState.IsKeyDown(key) && oldState.IsKeyUp(key);
		}

        /// <summary>
        /// Tarkistaa onko nappi p‰‰stetty irti
        /// </summary>
        /// <param name="key">Nappi.</param>
        /// <returns>True jos nappi ei ole pohjassa, false muuten</returns>
		public static bool IsKeyReleased(Keys key)
		{
			return newState.IsKeyUp(key) && oldState.IsKeyDown(key);
		}

        /// <summary>
        /// Tarkistaa painetaanko nappia kokoajan
        /// </summary>
        /// <param name="key">Nappi</param>
        /// <returns>True jos nappi on pohjassa</returns>
		public static bool KeyDown(Keys key)
		{
			return newState.IsKeyDown (key);
		}
        /// <summary>
        /// Tarkistaa onko n‰pp‰int‰ painettu
        /// </summary>
        /// <param name="button">N‰pp‰in.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos on</returns>
        public static bool IsButtonPressed(Buttons button, PlayerIndex index)
        {
            return newGamePadStates[(int)index].IsButtonDown(button) &&
                oldGamePadStates[(int)index].IsButtonUp(button);
        }

        /// <summary>
        /// Tarkistaa onko n‰pp‰in ylh‰‰ll‰
        /// </summary>
        /// <param name="button">N‰pp‰in.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos on</returns>
        public static bool IsButtonReleased(Buttons button, PlayerIndex index)
        {
            return newGamePadStates[(int)index].IsButtonUp(button) &&
                oldGamePadStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// Tarkistaa painetaanko n‰pp‰int‰
        /// </summary>
        /// <param name="button">N‰pp‰in.</param>
        /// <param name="index">Ohjaimen indeksi</param>
        /// <returns>True jos painetaan</returns>
        public static bool IsButtonDown(Buttons button, PlayerIndex index)
        {
            return newGamePadStates[(int)index].IsButtonDown(button);
        }
        #endregion

        #region Gui depency fields
        public static KeyboardState KeyboardState
        {
            get { return newState; }
        }

        public static Keys Current
        {
            get;
            private set;
        }

        public static Keys Last
        {
            get;
            private set;
        }
        public static Point MousePosition
        {
            get;
            private set;
        }
        public static MouseState CurrentMouseState
        {
            get;
            private set;
        }
        public static MouseState LastMouseState
        {
            get;
            private set;
        }
        #endregion
    }
}

