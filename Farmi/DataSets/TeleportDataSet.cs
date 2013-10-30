using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;
using System.Xml.Linq;

namespace Farmi.Datasets
{
    internal sealed class TeleportDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        public string TeleportTo
        {
            get;
            private set;
        }
        public Vector2 PositionOffSet
        {
            get;
            private set;
        }
        public Size Size
        {
            get;
            private set;
        }
        #endregion

        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            TeleportTo = xElement.Attribute("TeleportTo").Value;

            PositionOffSet = new Vector2(float.Parse(xElement.Attribute("X").Value),
                                         float.Parse(xElement.Attribute("Y").Value));

            Size = new Size(int.Parse(xElement.Attribute("Width").Value),
                            int.Parse(xElement.Attribute("Height").Value));
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
