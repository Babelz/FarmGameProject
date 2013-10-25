using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Gui.Components.BaseComponents;

namespace Khv.Gui.Components
{
    /// <summary>
    /// Luokka joka wräppää horizontal ja vertical alingment valuet.
    /// </summary>
    public class Alingment
    {
        #region Properties
        public HorizontalAlingment Horizontal
        {
            get;
            set;
        }
        public VerticalAlingment Vertical
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Laskee kohdan mihin kontrolli pitäisi laittaa
        /// nykyisillä aling arvoilla.
        /// </summary>
        public Point Calculate(Control owner)
        {
            Point position = owner.Position.Relative;

            if (owner.Parent != null)
            {
                if (Horizontal != HorizontalAlingment.None)
                {
                    position.X = GuiHelper.CalulateHorizontalPoint(owner.Position, owner.Size, owner.Parent.Size, owner.Parent.Position, Horizontal).X;
                }
                if (Vertical != VerticalAlingment.None)
                {
                    position.Y = GuiHelper.CalculateVerticalPoint(owner.Position, owner.Size, owner.Parent.Size, owner.Parent.Position, Vertical).Y;
                }
            }

            return position;
        }
    }
}
