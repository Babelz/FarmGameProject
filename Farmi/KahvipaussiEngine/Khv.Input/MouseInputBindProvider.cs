using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Khv.Input
{
    public class MouseInputBindProvider : IInputBindProvider
    {
        #region Properties
        public Dictionary<string, Binding> Bindings { get; private set; }
        #endregion

        public IEnumerable<IInputCallbacker> Update(Dictionary<Type, IInputStateProvider> providers, GameTime gameTime)
        {
            MouseStateProvider provider = (MouseStateProvider) providers[typeof (MouseStateProvider)];

            return new List<IInputCallbacker>();
        }
    }
}
