using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.World;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Layers.Tiles;
namespace Farmi.KahvipaussiEngine.Khv.Game.Collision
{
    public interface ITileCollisionQuerier
    {
        IEnumerable<RuleTile> Query(global::Khv.Game.World world, GameObject source);
    }
}
