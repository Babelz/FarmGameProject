using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using Farmi.XmlParsers;

namespace Farmi.Datasets
{
    internal sealed class BuildingDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        /// <summary>
        /// Rakennuksen nimi.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }
        /// <summary>
        /// Rakennuksen koko tai 
        /// määrä jota käytetään offsettauksessa.
        /// </summary>
        public Size Size
        {
            get;
            private set;
        }
        /// <summary>
        /// Tekstuurin nimi jota rakennus käyttää.
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }
        /// <summary>
        /// Rakennuksen colliderin positionin offset.
        /// </summary>
        public Vector2 ColliderPositionOffSet
        {
            get;
            private set;
        }
        /// <summary>
        /// Rakennuksen colliderin sizen offsetti.
        /// </summary>
        public Size ColliderSizeOffSet
        {
            get;
            private set;
        }
        /// <summary>
        /// Scriptien nimet, joita rakennus voi käyttää.
        /// </summary>
        public string[] Scripts
        {
            get;
            private set;
        }
        /// <summary>
        /// Lista ovista jotka rakennus omistaa.
        /// </summary>
        public DoorDataset[] Doors
        {
            get;
            private set;
        }
        #endregion

        private void GetDoorValues(XElement xElement)
        {
            IEnumerable<XElement> doorElements = xElement.Descendants("Doors");

            if (doorElements != null)
            {
                Doors = (from doors in doorElements
                         from door in doors.Descendants()
                         where door.Name == "Door"
                         select new DoorDataset(door)).ToArray();
            }
        }
        private void GetScriptValues(XElement xElement)
        {
            IEnumerable<XElement> scriptElements = xElement.Descendants("Scripts");
 
            if (scriptElements != null)
            {
                Scripts = (from scriptNames in scriptElements
                           from scriptName in scriptNames.Descendants()
                           where scriptName.Name == "Script"
                           select scriptName.Attribute("Name").Value).ToArray<string>();
            }
        }
        private void GetColliderValues(XElement xElement)
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
            AssetName = reader.ReadAttribute("AssetName", AtributeValueType.String);

            Size = reader.ReadSize();
        }

        /// <summary>
        /// Parsii XElementistä tiedot oliolle.
        /// </summary>
        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            GetBasicValues(xElement);
            GetColliderValues(xElement);
            GetScriptValues(xElement);
            GetDoorValues(xElement);
        }
        /// <summary>
        /// Palauttaa olion XElementtinä.
        /// </summary>
        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
