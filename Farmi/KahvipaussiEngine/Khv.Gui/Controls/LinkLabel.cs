using Khv.Engine.Structs;
using Khv.Gui.Components.EventDispatchers;
using Khv.Gui.Components.EventListeners;
using Khv.Gui.Components.Navigation;
using Microsoft.Xna.Framework;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Controls
{
    /// <summary>
    /// Teksti kontrolli joka voi sisältää focuksen.
    /// </summary>
    public class LinkLabel : Label, IButtonListener
    {
        #region Properties
        public KeyTrigger Trigger
        {
            get;
            set;
        }
        public bool ListenButtonEvents
        {
            get;
            set;
        }
        #endregion

        #region Event listeners
        public ButtonEventDispatcher ButtonEventListener
        {
            get
            {
                return (ButtonEventDispatcher)eventDispatchers.Find(d => d is ButtonEventDispatcher);
            }
        }
        #endregion

        public LinkLabel()
            : base()
        {
        }
        public LinkLabel(ControlPosition position, string text = "")
            : base(position, text)
        {
        }
        public LinkLabel(ControlPosition position, Index focusIndex, string text = "")
            : base(position, text)
        {
            FocusIndex = focusIndex;
        }
        protected override void BaseInitialize()
        {
            base.BaseInitialize();
            Name = "LinkLabel";
        }
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);
            }
        }
    }
}
