using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.MapComponents;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// Manageri joka sisältää kartan komponentit kuten
    /// liikkuvat layerit. Jokainen instanssi TileMapista 
    /// omistaa instanssin tästä oliosta.
    /// </summary>
    public class MapComponentManager : Manager
    {
        #region Vars
        private List<MapComponent> components;
        #endregion

        public MapComponentManager()
            : base()
        {
            components = new List<MapComponent>();
        }

        public void AddComponent(MapComponent mapComponent)
        {
            components.Add(mapComponent);
        }
        public void RemoveComponent(MapComponent mapComponent)
        {
            components.Remove(mapComponent);
        }
        public T GetComponent<T>(Predicate<T> predicate) where T : MapComponent
        {
            return (T)components.Find(c => c is T && predicate((T)c));
        }
        public void Update(GameTime gameTime)
        {
            components.ForEach(o =>
                {
                    if (o.Enabled)
                    {
                        o.Update(gameTime);
                    }
                });
        }
    }
}
