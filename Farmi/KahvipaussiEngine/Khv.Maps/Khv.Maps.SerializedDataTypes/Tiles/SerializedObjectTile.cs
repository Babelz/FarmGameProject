using System;
using System.Xml.Serialization;
using SerializedDataTypes.MapObjects;

namespace Khv.Maps.SerializedDataTypes.Tiles
{
    [XmlRoot("Object tile")]
    [Serializable]
    public class SerializedObjectTile : BaseSerializedTile
    {
        public SerializedMapObject MapObject { get; set; }
    }
}
