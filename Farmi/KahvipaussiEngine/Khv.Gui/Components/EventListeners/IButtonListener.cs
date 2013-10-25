using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components.EventDispatchers;

namespace Khv.Gui.Components.EventListeners
{
    /// <summary>
    /// Rajapinta joka mahdollistaa button tyyppisen kontrollin eventtien kuuntelun.
    /// </summary>
    public interface IButtonListener
    {
        #region Properties
        ButtonEventDispatcher ButtonEventListener
        {
            get;
        }
        #endregion
    }
}
