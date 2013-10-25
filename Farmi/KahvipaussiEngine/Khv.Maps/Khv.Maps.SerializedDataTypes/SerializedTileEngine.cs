using System;
using System.Xml.Serialization;

namespace Khv.Maps.SerializedDataTypes
{
    [XmlType(TypeName = "Tile engine")]
    [Serializable]
    public class SerializedTileEngine
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
    }
}
