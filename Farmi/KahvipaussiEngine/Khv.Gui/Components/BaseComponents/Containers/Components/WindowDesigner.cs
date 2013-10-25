using Khv.Gui.Components.BaseComponents.Containers.Components;

namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    // INFO: Ainakun luodaan uusi ikkuna tulee se periä tästä luokasta!
    public partial class Window : Container
    {
        #region Properties
        public ControlManager ControlManager
        {
            get
            {
                return controlManager;
            }
        }
        #endregion

        protected virtual void Initialize()
        {
            // INFO: Malli tyyli.

            #region Window init
            // alustetaan ikkuna
            #endregion

            #region Control init
            // alustetaan kaikki kontrollit
            #endregion

            #region Component init
            // alustetaan kaikki komponentit
            #endregion
        }
    }
}
