using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;
using Khv.Game;

namespace Farmi.KahvipaussiEngine.Khv.Game.Collision
{
    public class BasicObjectCollisionQuerier : IObjectCollisionQuerier
    {
        public IEnumerable<GameObject> Query(World world, GameObject source)
        {
            List<GameObject> nearGameObjects = world.WorldObjects.AllObjects().ToList();

            if (world.MapManager.ActiveMap != null)
            {
                foreach (GameObjectManager gameobjectManager in world.MapManager.ActiveMap.ObjectManagers.AllManagers())
                {
                    nearGameObjects.AddRange(gameobjectManager.AllObjects());
                }
            }
            return nearGameObjects;
        }
    }
}
