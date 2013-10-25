using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Scripts.CSharpScriptEngine.ScriptClasses
{
    public interface IScriptService : IScriptComponent
    {
        #region Properties
        bool IsRunning
        {
            get;
        }
        #endregion

        void Start();
        void Stop();
    }
}
