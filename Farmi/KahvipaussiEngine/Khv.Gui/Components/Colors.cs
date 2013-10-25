using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components
{
    /// <summary>
    /// Luokka joka wräppää kontrollin kaikki colorit itseensä.
    /// </summary>
    public class Colors
    {
        #region Properites
        public Color Foreground
        {
            get;
            set;
        }
        public Color Background
        {
            get;
            set;
        }
        #endregion
    }
}
