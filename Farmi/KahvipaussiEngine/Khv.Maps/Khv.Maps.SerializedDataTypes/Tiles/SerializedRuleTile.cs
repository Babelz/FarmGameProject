using System;
using System.Xml.Serialization;

namespace Khv.Maps.SerializedDataTypes.Tiles
{
    [XmlRoot("Rule tile")]
    [Serializable]
    public class SerializedRuleTile : BaseSerializedTile
    {
        public string RuleName { get; set; }
    }
}
