using System;
using Microsoft.Xna.Framework.Content;

namespace Khv.Gui.Components.BaseComponents
{
    /// <summary>
    /// Rajapinta joka peritään uutta tyyliä luodessa.
    /// </summary>
    public abstract class Style<T> where T : Control
    {
        #region Properties
        public Action<T> Apply
        {
            get;
            protected set;
        }
        public string StyleName
        {
            get;
            protected set;
        }
        #endregion

        public Style(ContentManager contentManager)
        {
        }
    }
}
