using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Gui.Components.BaseComponents
{
    public class Padding
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

        public Padding(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public Padding(int top, int side)
        {
            Top = top;
            Bottom = top;

            Left = side;
            Right = side;
        }
        public Padding(int value)
        {
            Top = value;
            Bottom = value;
            Left = value;
            Right = value;
        }

        /// <summary>
        /// Palauttaa paddinging jonka kaikki valuet ovat nollia.
        /// </summary>
        public static Padding Empty()
        {
            return new Padding(0);
        }
    }
}
