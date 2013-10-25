using System;
using System.Xml.Serialization;

namespace Khv.Maps.SerializedDataTypes.Layers.SerializedComponents
{
    [XmlType(TypeName = "Mdi data")]
    [Serializable]
    public class SerializedMdiData
    {
        public int PositionIndexX { get; set; }
        public int PositionIndexY { get; set; }
    }
}
