using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Khv.Maps.SerializedDataTypes.Layers;

namespace Khv.Maps.SerializedDataTypes
{
    [XmlRoot("KebabMap")]
    [Serializable]
    public class SerializedMap
    {
        public string Name { get; set; }
        public SerializedTileEngine TileEngine { get; set; }
        [XmlArray("MapAtributes")]
        [XmlArrayItem("Atribute")]
        public List<string> Attributes { get; set; }
        [XmlArray("TileLayers")]
        public List<SerializedTileLayer> TileLayers { get; set; }
        [XmlArray("AnimationLayers")]
        public List<SerializedAnimationLayer> AnimationLayers { get; set; }
        [XmlArray("ObjectLayers")]
        public List<SerializedObjectLayer> ObjectLayers { get; set; }
        [XmlArray("RuleLayers")]
        public List<SerializedRuleLayer> RuleLayers { get; set; }
    }
}
