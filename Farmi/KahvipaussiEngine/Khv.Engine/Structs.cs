using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Engine.Structs
{
    // käytössä - maps, gui
    public struct Size
    {
        #region Vars
        private int width;
        private int height;
        #endregion

        #region Properties
        public int Width
        {
            get
            {
                return width;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
        }
        #endregion

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    // käytössä - maps, gui
    public struct Index
    {
        #region Vars
        private int x;
        private int y;
        #endregion

        #region Properties
        public int X
        {
            get
            {
                return x;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
        }
        public static Index Empty
        {
            get
            {
                return new Index(-1, -1);
            }
        }
        #endregion

        public Index(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Index a, Index b)
        {
            if (a.x == b.x && a.y == b.y)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Index a, Index b)
        {
            if (a.x != b.x && a.y != b.y)
            {
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return "X: " + x + "- Y: " + y;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Index)
            {
                return this == (Index)obj;
            }
            else
            {
                return false;
            }
        }
    }
}
