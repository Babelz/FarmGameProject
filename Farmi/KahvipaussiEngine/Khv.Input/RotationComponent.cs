using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Khv.Input
{
    public interface IRotationComponent : IObjectComponent
    {
        #region Properties

        float Rotation
        {
            get;
        }


        #endregion
    }
}
