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

            XAtributeReader reader = new XAtributeReader(xElement);

            Name = reader.ReadAttribute("Name", AtributeValueType.String);
            AssetName = reader.ReadAttribute("AssetName", AtributeValueType.String);
            Description = reader.ReadAttribute("Description", AtributeValueType.String);

            AddedStamina = int.Parse(reader.ReadAttribute("MaxStamina", AtributeValueType.Number));
            RecoveredStamina = int.Parse(reader.ReadAttribute("Stamina", AtributeValueType.Number));

            Script = reader.ReadAttribute("Script", AtributeValueType.String);

            Value = int.Parse(reader.ReadAttribute("Value", AtributeValueType.Number));

            Size = new Size(int.Parse(reader.ReadAttribute("Width", AtributeValueType.Number)),
                            int.Parse(reader.ReadAttribute("Height", AtributeValueType.Number)));
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
