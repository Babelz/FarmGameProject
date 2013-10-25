using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Khv.Maps.SerializedDataTypes.Tiles;

namespace Khv.Maps.SerializedDataTypes.Layers
{
    [XmlRoot("Rule layer")]
    [Serializable]
    public class SerializedRuleLayer : BaseSerializedLayer
    {
        [XmlArray("Rule tiles")]
        public List<SerializedRuleTile> Tiles { get; set; }
    }
}
