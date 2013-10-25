using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khv.Engine.State
{
    /// <summary>
    /// Enum joka kuvastaa ruudun tilannetta
    /// </summary>
    public enum ScreenState
    {
        /// <summary>
        /// Aktiivinen, päivitetään ja piirretään
        /// </summary>
        Active,
        Hidden,
        /// <summary>
        /// Häivytetään sisään
        /// </summary>
        TransitionOn,
        /// <summary>
        /// Häivytetään ulos
        /// </summary>
        TransitionOff,
        /// <summary>
        /// Pyörii taustalla
        /// </summary>
        Background
    }
}
