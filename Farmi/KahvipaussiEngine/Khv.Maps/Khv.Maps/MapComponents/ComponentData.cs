using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapComponents.Components
{
    public class MapComponentData 
    {
        #region Properties
        public string ComponentName
        {
            get;
            private set;
        }
        public string LayerName
        {
            get;
            private set;
        }
        public string[] Atributes
        {
            get;
            private set;
        }
        #endregion

        public MapComponentData(string componentName, string layerName, string[] atributes)
        {
            ComponentName = componentName;
            LayerName = layerName;
            Atributes = atributes;
        }
        public override string ToString()
        {
            string str = base.ToString();
            str += LayerName + Environment.NewLine;
            return str;
        }
    }
}
