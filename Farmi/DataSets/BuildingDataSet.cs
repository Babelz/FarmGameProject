using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;

namespace Farmi.Datasets
{
    internal struct BuildingDataset
    {
        #region Properties
        /// <summary>
        /// Rakennuksen nimi.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Rakennuksen koko tai 
        /// määrä jota käytetään offsettauksessa.
        /// </summary>
        public Size Size
        {
            get;
            set;
        }
        /// <summary>
        /// Tekstuurin nimi jota rakennus käyttää.
        /// </summary>
        public string AssetName
        {
            get;
            set;
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
            set;
        }
        /// <summary>
        /// Scriptien nimet, joita rakennus voi käyttää.
        /// </summary>
        public string[] Scripts
        {
            get;
            set;
        }
        /// <summary>
        /// Lista ovista jotka rakennus omistaa.
        /// </summary>
        public DoorDataset[] Doors
        {
            get;
            set;
        }
        #endregion
    }
}
