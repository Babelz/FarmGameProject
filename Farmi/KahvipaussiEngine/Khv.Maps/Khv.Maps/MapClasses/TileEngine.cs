using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses
{
    /// <summary>
    /// Jokaisella kartalla on oma moottorinsa joka sisältää 
    /// tiedot kartan koosta ja tilejen koosta.
    /// </summary>
    public class TileEngine
    {
        #region Vars
        /// <summary>
        /// Palauttaa tilen koon.
        /// </summary>
        public Size TileSize
        {
            get;
            private set;
        }
        /// <summary>
        /// Palauttaa kartan koon.
        /// </summary>
        public Size MapSize
        {
            get;
            private set;
        }
        /// <summary>
        /// Palauttaa kartan koon pikseleissä.
        /// </summary>
        public Size MapSizeInPixels
        {
            get;
            private set;
        }
        #endregion

        public TileEngine(Size tileSize, Size mapSize)
        {
            TileSize = tileSize;
            MapSize = mapSize;

            MapSizeInPixels = new Size(mapSize.Width * tileSize.Width,
                                       mapSize.Height * tileSize.Height);
        }
    }
}
