using Khv.Gui.Components.BaseComponents.Containers.Components;

namespace Khv.Gui.Components.BaseComponents.Containers.Components.Layout
{
    public abstract class LayoutManager
    {
        #region Vars
        protected Control container;
        #endregion

        #region Properties
        public Control Container
        {
            set
            {
                container = value;
            }
        }
        #endregion

        public abstract void Add(ControlManager controlManager, Control controlToAdd, ILayoutConstraints constraints);
        public abstract void Validate();
    }
}
