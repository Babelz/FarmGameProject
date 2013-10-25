using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Input
{
    public interface IInputStateProvider
    {
        /// <summary>
        /// Päivittää statea
        /// </summary>
        void Update();
    }
}
