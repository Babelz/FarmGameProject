using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;

namespace Farmi.XmlParsers
{
    internal enum AttributeValueType
    {
        Number,
        String
    }

    internal sealed class XAttributeReader
    {
        #region Properties
        public XElement CurrentElement
        {
            get;
            set;
        }
        #endregion

        public XAttributeReader(XElement startElement)
        {
            CurrentElement = startElement;
        }

        public string ReadAttribute(string attributeName, AttributeValueType valueType)
        {
            XAttribute attribute = CurrentElement.Attribute(attributeName);

            if (attribute == null)
            {
                switch (valueType)
                {
                    case AttributeValueType.Number:
                        return "0";
                    case AttributeValueType.String:
                        return string.Empty;
                    default:
                        throw new NotImplementedException("Unkown value type.");
                }
            }

            return attribute.Value;
        }
        public Size ReadSize()
        {
            return new Size(int.Parse(ReadAttribute("Width", AttributeValueType.Number)),
                            int.Parse(ReadAttribute("Height", AttributeValueType.Number)));
        }
        public Vector2 ReadVector()
        {
            return new Vector2(float.Parse(ReadAttribute("X", AttributeValueType.Number)),
                               float.Parse(ReadAttribute("Y", AttributeValueType.Number)));
        }
    }
}
