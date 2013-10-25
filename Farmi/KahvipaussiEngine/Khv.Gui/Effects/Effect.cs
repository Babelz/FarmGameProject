using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components.BaseComponents;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Effects
{
    /// <summary>
    /// Pohjaluokka efektille jota voi päivittää.
    /// Tämä luokka on abstrakti.
    /// </summary>
    public abstract class Effect
    {
        #region Vars
        protected readonly Control owner;
        #endregion

        public Effect(Control owner)
        {
            this.owner = owner;
        }

        public abstract void Update(GameTime gameTime);
    }
}
