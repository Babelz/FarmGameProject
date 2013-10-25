using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Engine.Transition
{
    /// <summary>
    /// TODO: korjaa nimi jollaki kuvaavammalla
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// Kun tullaan näytetään tämä siirtymä
        /// </summary>
        TransitionEffect EnterTransition
        {
            get;
            set;
        }

        /// <summary>
        /// Kun lähdetään näytetään tämä siirtymä
        /// </summary>
        TransitionEffect LeaveTransition
        {
            get;
            set;
        }

        /// <summary>
        /// Mille alueelle tämä transition vaikuttaa
        /// </summary>
        Rectangle Area
        {
            get;
            set;
        }
    }
}
