using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;
using Khv.Engine;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;

namespace Khv.Maps.MapClasses.Layers.Components
{
    public class ComponentBuilder
    {
        #region Vars
        private readonly KhvGame game;
        #endregion

        public ComponentBuilder(KhvGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// Rakentaa kaikki tarvittavat komponentit annetulle
        /// layerille sen geneerisen tyypin perusteella.
        /// </summary>
        public List<LayerComponent> BuildComponents(ILayer layer)
        {
            List<LayerComponent> components = new List<LayerComponent>();

            // Lisää animation komponentin jos layeri sisältää AnimationTilejä.
            if (GetGenericType(layer) == typeof(AnimationTile)) 
            {
                components.Add(new AnimationComponent(game, layer));
            }

            // Lisäää tile tai animaatio piirtäjän.
            if (GetGenericType(layer) == typeof(AnimationTile))
            {
                components.Add(new TileDrawingComponent<AnimationTile>(game, layer));
            }
            else if (GetGenericType(layer) == typeof(Tile))
            {
                components.Add(new TileDrawingComponent<Tile>(game, layer));
            }

            return components;
        }
        private Type GetGenericType(ILayer layer)
        {
            return layer.GetType().GetGenericArguments()[0];
        }
    }
}
