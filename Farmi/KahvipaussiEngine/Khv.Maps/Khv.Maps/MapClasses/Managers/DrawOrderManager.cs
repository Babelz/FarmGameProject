using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Components;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// luokka joka pitää huolen layereiden drawordereista ja
    /// managoi niitä, kaikki niihin liittyvät metodit tulisi
    /// kutsua tämän kautta
    /// </summary>
    public class DrawOrderManager : Manager
    {
        #region Vars
        private List<DrawOrder> drawOrders;
        #endregion

        public DrawOrderManager()
            : base()
        {
            drawOrders = new List<DrawOrder>();
        }

        public void AddOrder(DrawOrder drawOrder)
        {
            drawOrders.Add(drawOrder);
        }
        /// <summary>
        /// kutsutaan kun halutaan muuttaa draworderia
        /// </summary>
        /// <param name="layerName">layerin nimi</param>
        /// <param name="newValue">uusi value</param>
        public void ChangeOrder(string layerName, int newValue)
        {
            for (int i = 0; i < drawOrders.Count; i++)
            {
                if (drawOrders[i].Value == newValue)
                {
                    drawOrders[i].Value = drawOrders.Find(o => o.LayerName == layerName).Value;
                    break;
                }
            }
            drawOrders.Find(o => o.LayerName == layerName).Value = newValue;
        }
    }
}
