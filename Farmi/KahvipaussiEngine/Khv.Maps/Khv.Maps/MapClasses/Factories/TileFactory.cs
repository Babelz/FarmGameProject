using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Khv.Maps.MapClasses.Layers.Tiles;

namespace Khv.Maps.MapClasses.Factories
{
    /// <summary>
    /// tehdas joka tuottaa tilejä käyttäen apunaan reflectionia
    /// </summary>
    public class TileFactory
    {
        #region Vars
        private readonly Type tileType;
        #endregion

        /// <summary>
        /// alustaa uuden tile tehtaan
        /// </summary>
        /// <param name="tileType">tehtaan tuotteen tyyppi</param>
        public TileFactory(Type tileType)
        {
            this.tileType = tileType;
        }

        /// <summary>
        /// luo uuden tilen tehtaan tyypin perusteella käyttäen
        /// reflectionia
        /// </summary>
        public BaseTile MakeNew(object[] parameters)
        {
            return (BaseTile)Activator.CreateInstance(tileType, parameters);
        }
    }
}
