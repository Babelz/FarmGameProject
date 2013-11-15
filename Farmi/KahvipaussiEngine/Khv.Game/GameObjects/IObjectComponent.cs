using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Game.GameObjects
{
    /// <summary>
    /// Rajapinta komponenteille jotka voivat suorittaa päivityksiä.
    /// </summary>
    public interface IUpdatableObjectComponent : IObjectComponent
    {
        void Update(GameTime gametime);
    }

    /// <summary>
    /// Markkeri rajapinta object komponentille.
    /// </summary>
    public interface IObjectComponent
    {
    }
}
