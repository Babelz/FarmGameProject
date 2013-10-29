using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.World;
using Khv.Game.GameObjects;


namespace Farmi.KahvipaussiEngine.Khv.Game.Collision
{
    public interface IObjectCollisionQuerier
    {
        IEnumerable<GameObject> Query(global::Khv.Game.World world, GameObject source);
    }
}
