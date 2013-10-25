using Khv.Gui.Components.EventDispatchers;

namespace Khv.Gui.Components.EventListeners
{
    /// <summary>
    /// Rajapinta joka mahdollistaa jokaiselle kontrollille ominaisien eventtien kuuntelun.
    /// </summary>
    public interface IControlListener
    {
        #region Properties
        ControlEventDispatcher ControlEventListener
        {
            get;
        }
        #endregion
    }
}
