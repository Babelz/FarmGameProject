using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace Farmi.Datasets
{
    internal sealed class BuildingDataset
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
            set;
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

        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;

            GetBasicValues(xElement);
            GetColliderValues(xElement);
            GetScriptValues(xElement);
            GetDoorValues(xElement);
        }
        public XElement AsXElement()
        {
            return xElement;
        }

        private void GetDoorValues(XElement xElement)
        {
            if (xElement.Descendants("Doors") != null)
            {
                Doors = (from doors in xElement.Descendants("Doors")
                         from door in doors.Descendants() select
                         new DoorDataset()
                         {
                             AssetName = door.Attribute("AssetName").Value,
                             TeleportTo = door.Attribute("TeleportTo").Value,
                             Position = new Vector2(float.Parse(door.Attribute("X").Value),
                                                    float.Parse(door.Attribute("Y").Value)),
                             Size = new Size(int.Parse(door.Attribute("Width").Value),
                                             int.Parse(door.Attribute("Height").Value))
                         }).ToArray<DoorDataset>();
            }
        }
        private void GetScriptValues(XElement xElement)
        {
            if (xElement.Descendants("Scripts") != null)
            {
                Scripts = (from scriptNames in xElement.Descendants("Scripts")
                           from scriptName in scriptNames.Descendants()
                           select scriptName.Attribute("Name").Value).ToArray<string>();
            }
        }
        private void GetColliderValues(XElement xElement)
        {
            if (xElement.Element("Collider") != null)
            {
                var colliderData = xElement.Element("Collider");

                ColliderPositionOffSet = new Vector2(float.Parse(colliderData.Attribute("X").Value),
                                                     float.Parse(colliderData.Attribute("Y").Value));

                ColliderSizeOffSet = new Size(int.Parse(colliderData.Attribute("Width").Value),
                                              int.Parse(colliderData.Attribute("Height").Value));
            }
        }
        private void GetBasicValues(XElement xElement)
        {
            Name = xElement.Attribute("Name").Value;

            Size = new Size(int.Parse(xElement.Attribute("Width").Value),
                            int.Parse(xElement.Attribute("Height").Value));

            AssetName = xElement.Attribute("AssetName").Value;
        }
    }
}
