using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Farmi.XmlParsers;

namespace Farmi.Datasets
{
    public class ToolDataset : IDataset
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
        public int MinPow
        {
            get;
            private set;
        }
        public int MaxPow
        {
            get;
            private set;
        }
        public int PowTimestep
        {
            get;
            private set;
        }
        public string Behaviour
        {
            get;
            private set;
        }
        #endregion

        public virtual void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            XAttributeReader reader = new XAttributeReader(xElement);

            Name = reader.ReadAttribute("Name", AttributeValueType.String);
            AssetName = reader.ReadAttribute("AssetName", AttributeValueType.String);
            Description = reader.ReadAttribute("Description", AttributeValueType.String);

            MinPow = int.Parse(reader.ReadAttribute("MinPow", AttributeValueType.Number));
            MaxPow = int.Parse(reader.ReadAttribute("MaxPow", AttributeValueType.Number));

            PowTimestep = int.Parse(reader.ReadAttribute("PowTimestep", AttributeValueType.Number));
            
            Behaviour = reader.ReadAttribute("Behaviour", AttributeValueType.String);
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
