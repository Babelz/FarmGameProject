using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;

namespace Farmi.Datasets
{
    internal struct DoorDataset
    {
        #region Vars
        /// <summary>
        /// Tekstuurin nimi jota ovi käyttää.
        /// </summary>
        public string AssetName
        {
            get;
            set;
        }
        /// <summary>
        /// Minne teleporttaa.
        /// </summary>
        public string TeleportTo
        {
            get;
            set;
        }
        /// <summary>
        /// Oven sijainti.
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }
        /// <summary>
        /// Oven koko.
        /// </summary>
        public Size Size
        {
            get;
            set;
        }
        #endregion
    }
}
