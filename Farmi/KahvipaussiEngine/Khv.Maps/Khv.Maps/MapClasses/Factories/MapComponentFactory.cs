using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapComponents.Components;
using System.Reflection;
using Khv.Maps.MapClasses.MapComponents;
using Khv.Engine;
using Khv.Maps.MapClasses.Layers;

namespace Khv.Maps.MapClasses.Factories
{
    /// <summary>
    /// Tehdas joka tuottaa kartta komponentteja.
    /// </summary>
    public class MapComponentFactory
    {
        #region Vars
        private readonly string[] componentNamespaces;
        private readonly KhvGame game;
        private readonly TileMap map;
        #endregion

        public MapComponentFactory(KhvGame game, TileMap map, string[] componentNamespaces)
        {
            this.game = game;
            this.map = map;
            this.componentNamespaces = componentNamespaces;
        }

        /// <summary>
        /// Luo uuden komponentin datan perusteella.
        /// </summary>
        public MapComponent MakeNew(MapComponentData mapComponentData)
        {
            Type componentType = null;
            MapComponent mapComponent = null;
            
            foreach (string componentNamespace in componentNamespaces)
            {
                componentType = Type.GetType(componentNamespace + "." + mapComponentData.ComponentName);
                if (componentType != null)
                {
                    break;
                }
            }
            if (componentType != null)
            {
                ILayer layer = map.LayerManager.GetLayer<ILayer>(l => l.Name == mapComponentData.LayerName);

                mapComponent = (MapComponent)Activator.CreateInstance(componentType, game, map, layer);

                mapComponent.Initialize(mapComponentData);
            }

            return mapComponent;
        }
    }
}
