using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapClasses.Layers.Components
{
    public class DrawOrder
    {
        #region Properties
        public int Value
        {
            get;
            set;
        }
        public string LayerName
        {
            get;
            set;
        }
        #endregion

        public DrawOrder(string layerName)
        {
            LayerName = layerName;
        }
    }
}
