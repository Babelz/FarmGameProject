using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Khv.Input
{
    public class KeyTrigger : ITrigger
    {
        #region Properties

        public Keys Key { get; private set; }
        public Keys[] AlternateKeys { get; private set; }
        public string Name { get; private set; }

        #endregion

        public KeyTrigger(string name, Keys key, params Keys[] alternateKeys)
        {
            Name = name;
            Key = key;
            if (alternateKeys == null)
                alternateKeys = new Keys[0];
            AlternateKeys = alternateKeys;
        }
    }
}
