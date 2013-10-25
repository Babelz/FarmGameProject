using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Khv.Maps.MapClasses.MapComponents.Layers.Sheets;
using System;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Layers;

namespace Khv.Maps.MapClasses
{
    /// <summary>
    /// tilekartta objekti 
    /// </summary>
    public class TileMap 
    {
        #region Vars
        private string name;
        private TileEngine tileEngine;
        private LayerManager layerManager;
        private DrawOrderManager drawOrderManager;
        private GameObjectManagerContainer mapObjectManager;
        private MapComponentManager mapComponentManager;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// kartan tile moottori
        /// </summary>
        public TileEngine TileEngine
        {
            get
            {
                return tileEngine;
            }
        }
        /// <summary>
        /// mapin layermanager jossa layerit asuvat
        /// </summary>
        public LayerManager LayerManager
        {
            get
            {
                return layerManager;
            }
        }
        /// <summary>
        /// mapin layereiden draworder manager
        /// </summary>
        public DrawOrderManager DrawOrderManager
        {
            get
            {
                return drawOrderManager;
            }
        }
        /// <summary>
        /// kartan objekti manageri joka sisältää
        /// kaikkien objekti layereiden obejti kokoelmat
        /// </summary>
        public GameObjectManagerContainer ObjectManagers
        {
            get
            {
                return mapObjectManager;
            }
        }
        /// <summary>
        /// kartan komponenttien managoija
        /// </summary>
        public MapComponentManager ComponentManager
        {
            get
            {
                return mapComponentManager;
            }
        }
        #endregion

        public TileMap(TileEngine tileEngine, string name)
        {
            this.tileEngine = tileEngine;
            this.name = name;

            layerManager = new LayerManager();
            drawOrderManager = new DrawOrderManager();
            mapObjectManager = new GameObjectManagerContainer();
            mapComponentManager = new MapComponentManager();
        }

        /// <summary>
        /// päivittää kartaa objektit ja animaatio layerit
        /// </summary>
        /// <param name="gameTime">xna peli aika</param>
        public void Update(GameTime gameTime)
        {
            // Päivittää kaikki komponentit ja objektit.
            mapComponentManager.Update(gameTime);
            mapObjectManager.Update();

            foreach (ILayer layer in layerManager.AllLayers())
            {
                layer.Update(gameTime);
            }
        }
        /// <summary>
        /// pirtää layerit niiden piirto järjestyksen mukaan
        /// </summary>
        /// <param name="spriteBatch">xna spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ILayer layer in LayerManager.AllLayers().OrderBy(l => l.DrawOrder.Value))
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}
