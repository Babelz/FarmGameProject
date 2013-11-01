using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;

namespace Farmi.XmlParsers
{
    internal enum AtributeValueType
    {
        Number,
        String
    }

    internal sealed class XAtributeReader
    {
        #region Properties
        public XElement CurrentElement
        {
            get;
            set;
        }
        #endregion

        public XAtributeReader(XElement startElement)
        {
            CurrentElement = startElement;
        }

        public string ReadAttribute(string attributeName, AtributeValueType valueType)
        {
            XAttribute attribute = CurrentElement.Attribute(attributeName);

            if (attribute == null)
            {
                switch (valueType)
                {
                    case AtributeValueType.Number:
                        return "0";
                    case AtributeValueType.String:
                        return string.Empty;
                    default:
                        throw new NotImplementedException("Unkown value type.");
                }
            }

            return attribute.Value;
        }
        public Size ReadSize()
        {
            return new Size(int.Parse(ReadAttribute("Width", AtributeValueType.Number)),
                            int.Parse(ReadAttribute("Height", AtributeValueType.Number)));
        }
        public Vector2 ReadVector()
        {
            return new Vector2(float.Parse(ReadAttribute("X", AtributeValueType.Number)),
                               float.Parse(ReadAttribute("Y", AtributeValueType.Number)));
        }
    }
}
