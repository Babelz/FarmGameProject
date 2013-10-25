using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapClasses.Layers.Components
{
    /// <summary>
    /// pelin karttojen säännöt
    /// </summary>
    public enum Rules
    {
        Blocked,
        Damaging,
        None
    }

    /// <summary>
    /// luokka joka helpottaa rulejen asettamista tileille
    /// </summary>
    public class RuleHelper
    {
        /// <summary>
        /// palauttaa Rules enum arvon stringin perusteella
        /// </summary>
        /// <param name="ruleName">rulen nimi tiedostossa</param>
        /// <returns>rule arvo</returns>
        public Rules GetRuleByName(string ruleName)
        {
            return (Rules)Enum.Parse(typeof(Rules), ruleName);
        }
    }
}
