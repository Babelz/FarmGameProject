using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Farmi.XmlParsers;

namespace Farmi.Datasets
{
    public sealed class FeedDataset : IDataset
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
        public string Type
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
        public string AssetName
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
            Type = reader.ReadAttribute("Type", AttributeValueType.String);
            Description = reader.ReadAttribute("Description", AttributeValueType.String);

            AssetName = reader.ReadAttribute("AssetName", AttributeValueType.String);
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
