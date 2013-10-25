using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Effects
{
    /// <summary>
    /// Pohjaluokka efektille joka voi piirtää. 
    /// Tämä luokka on abstrakti.
    /// </summary>
    public abstract class DrawableEffect : Effect
    {
        #region Properties
        public bool IsOverridingDraw
        {
            get;
            protected set;
        }
        public int DrawOrder
        {
            get;
            set;
        }
        #endregion

        public DrawableEffect(Control owner, int drawOrder)
            : base(owner)
        {
            DrawOrder = drawOrder;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
