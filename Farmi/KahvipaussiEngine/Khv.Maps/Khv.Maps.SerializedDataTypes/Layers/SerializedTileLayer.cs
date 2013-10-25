using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Khv.Maps.SerializedDataTypes.Tiles;

namespace Khv.Maps.SerializedDataTypes.Layers
{
    [XmlRoot("Tile layer")]
    [Serializable]
    public class SerializedTileLayer : BaseSerializedLayer
    {
        [XmlArray("Tiles")]
        public List<SerializedBaseTile> Tiles { get; set; }
    }
}
