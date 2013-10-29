using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.World;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Tiles;
using Microsoft.Xna.Framework;

namespace Farmi.KahvipaussiEngine.Khv.Game.Collision
{
    public class BasicTileCollisionQuerier : ITileCollisionQuerier
    {
        public IEnumerable<RuleTile> Query(global::Khv.Game.World world, GameObject source)
        {
            Layer<RuleTile> rules = world.MapManager.ActiveMap.LayerManager.AllLayers().First(l => l is Layer<RuleTile>) as Layer<RuleTile>;

            if (rules == null)
                yield break;

            RuleTile[][] tiles = rules.GetSurroundingTiles(source.Position);
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    RuleTile tile = tiles[i][j];
                    if (tile == null)
                        continue;
                    yield return tile;
                }
            }
        }
    }
}
