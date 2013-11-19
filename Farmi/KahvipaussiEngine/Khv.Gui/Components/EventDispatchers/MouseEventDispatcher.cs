using Khv.Engine.Args;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Components.EventDispatchers
{
    /// <summary>
    /// EventDispatcher luokka joka lähettää hiiren eventtejä.
    /// </summary>
    public class MouseEventDispatcher : EventDispatcher
    {
        #region Vars
        private bool isMouseOver;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun vasenta hiiren nappia painetaan kerran.
        /// </summary>
        public event GuiEventHandler OnLeftClick;

        /// <summary>
        /// Laukaistaan kun oikee hiiren nappia painetaan kerran.
        /// </summary>
        public event GuiEventHandler OnRightClick;

        /// <summary>
        /// Laukaistaan kun vasenta hiiren nappia pidetään pohjassa.
        /// </summary>
        public event GuiEventHandler OnLeftButtonDown;

        /// <summary>
        /// Laukaistaan kun oikeaa hiiren nappia pidetään pohjassa.
        /// </summary>
        public event GuiEventHandler OnRightButtonDown;
        
        /// <summary>
        /// Laukaistaan kun hiiri tulee kontrollin sisälle.
        /// </summary>
        public event GuiEventHandler OnEnter;

        /// <summary>
        /// Laukaistaan kun hiiri poistuu kontrollin sisältä.
        /// </summary>
        public event GuiEventHandler OnLeave;

        /// <summary>
        /// Laukaistaan kun hiiri on kontrollin päällä.
        /// </summary>
        public event GuiEventHandler OnHover;
        #endregion

        #region Properties
        public bool IsLeftButtonDown
        {
            get;
            private set;
        }
        public bool IsRightButtonDown
        {
            get;
            private set;
        }
        public bool WasLeftClicked
        {
            get;
            private set;
        }
        public bool WasRightClicked
        {
            get;
            private set;
        }
        public bool IsHovering
        {
            get
            {
                return isMouseOver;
            }
        }
        #endregion

        public MouseEventDispatcher(Control sender)
            : base(sender)
        {
        }
        private bool IsMouseOver()
        {
            Point mousePosition = Khv.Engine.InputManager.MousePosition;
            Rectangle controlRect = new Rectangle((int)sender.Position.Real.X, (int)sender.Position.Real.Y, sender.Size.Width, sender.Size.Height);

            return mousePosition.X <= controlRect.Right && mousePosition.X >= controlRect.Left &&
                   mousePosition.Y >= controlRect.Top && mousePosition.Y <= controlRect.Bottom;
        }
        public override void ListenOnce()
        {
            if (IsListening)
            {
                if (IsMouseOver())
                {
                    #region MouseOver and hover logic
                    // Mouse enter logiikka.
                    if (!isMouseOver)
                    {
                        if (OnEnter != null)
                        {
                            OnEnter(sender, new GameEventArgs());
                            isMouseOver = true;
                            return;
                        }
                    }

                    // Mouse hover logiikka.
                    if (isMouseOver)
                    {
                        if (OnHover != null)
                        {
                            OnHover(sender, new GameEventArgs());
                        }
                    }
                    #endregion

                    #region Click logic
                    // Oikea klikkaus.
                    if (InputManager.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                        InputManager.LastMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (OnLeftClick != null)
                        {
                            OnLeftClick(sender, new GameEventArgs());
                        }
                        WasLeftClicked = true;
                    }
                    else
                    {
                        WasLeftClicked = false;
                    }
                    
                    // Vasen klikkaus.
                    if (InputManager.CurrentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                       InputManager.LastMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (OnRightClick != null)
                        {
                            OnRightClick(sender, new GameEventArgs());
                        }
                        WasRightClicked = true;
                    }
                    else
                    {
                        WasRightClicked = false;
                    }
                    #endregion

                    #region Button down logic
                    // Vasen down.
                    if (InputManager.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (OnLeftButtonDown != null)
                        {
                            OnLeftButtonDown(sender, new GameEventArgs());
                        }
                        IsLeftButtonDown = true;
                    }
                    else
                    {
                        IsLeftButtonDown = false;
                    }

                    // Oikea down.
                    if (InputManager.CurrentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (OnRightButtonDown != null)
                        {
                            OnRightButtonDown(sender, new GameEventArgs());
                        }
                        IsRightButtonDown = true;
                    }
                    else
                    {
                        IsRightButtonDown = false;
                    }
                    #endregion
                }
                else if (isMouseOver)
                {
                    #region Leave logic
                    if (OnLeave != null)
                    {
                        OnLeave(sender, new GameEventArgs());
                    }
                    isMouseOver = false;
                    #endregion
                }
            }
        }
    }
}
