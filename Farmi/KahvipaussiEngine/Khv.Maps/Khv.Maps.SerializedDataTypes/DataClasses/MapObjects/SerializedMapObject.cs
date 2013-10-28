using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerializedDataTypes.Components;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Tiles;

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

        public override string ToString()
        {
            string str = Environment.NewLine + Name + Environment.NewLine;

            valuepairs.ForEach(s =>
                {
                    str += string.Format("Name: {0} - Value: {1}", s.Name, s.Value) + Environment.NewLine;
                });

            return str;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
