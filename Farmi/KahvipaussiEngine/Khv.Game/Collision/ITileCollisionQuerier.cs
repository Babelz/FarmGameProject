using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Game;

namespace Khv.Game.Collision
{
    public interface ITileCollisionQuerier
    {
        IEnumerable<RuleTile> Query(World world, GameObject source);
    }
}
