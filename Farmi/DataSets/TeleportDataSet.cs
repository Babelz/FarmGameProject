using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;
using System.Xml.Linq;
using Farmi.XmlParsers;

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

            XAtributeReader reader = new XAtributeReader(xElement);

            TeleportTo = reader.ReadAttribute("TeleportTo", AtributeValueType.String);

            PositionOffSet = reader.ReadVector();
            Size = reader.ReadSize();
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
