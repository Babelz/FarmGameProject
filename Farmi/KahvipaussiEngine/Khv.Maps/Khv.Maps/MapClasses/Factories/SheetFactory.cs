using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;
using Microsoft.Xna.Framework.Content;

namespace Khv.Maps.MapClasses.Factories
{
    /// <summary>
    /// tehdas joka tuottaa sheettejä layerin tyypin perusteella
    /// käyttäen reflectionia
    /// </summary>
    public class SheetFactory
    {
        /// <summary>
        /// luo uuden sheetin layerin ja parametrien perusteella
        /// </summary>
        /// <param name="typeOfLayer">layerin tyyppi</param>
        /// <param name="parameters">sheetin parametrit</param>
        /// <returns>sheetti joka on luotu parametrien ja tyypin perusteella</returns>
        public Sheet MakeNew(Type typeOfLayer, object[] parameters)
        {
            // hakee layerin geneerisen tyypin
            Type[] types = typeOfLayer.GetGenericArguments();

            // hakee nimen
            string sheetName = types[0].Name + "Sheet";

            // hakee sheetin typin nimen perusteella
            Type sheetType = Type.GetType("Khv.Maps.MapClasses.MapComponents.Layers.Sheets." + sheetName);

            return (Sheet)Activator.CreateInstance(sheetType, parameters);
        }
    }
}
