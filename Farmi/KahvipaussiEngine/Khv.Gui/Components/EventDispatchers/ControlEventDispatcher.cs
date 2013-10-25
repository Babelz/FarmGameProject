using Khv.Engine.Args;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Components.EventDispatchers
{
    /// <summary>
    /// EventDispatcher luokka joka lähettää kaikille kontrolleille
    /// ominaisia eventtejä.
    /// </summary>
    public class ControlEventDispatcher : EventDispatcher
    {
        #region Vars
        private bool oldEnabled;
        private bool oldFocus;
        private bool oldVisibility;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun enabled value on muuttunut.
        /// </summary>
        public event GuiEventHandler OnEnabledChanged;

        /// <summary>
        /// Laukaistaan kun visible value on muuttunut.
        /// </summary>
        public event GuiEventHandler OnVisibilityChanged;

        /// <summary>
        /// Laukaistaan kun focused value on muuttunut.
        /// </summary>
        public event GuiEventHandler OnFocusChanged;
        #endregion

        public ControlEventDispatcher(Control sender)
            : base(sender)
        {
        }
        public override void ListenOnce()
        {
            if (IsListening)
            {
                if (sender.Enabled != oldEnabled && OnEnabledChanged != null)
                {
                    OnEnabledChanged(sender, new GameEventArgs());
                }
                if (sender.HasFocus != oldFocus && OnFocusChanged != null)
                {
                    OnFocusChanged(sender, new GameEventArgs());
                }
                if (sender.Visible != oldVisibility && OnVisibilityChanged != null)
                {
                    OnVisibilityChanged(sender, new GameEventArgs());
                }

                oldEnabled = sender.Enabled;
                oldFocus = sender.HasFocus;
                oldVisibility = sender.Visible;
            }
        }
    }
}
