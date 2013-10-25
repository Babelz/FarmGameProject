using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Khv.Maps.MapClasses.Layers.Sheets.BaseClasses
{
    /// <summary>
    /// Kaikkien sheettien perus luokka
    /// </summary>
    public abstract class Sheet
    {
        #region Vars
        // path on sheetin tietojen sijainti,
        // kuva tai objekti tiedosto
        protected string path;
        #endregion

        public Sheet(string path)
        {
            this.path = path;
        }

        // alustaa sheetin ja sen tilet
        protected abstract void Initialize();
    }
}
