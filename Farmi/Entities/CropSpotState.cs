using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmi.Entities
{
    public enum CropSpotState
    {
        /// <summary>
        /// Ei mitään
        /// </summary>
        Dirt,
        /// <summary>
        /// Tällä on maaperä, mutta siinä ei kasva mitään
        /// </summary>
        Grounded,
        /// <summary>
        /// Tähän on istutettu jotain
        /// </summary>
        Seeded,
        /// <summary>
        /// Tässä on valmis kasvi
        /// </summary>
        Plant
    }
}
