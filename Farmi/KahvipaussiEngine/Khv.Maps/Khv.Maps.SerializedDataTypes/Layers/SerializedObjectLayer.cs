using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Khv.Maps.SerializedDataTypes.Tiles;

namespace Khv.Maps.SerializedDataTypes.Layers
{
    [XmlRoot("Object layer")]
    [Serializable]
    public class SerializedObjectLayer : BaseSerializedLayer
    {
        [XmlArray("Object tiles")]
        public List<SerializedObjectTile> Tiles { get; set; }
    }
}
