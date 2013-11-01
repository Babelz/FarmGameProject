using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Farmi.XmlParsers;

namespace Farmi.Datasets
{
    public sealed class AnimalDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public string Type
        {
            get;
            private set;
        }
        public Size Size
        {
            get;
            private set;
        }
        public string AssetName
        {
            get;
            private set;
        }
        public Vector2 ColliderPositionOffSet
        {
            get;
            private set;
        }
        public Size ColliderSizeOffSet
        {
            get;
            private set;
        }
        public string[] Behaviours
        {
            get;
            private set;
        }
        public DateDataset BirthDay
        {
            get;
            private set;
        }
        public string[] LootTable
        {
            get;
            private set;
        }
        public string[] Tags
        {
            get;
            private set;
        }
        #endregion

        private void ReadBirthDay(XElement xElement)
        {
        }
        private void ReadTags(XElement xElement)
        {
            IEnumerable<XElement> tagElements = xElement.Descendants("Tag");

            if (tagElements != null)
            {
                Tags = (from tags in tagElements
                        from tag in tags.Descendants()
                        where tag.Name == "Tag"
                        select tag.Attribute("Value").Value).ToArray<string>();
            }
        }
        private void ReadLootTable(XElement xElement)
        {
            IEnumerable<XElement> lootTableElements = xElement.Descendants("LootTable");

            if (lootTableElements != null)
            {
                LootTable = (from items in lootTableElements
                             from item in items.Descendants()
                             where item.Name == "Item"
                             select item.Attribute("Name").Value).ToArray<string>();
            }
        }
        private void GetBehaviours(XElement xElement)
        {
            IEnumerable<XElement> behaviourElements = xElement.Descendants("Behaviours");

            if (behaviourElements != null)
            {
                Behaviours = (from behaviours in behaviourElements
                              from behaviour in behaviours.Descendants()
                              where behaviour.Name == "Behaviour"
                              select behaviour.Attribute("Name").Value).ToArray<string>();
            }
        }
        private void ReadColliderValues(XElement xElement)
        {
            XElement colliderElement = xElement.Element("Collider");

            if (colliderElement != null)
            {
                XAtributeReader reader = new XAtributeReader(colliderElement);

                ColliderPositionOffSet = reader.ReadVector();
                ColliderSizeOffSet = reader.ReadSize();
            }
        }
        private void GetBasicValues(XElement xElement)
        {
            XAtributeReader reader = new XAtributeReader(xElement);

            Name = reader.ReadAttribute("Name", AtributeValueType.String);
            Type = reader.ReadAttribute("Type", AtributeValueType.String);
            AssetName = reader.ReadAttribute("AssetName", AtributeValueType.String);

            Size = reader.ReadSize();
        }

        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            GetBasicValues(xElement);
            ReadColliderValues(xElement);
            GetBehaviours(xElement);
            ReadLootTable(xElement);
        }
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
