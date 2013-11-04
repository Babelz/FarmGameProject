using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Game;

namespace Khv.Game.Collision
{
    public interface IObjectCollisionQuerier
    {
        IEnumerable<GameObject> Query(World world, GameObject source);
    }
}
