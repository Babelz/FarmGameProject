using Khv.Gui.Components.EventDispatchers;

namespace Khv.Gui.Components.EventListeners
{
    /// <summary>
    /// Interface joka mahdollistaa kontrollin kuunnella hiiri eventtejä.
    /// </summary>
    public interface IMouseListener
    {
        #region Properties
        MouseEventDispatcher MouseEventListener
        {
            get;
        }
        #endregion
    }
}
