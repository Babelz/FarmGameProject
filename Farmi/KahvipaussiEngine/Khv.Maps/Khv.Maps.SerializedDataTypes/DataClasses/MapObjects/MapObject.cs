using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerializedDataTypes.Components;

namespace SerializedDataTypes.MapObjects
{
    [Serializable]
    public class MapObject : IObjectData
    {
        [XmlArray("Valuepairs")]
        [XmlArrayItem("Pair")]
        public List<Valuepair> valuepairs = new List<Valuepair>();

        public void SetValues(string name, List<Valuepair> valuepairs)
        {
            Name = name;
            if (valuepairs.Count < this.valuepairs.Capacity)
            {
                this.valuepairs = valuepairs;
            }
        }
    }
}
