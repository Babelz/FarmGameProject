using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Game.GameObjects
{
    public interface IObjectComponent
    {
        void Update(GameTime gametime);
    }
}
