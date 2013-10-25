using Khv.Engine.Args;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Components.EventDispatchers
{
    /// <summary>
    /// Eventti dispatcherien perusluokka.
    /// </summary>
    public abstract class EventDispatcher
    {
        #region Vars
        private bool isListening;
        protected readonly Control sender;
        #endregion

        #region Properties
        public bool IsListening
        {
            get
            {
                return isListening;
            }
            set
            {
                isListening = value;
            }
        }
        #endregion

        public EventDispatcher(Control sender)
        {
            this.sender = sender;
            isListening = true;
        }

        /// <summary>
        /// Kuuntelee eventtejä kerran.
        /// </summary>
        public abstract void ListenOnce();
    }
    public delegate void GuiEventHandler(object sender, GameEventArgs e);
}
