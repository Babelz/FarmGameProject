using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Gui.Components.BaseComponents.Containers.Components;

namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    /// <summary>
    /// Yksinkertaisin controlcollection luokka.
    /// </summary>
    public class Panel : Container
    {
        #region Properties
        public ControlManager ControlManager
        {
            get
            {
                return controlManager;
            }
        }
        public FocusManager FocusManager
        {
            get
            {
                return focusManager;
            }
        }
        public IndexNavigator IndexNavigator
        {
            get
            {
                return indexNavigator;
            }
        }
        #endregion

        public Panel()
            : base()
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
