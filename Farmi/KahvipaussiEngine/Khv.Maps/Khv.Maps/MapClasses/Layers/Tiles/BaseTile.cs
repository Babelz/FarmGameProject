using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Sheets;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;

namespace Khv.Maps.MapClasses.Layers.Tiles
{
    /// <summary>
    /// kaikkien tilejen perus luokka
    /// </summary>
    public abstract class BaseTile
    {
        #region Vars
        protected Vector2 position;
        #endregion

        #region Properties
        /// <summary>
        /// tilen sijanti
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        /// <summary>
        /// sheetin base muoto
        /// </summary>
        public Sheet Sheet
        {
            get;
            set;
        }
        #endregion

        public BaseTile(Vector2 position)
        {
            Position = position;
        }

        // kertoo onko tilellä tekstuuria tai objektia
        public abstract bool IsEmpty();
        // tyhjentää tilen tekstuuri indeksin tai nullaa objekti viitteen
        public abstract void Clear();

        public void Move(Vector2 position)
        {
            this.position = position;
        }
    }
}
