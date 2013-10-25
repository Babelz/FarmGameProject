using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using SerializedDataTypes.Components;

namespace SerializedDataTypes.RuleObjects
{
    [Serializable]
    public class RuleData : IObjectData
    {
        [XmlIgnore]
        [NonSerialized]
        private Texture2D texture;

        public string TextureName
        {
            get;
            set;
        }
        public string Desc
        {
            get;
            set;
        }
        [XmlIgnore]
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                if (texture == null)
                {
                    texture = value;
                }
            }
        }
    }
}
