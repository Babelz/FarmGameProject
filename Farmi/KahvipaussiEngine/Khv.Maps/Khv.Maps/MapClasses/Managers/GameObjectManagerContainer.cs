using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Layers;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// Luokka joka sisältää karttaan kuuluvien layereiden 
    /// GameObjectManagerit ja managoi niitä. Jokainen 
    /// instanssi TileMapista omistaa yhden instanssin 
    /// tästä luokasta. 
    /// </summary>
    public class GameObjectManagerContainer : Manager
    {
        #region Vars
        private List<GameObjectManager> gameObjectManagers;
        #endregion

        public GameObjectManagerContainer()
            : base()
        {
            gameObjectManagers = new List<GameObjectManager>();
        }

        #region Add and remove methods
        /// <summary>
        /// Lisää uuden managerin containeriin.
        /// </summary>
        public void AddManager(GameObjectManager gameObjectManager)
        {
            gameObjectManagers.Add(gameObjectManager);
        }
        /// <summary>
        /// Poistaa managerin joka tyättää annetut ehdot.
        /// </summary>
        public void RemoveManager(Predicate<GameObjectManager> predicate)
        {
            gameObjectManagers.Remove(gameObjectManagers.Find(o => predicate(o)));
        }
        #endregion

        #region Query methods
        public bool ContainsGameObject(GameObject gameObject)
        {
            return gameObjectManagers.Find(c => c.Contains(gameObject)) != null;
        }
        public bool HasObjectInBackground(GameObject gameObject)
        {
            return gameObjectManagers.First(c => c.AllObjectsInBackground().Contains(gameObject)) != null;
        }
        /// <summary>
        /// Palauttaa managerin joka täyttää annetut ehdot.
        /// </summary>
        public GameObjectManager GetManager(Predicate<GameObjectManager> predicate)
        {
            return gameObjectManagers.Find(o => predicate(o));
        }
        #endregion

        /// <summary>
        /// Poistaa kaikki managerit joiden InUse value on
        /// false.
        /// </summary>
        public void Update()
        {
            gameObjectManagers.RemoveAll(m => !m.InUse);
        }
        public IEnumerable<GameObjectManager> AllManagers()
        {
            foreach (GameObjectManager gameObjectManager in gameObjectManagers)
            {
                yield return gameObjectManager;
            }
        }
    }
}
