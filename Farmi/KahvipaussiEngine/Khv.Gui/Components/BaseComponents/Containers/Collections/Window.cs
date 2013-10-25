using Microsoft.Xna.Framework.Content;

namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    public partial class Window 
    {
        #region Vars
        protected ContentManager contentManager;
        #endregion

        public Window(ContentManager contentManager) : base()
        {
            this.contentManager = contentManager;
        }

        // Tehdään eventti ja metodi implementaatiot tässä osassa luokkaa,
    }
}
