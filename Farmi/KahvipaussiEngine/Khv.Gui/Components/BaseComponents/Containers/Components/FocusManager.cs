using Khv.Gui.Components;
using Khv.Engine.Args;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    /// <summary>
    /// Luokka jolla managoidaan kontrollien focusta.
    /// </summary>
    public class FocusManager
    {
        #region Vars
        private Control currentFocused;
        #endregion

        #region Events
        public event FocusedControlChanged OnFocusedControlChanged;
        #endregion

        #region Properties
        public Control CurrentFocused
        {
            get
            {
                return currentFocused;
            }
        }
        #endregion

        /// <summary>
        /// Antaa halutulle kontrollille focuksen ja laukaisee eventin. 
        /// FocusedControlChanged jos se on implementoitu.
        /// </summary>
        /// <param name="control"></param>
        public void ChangeFocus(Control control)
        {
            if (currentFocused != null)
            {
                if (OnFocusedControlChanged != null)
                {
                    OnFocusedControlChanged(currentFocused, new FocusEventArgs(control));
                }
                currentFocused.Defocus();
            }
            currentFocused = control;
            currentFocused.Focus();
        }
    }
    public class FocusEventArgs : GameEventArgs
    {
        #region Properites
        public Control NextInFocus
        {
            get;
            private set;
        }
        #endregion

        public FocusEventArgs(Control nextInFocus)
        {
            NextInFocus = nextInFocus;
        }
    }

    public delegate void FocusedControlChanged(object sender, FocusEventArgs e);
}
