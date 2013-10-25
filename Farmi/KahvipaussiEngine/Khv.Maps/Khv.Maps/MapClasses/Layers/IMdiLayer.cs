using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.Layers
{
    /// <summary>
    /// rajapinta josta mdilayerit johdetaan
    /// </summary>
    public interface IMdiLayer : ILayer
    {
        #region Properties
        /// <summary>
        /// layerin tämän hetkinen sijainti
        /// </summary>
        Vector2 Position
        {
            get;
            set;
        }
        /// <summary>
        /// layerin alkuperäinen sijainti
        /// </summary>
        Vector2 OriginalPosition
        {
            get;
        }
        /// <summary>
        /// palauttaa sijainti indeksin, tulee käyttää kun
        /// sijainti on jaettavissa kahdella
        /// </summary>
        Point Index
        {
            get;
        }
        /// <summary>
        /// palauttaa mdi layerin rectanglen koon
        /// </summary>
        Size RectangleSize
        {
            get;
        }
        #endregion

        // liikuttaa layerin haluttuu indeksiin
        void MoveToIndex(Index index);
        // liikuttaa layerin haluttuun kohtaan
        void MoveTo(float x, float y);
        // liikuttaa layeriä halutulla valuella
        void MoveBy(float x, float y);
    }
}
