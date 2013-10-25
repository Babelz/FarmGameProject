using Khv.Engine;
using Khv.Engine.Args;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components.Navigation;

namespace Khv.Gui.Components.EventDispatchers
{
    /// <summary>
    /// EventDispatcher luokka joka lähettää buttonina
    /// toimivalle kontrollille ominaisia eventtejä.
    /// </summary>
    public class ButtonEventDispatcher : EventDispatcher
    {
        #region Vars
        private KeyTrigger keyTrigger;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun nappia on painettu.
        /// </summary>
        public event GuiEventHandler OnKeyPressed;

        /// <summary>
        /// Laukaistaan kun nappi on relasettu.
        /// </summary>
        public event GuiEventHandler OnKeyReleased;
        #endregion

        #region Properties
        /// <summary>
        /// Eventin trigger key joka laukaisee itse eventin.
        /// </summary>
        public KeyTrigger Trigger
        {
            get
            {
                return keyTrigger;
            }
        }
        public bool IsKeyPressed
        {
            get
            {
                return keyTrigger.CurrentState == PressedState.Pressed;
            }
        }
        public bool IsKeyReleased
        {
            get
            {
                return keyTrigger.CurrentState == PressedState.Released;
            }
        }
        #endregion


        public ButtonEventDispatcher(Control sender)
            : base(sender)
        {
            keyTrigger = new KeyTrigger(Microsoft.Xna.Framework.Input.Keys.Enter);
        }
        public override void ListenOnce()
        {
            if (IsListening)
            {
                if (InputManager.Current == Trigger.Key)
                {
                    if (keyTrigger.CurrentState == PressedState.None)
                    {
                        if (OnKeyPressed != null)
                        {
                            OnKeyPressed(sender, new GameEventArgs());
                        }
                        keyTrigger.CurrentState = PressedState.Pressed;
                    }
                }
                else if (keyTrigger.CurrentState == PressedState.Pressed)
                {
                    if (OnKeyReleased != null)
                    {
                        OnKeyReleased(sender, new GameEventArgs());
                    }
                    keyTrigger.CurrentState = PressedState.Released;
                }
                else
                {
                    keyTrigger.CurrentState = PressedState.None;
                }
            }
        }
    }
}
