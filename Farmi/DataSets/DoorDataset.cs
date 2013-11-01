using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using Farmi.XmlParsers;

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
        /// <summary>
        /// Oven teleportin tiedot.
        /// </summary>
        public TeleportDataset TeleportDataset
        {
            get;
            private set;
        }
        #endregion

        public DoorDataset(XElement xElement)
        {
            ParseValuesFrom(xElement);
        }

        private void GetTeleportValues(XElement xElement)
        {
            XElement teleportElement = xElement.Element("Teleport");

            if (teleportElement != null)
            {
                TeleportDataset = new TeleportDataset();
                TeleportDataset.ParseValuesFrom(teleportElement);
            }
        }
        private void GetBasicValues(XElement xElement)
        {
            XAtributeReader reader = new XAtributeReader(xElement);

            AssetName = reader.ReadAttribute("AssetName", AtributeValueType.String);
            Position = reader.ReadVector();
            Size = reader.ReadSize();
        }

        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            GetBasicValues(xElement);
            GetTeleportValues(xElement);
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
