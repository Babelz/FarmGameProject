using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerializedDataTypes.Components;

namespace SerializedDataTypes.MapObjects
{
    [Serializable]
    public class SerializedMapObject : IObjectData
    {
        [XmlArray("Valuepairs")]
        [XmlArrayItem("Pair")]
        public List<Valuepair> valuepairs = new List<Valuepair>(5);

        public void SetValues(string name, List<Valuepair> valuepairs)
        {
            Name = name;
            if (valuepairs.Count < this.valuepairs.Capacity)
            {
                this.valuepairs = valuepairs;
            }
            else
            {
                throw new Exception("Too many valuepairs!");
            }
        }
    }
}
