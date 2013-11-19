using Microsoft.Xna.Framework.Content;
using Khv.Engine;

namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    public partial class Window 
    {
        #region Vars
        protected readonly KhvGame khvGame;
        #endregion

        public Window(KhvGame khvGame)
            : base()
        {
            this.khvGame = khvGame;
        }

        // Tehdään eventti ja metodi implementaatiot tässä osassa luokkaa
    }
}
