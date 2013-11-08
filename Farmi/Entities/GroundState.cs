using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmi.Entities
{
    public enum GroundState
    {
        /// <summary>
        /// Default, tähän voi istuttaa kasvin
        /// </summary>
        Hoed,
        /// <summary>
        /// Tähän maaperään on pistetty kasvi
        /// </summary>
        Planted,
        /// <summary>
        /// Tätä maaperää on kasteltu
        /// </summary>
        Watered,
        /// <summary>
        /// Tämä maaperä on kuiva (tarvii vettä)
        /// </summary>
        Dry,
        /// <summary>
        /// Tämä maaperä on lumen peitossa
        /// </summary>
        Snow
    }
}
