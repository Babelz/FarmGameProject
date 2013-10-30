using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace Farmi.Datasets
{
    internal sealed class DoorDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        /// <summary>
        /// Tekstuurin nimi jota ovi käyttää.
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }
        /// <summary>
        /// Minne teleporttaa.
        /// </summary>
        public string TeleportTo
        {
            get;
            private set;
        }
        /// <summary>
        /// Oven sijainti.
        /// </summary>
        public Vector2 Position
        {
            get;
            private set;
        }
        /// <summary>
        /// Oven koko.
        /// </summary>
        public Size Size
        {
            get;
            private set;
        }
        #endregion

        public DoorDataset(XElement xElement)
        {
            ParseValuesFrom(xElement);
        }
        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            GetBasicValues(xElement);
            GetPositionValues(xElement);
            GetSizeValues(xElement);
        }
        public XElement AsXElement()
        {
            return xElement;
        }

        private void GetBasicValues(XElement xElement)
        {
            AssetName = xElement.Attribute("AssetName").Value;
            TeleportTo = xElement.Attribute("TeleportTo").Value;
        }
        private void GetPositionValues(XElement xElement)
        {
            Position = new Vector2(float.Parse(xElement.Attribute("X").Value),
                                   float.Parse(xElement.Attribute("Y").Value));
        }
        private void GetSizeValues(XElement xElement)
        {
            Size = new Size(int.Parse(xElement.Attribute("Width").Value),
                            int.Parse(xElement.Attribute("Height").Value));
        }
    }
}
