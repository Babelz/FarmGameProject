using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Gui.Components.BaseComponents
{
    public class Margin
    {
        #region Properties
        public int Left
        {
            get;
            set;
        }
        public int Right
        {
            get;
            set;
        }
        public int Top
        {
            get;
            set;
        }
        public int Bottom
        {
            get;
            set;
        }
        #endregion

        public Margin(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public Margin(int top, int side)
        {
            Top = top;
            Bottom = top;

            Left = side;
            Right = side;
        }
        public Margin(int value)
        {
            Top = value;
            Bottom = value;
            Left = value;
            Right = value;
        }

        /// <summary>
        /// Palauttaa marginin minkä kaikki valuet ovat nollia.
        /// </summary>
        public static Margin Default()
        {
            return new Margin(0);
        }
    }
}
