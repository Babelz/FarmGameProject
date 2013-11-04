using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using System.Xml.Linq;
using Farmi.XmlParsers;

namespace Farmi.Datasets
{
    public sealed class ConsumableItemDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public string AssetName
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
        /// <summary>
        /// Value joka lisätään playerin max staminaa.
        /// </summary>
        public int AddedStamina
        {
            get;
            private set;
        }
        /// <summary>
        /// Stamina jonka item palauttaa.
        /// </summary>
        public int RecoveredStamina
        {
            get;
            private set;
        }
        /// <summary>
        /// Paljonko rahaa itemistä saa.
        /// </summary>
        public int Value
        {
            get;
            private set;
        }
        public string Script
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

            XAttributeReader reader = new XAttributeReader(xElement);

            Name = reader.ReadAttribute("Name", AttributeValueType.String);
            AssetName = reader.ReadAttribute("AssetName", AttributeValueType.String);
            Description = reader.ReadAttribute("Description", AttributeValueType.String);

            AddedStamina = int.Parse(reader.ReadAttribute("MaxStamina", AttributeValueType.Number));
            RecoveredStamina = int.Parse(reader.ReadAttribute("Stamina", AttributeValueType.Number));

            Script = reader.ReadAttribute("Script", AttributeValueType.String);

            Value = int.Parse(reader.ReadAttribute("Value", AttributeValueType.Number));

            Size = new Size(int.Parse(reader.ReadAttribute("Width", AttributeValueType.Number)),
                            int.Parse(reader.ReadAttribute("Height", AttributeValueType.Number)));
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
