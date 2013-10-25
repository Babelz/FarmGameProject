using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Engine.Transition
{
    /// <summary>
    /// Kuvaa transitionia joka toimii out of box argumenteilla Initistä
    /// </summary>
    public interface IStandAloneTransition
    {
        void Init(params object[] args);
    }
}
