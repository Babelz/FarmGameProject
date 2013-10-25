using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// kaikkien managerien pohja luokka.
    /// </summary>
    public abstract class Manager
    {
        #region Vars
        public bool InUse
        {
            get;
            set;
        }
        #endregion

        public Manager()
        {
            InUse = true;
        }
    }
}
