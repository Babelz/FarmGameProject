using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Input
{
    public enum InputState
    {
        /// <summary>
        /// default
        /// </summary>
        None,
        /// <summary>
        /// kun nappi on juuri painettu
        /// </summary>
        Pressed,
        /// <summary>
        /// Kun nappi on ollut pohjassa ainakin 2 framea
        /// </summary>
        Down,
        /// <summary>
        /// Kun nappi oli viime framella pohjassa mutta nykyisellä ei
        /// </summary>
        Released,
        /// <summary>
        /// Kun nappi on ollut vähintään 2 framea ylhäällä
        /// </summary>
        Up
        
    }
}
