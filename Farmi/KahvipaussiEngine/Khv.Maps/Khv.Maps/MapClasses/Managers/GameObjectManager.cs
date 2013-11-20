using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Game.GameObjects;
using System.Collections;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// Luokka joka sisältää peliobjekteja. Jokainen object layer omistaa
    /// instanssin tästä oliosta. Manageri on tietoinen layeristä joka
    /// omistaa sen, mutta layeri on tietoinen tästä managerista vain
    /// layercomponentin välityksellä joka
    /// hoitaa objektien piirtämisen ja päivittämisen.
    /// </summary>
    public class GameObjectManager : Manager
    {
        #region Vars
        private readonly Layer<ObjectTile> owner;

        private readonly Dictionary<Type, IList> objectLists;
        private readonly List<GameObject> allObjects;
        private readonly List<DrawableGameObject> drawableObjects;

        private readonly List<GameObject> safeAddQue;
        private readonly List<GameObject> safeRemoveQue;

        private GameObjectManager backgroundObjectManager;
        #endregion

        #region Properties
        public Layer<ObjectTile> Owner
        {
            get
            {
                return owner;
            }
        }
        public bool IsOwnedByLayer
        {
            get
            {
                return owner != null;
            }
        }
        public int Count
        {
            get
            {
                return allObjects.Count;
            }
        }
        public GameObject this[int index]
        {
            get
            {
                return allObjects[index];
            }
        }
        public bool HasBackgroundObjects
        {
            get
            {
                if (CanTransferObjectsToBackground)
                {
                    return backgroundObjectManager.Count > 0;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool CanTransferObjectsToBackground
        {
            get
            {
                return backgroundObjectManager != null;
            }
        }
        #endregion

        public GameObjectManager()
            : this(null)
        {
        }
        public GameObjectManager(Layer<ObjectTile> owner)
            : base()
        {
            this.owner = owner;

            allObjects = new List<GameObject>();
            drawableObjects = new List<DrawableGameObject>();

            objectLists = new Dictionary<Type, IList>();
            objectLists.Add(typeof(DrawableGameObject), drawableObjects);
            objectLists.Add(typeof(GameObject), allObjects);

            safeAddQue = new List<GameObject>();
            safeRemoveQue = new List<GameObject>();
        }

        #region Transfer methods
        /// <summary>
        /// Enabloi oliojen taustalle siirtämisen.
        /// </summary>
        public void EnableBackgroundTransfers()
        {
            if (!CanTransferObjectsToBackground)
            {
                backgroundObjectManager = new GameObjectManager();
            }
        }
        /// <summary>
        /// Disabloi oliojen taustalle siirtämisen.
        /// </summary>
        /// <param name="transfer">Tuleeko taustalla olevat oliot siirtää takaisin tälle managerille.</param>
        public void DisableBackgroundTransfers(bool transfer)
        {
            if (CanTransferObjectsToBackground)
            {
                if (transfer)
                {
                    MoveAllToForeground();
                }
                else
                {
                    backgroundObjectManager = null;
                }
            }
        }
        /// <summary>
        /// Siirtää olion tustalle.
        /// </summary>
        public void MoveToBackground<T>(T gameObject) where T : GameObject
        {
            if (Contains(gameObject))
            {
                SafelyRemove(gameObject);
                backgroundObjectManager.AddGameObject(gameObject);
            }
        }
        /// <summary>
        /// Siirtää oliot taustalle.
        /// </summary>
        public void MoveToBackground<T>(IEnumerable<T> gameObjects) where T : GameObject
        {
            foreach (T gameObject in gameObjects.Where(o => Contains(o)))
            {
                SafelyRemove(gameObject);
                backgroundObjectManager.AddGameObject(gameObject);
            }
        }
        /// <summary>
        /// Siirtää oliot taustalle.
        /// </summary>
        public void MoveToBackground<T>(params T[] gameObjects) where T : GameObject
        {
            MoveToBackground<T>(gameObjects.ToList());
        }

        /// <summary>
        /// Siirtää kaikki taustalla olevat oliot takaisin 
        /// tähän manageriin.
        /// </summary>
        public void MoveAllToForeground()
        {
            SafelyAddMany(backgroundObjectManager.allObjects);
            backgroundObjectManager.Clear();
        }

        /// <summary>
        /// Siirtää halutun olion takaisin tähän manageriin.
        /// </summary>
        public void MoveToForeground<T>(T gameObject) where T : GameObject
        {
            if (backgroundObjectManager.allObjects.Contains(gameObject))
            {
                SafelyAdd(gameObject);
                backgroundObjectManager.RemoveGameObject(gameObject);
            }
        }
        /// <summary>
        /// Siirtää halutut olion takaisin tähän manageriin.
        /// </summary>
        public void MoveToForeground<T>(IEnumerable<T> gameObjects) where T : GameObject
        {
            foreach (T gameObject in gameObjects.Where(o => backgroundObjectManager.allObjects.Contains(o)))
            {
                SafelyAdd(gameObject);
                backgroundObjectManager.RemoveGameObject(gameObject);
            }
        }
        /// <summary>
        /// Siirtää halutut olion takaisin tähän manageriin.
        /// </summary>
        public void MoveToForeground<T>(params T[] gameObjects) where T : GameObject
        {
            MoveToForeground<T>(gameObjects.ToList());
        }
        #endregion

        #region Safe add methods
        /// <summary>
        /// Lisää safe add queen uuden olion.
        /// </summary>
        public void SafelyAdd(GameObject gameObject)
        {
            safeAddQue.Add(gameObject);
        }
        /// <summary>
        /// Lisää safe add queen monta olioa.
        /// </summary>
        public void SafelyAddMany(IEnumerable<GameObject> gameObjects)
        {
            safeAddQue.AddRange(gameObjects);
        }
        /// <summary>
        /// Lisää safe add queen monta olioa.
        /// </summary>
        public void SafelyAddMany(params GameObject[] gameObjects)
        {
            safeAddQue.AddRange(gameObjects);
        }
        /// <summary>
        /// Lisää kaikki safe add quessa olevat oliot
        /// manageriin.
        /// </summary>
        public void FlushAddQue()
        {
            if (safeAddQue.Select(o => o.GetType()).Distinct().Count() >= 3)
            {
                AddGameObjects(safeAddQue);
            }
            else
            {
                safeAddQue.ForEach(
                    o => AddGameObject(o));
            }

            safeAddQue.Clear();
        }
        #endregion

        #region Safe remove methods
        /// <summary>
        /// Lisää olion safe remove queen.
        /// </summary>
        public void SafelyRemove(GameObject gameObject)
        {
            safeRemoveQue.Add(gameObject);
        }
        /// <summary>
        /// Lisää olion safe remove queen joka 
        /// täyttää ensimmäisenä ehdon.
        /// </summary>
        public void SafelyRemove(Predicate<GameObject> predicate)
        {
            GameObject gameObject = GetGameObject<GameObject>(predicate);

            if (gameObject != null)
            {
                safeRemoveQue.Add(gameObject);
            }
        }
        /// <summary>
        /// Lisää olion safe remove queen joka 
        /// täyttää ensimmäisenä ehdon.
        /// </summary>
        public void SafelyRemove<T>(Predicate<T> predicate) where T : GameObject
        {
            T gameObject = GetGameObject<T>(predicate);

            if (gameObject != null)
            {
                safeRemoveQue.Add(gameObject);
            }
        }
        /// <summary>
        /// Lisää argumenttina saadut oliot safe remove queen.
        /// </summary>
        public void SafelyRemove<T>(IEnumerable<T> objectsToRemove) where T : GameObject
        {
            safeRemoveQue.AddRange(objectsToRemove);
        }
        /// <summary>
        /// Lisää kaikki oliot jotka täyttävät ehdon safe
        /// remove queen.
        /// </summary>
        public void SafelyRemoveAll<T>(Predicate<T> predicate) where T : GameObject
        {
            IEnumerable<T> objectsToRemove = GetMany<T>(predicate);

            if (objectsToRemove.Count() > 0)
            {
                safeRemoveQue.AddRange(objectsToRemove);
            }
        }
        /// <summary>
        /// Poistaa managerista kaikki safe remove 
        /// quen oliot.
        /// </summary>
        public void FlushRemoveQue()
        {
            if (safeRemoveQue.Select(o => o.GetType()).Distinct().Count() >= 3)
            {
                RemoveGameObjects(safeRemoveQue);
            }
            else
            {
                safeRemoveQue.ForEach(
                    o => RemoveGameObject(o));
            }

            safeRemoveQue.Clear();
        }
        #endregion

        #region Add methods
        /// <summary>
        /// Lisää uuden peliobjektin manageriin.
        /// </summary>
        public void AddGameObject(GameObject gameObject)
        {
            IList list = GetObjectList(gameObject.GetType());
            AddToLists(list, gameObject);
        }
        /// <summary>
        /// Lisää monta peliobjektia manageriin.
        /// </summary>
        public void AddGameObjects(IEnumerable<GameObject> gameObjectList)
        {
            IList list = allObjects;
            var sortedObjects = gameObjectList.OrderBy(o => o.GetType().Name);

            foreach (GameObject gameObject in sortedObjects)
            {
                if (list.GetType().GetGenericArguments().First() != gameObject.GetType())
                {
                    list = GetObjectList(gameObject.GetType());
                }

                AddToLists(list, gameObject);
            }
        }
        /// <summary>
        /// Lisää monta peliobjektia manageriin.
        /// </summary>
        /// <param name="gameObjects"></param>
        public void AddGameObjects(params GameObject[] gameObjects)
        {
            AddGameObjects(gameObjects.ToList());
        }
        #endregion

        #region Remove methods
        /// <summary>
        /// Poistaa annetun peliobjektin managerista.
        /// </summary>
        /// <param name="gameObject"></param>
        public void RemoveGameObject(GameObject gameObject)
        {
            IList list = TryGetObjectList(gameObject.GetType());

            if (list != null)
            {
                RemoveFromLists(list, gameObject);

                RemoveIfEmpty(list.GetType().GetGenericArguments().First());
            }
        }
        /// <summary>
        /// Poistaa peliobjektin managerista joka täyttää ehdon.
        /// </summary>
        public void RemoveGameObject(Predicate<GameObject> predicate)
        {
            IList list = TryGetObjectList(predicate.GetType().GetGenericArguments().First());

            if (list != null)
            {
                GameObject gameObject = allObjects.Find(o => predicate(o));

                RemoveFromLists(list, gameObject);

                RemoveIfEmpty(list.GetType().GetGenericArguments().First());
            }
        }
        /// <summary>
        /// Poistaa annettua tyyppiä olevan peliobjektin managerista
        /// joka täyttää ehdon.
        /// </summary>
        public void RemoveGameObject<T>(Predicate<T> predicate) where T : GameObject
        {
            List<T> list = GetObjectList<T>(typeof(T));

            if (list != null)
            {
                T gameObject = list.Find(o => predicate(o));

                RemoveFromLists(list, gameObject);

                RemoveIfEmpty(list.GetType().GetGenericArguments().First());
            }
        }
        /// <summary>
        /// Poistaa kaikki annettua tyyppiä olevat peliobjektit managerista.
        /// </summary>
        public void RemoveGameObjects(IEnumerable<GameObject> objectsToRemove)
        {
            List<GameObject> objects = objectsToRemove
                .OrderBy(o => o.GetType().Name)
                .ToList();
            IList lastObjectList = null;
            IList currentObjectList = GetObjectList(objects.First().GetType());

            foreach (GameObject gameObject in objects)
            {
                if (currentObjectList.GetType().GetGenericArguments().First() != gameObject.GetType())
                {
                    lastObjectList = currentObjectList;
                    currentObjectList = GetObjectList(gameObject.GetType());
                    RemoveIfEmpty(lastObjectList.GetType().GetGenericArguments().First());
                }

                RemoveFromLists(currentObjectList, gameObject);
            }

            RemoveIfEmpty(currentObjectList.GetType().GetGenericArguments().First());
        }
        /// <summary>
        /// Poistaa kaikki tyyppiä olevat peliobjektit managerista jotka täyttävät ehdon.
        /// </summary>
        public void RemoveAll<T>(Predicate<T> predicate) where T : GameObject
        {
            List<T> list = GetObjectList<T>(typeof(T));

            if (list != null)
            {
                List<T> objectsToRemove = list.FindAll(o => predicate(o));

                foreach (T gameObject in objectsToRemove)
                {
                    RemoveFromLists(list, gameObject);
                }

                RemoveIfEmpty(list.GetType().GetGenericArguments().First());
            }
        }
        #endregion

        #region Query methods
        /// <summary>
        /// Palauttaa tyyppiä olevan peliobjektin managerista joka täyttää annetun ehdon.
        /// </summary>
        public T GetGameObject<T>(Predicate<T> preidcate) where T : GameObject
        {
            T results = null;
            List<T> list = GetObjectList<T>(typeof(T));

            if (list != null)
            {
                results = list.Find(o => preidcate(o));
            }

            return results;
        }
        /// <summary>
        /// Palauttaa kaikki tyyppiä olevat objektit jotka täyttävät annetun ehdot.
        /// </summary>
        public IEnumerable<T> GetMany<T>(Predicate<T> predicate) where T : GameObject
        {
            IEnumerable<T> results = null;
            List<T> list = GetObjectList<T>(typeof(T));

            if (list != null)
            {
                results = list.Where(o => predicate(o));
            }

            return results;
        }
        /// <summary>
        /// Palauttaa truen jos manager omaa argumenttina
        /// saadun olion.
        /// </summary>
        public bool Contains(GameObject gameObject)
        {
            return allObjects.Contains(gameObject);
        }
        #endregion

        #region IEnumerable methods
        /// <summary>
        /// Palauttaa kaikki peliobjektit jotka ovat taustalla iterointia varten.
        /// </summary>
        public IEnumerable<GameObject> AllObjectsInBackground(Predicate<GameObject> predicate = null)
        {
            return backgroundObjectManager.GetIterator<GameObject>(backgroundObjectManager.allObjects, predicate);
        }
        /// <summary>
        /// Palauttaa tyyppinä syötetyt peliobjektit jotka ovat taustalla iterointia varten.
        /// </summary>
        public IEnumerable<T> GameObjectsOfTypeInBackground<T>(Predicate<T> predicate = null) where T : GameObject
        {
            return backgroundObjectManager.GameObjectsOfType<T>(predicate);
        }
        /// <summary>
        /// Palauttaa kaikki managerin peliobjektit iterointia varten.
        /// </summary>
        public IEnumerable<GameObject> AllObjects(Predicate<GameObject> predicate = null)
        {
            return GetIterator<GameObject>(allObjects, predicate);
        }
        /// <summary>
        /// Palauttaa tyyppinä syötetyt peliobjektit iterointia varten.
        /// </summary>
        public IEnumerable<T> GameObjectsOfType<T>(Predicate<T> predicate = null) where T : GameObject
        {
            // Flussitaan aluksi quet jos ollaan poistettu 
            // tai lisätty olioja safe metodeilla.
            FlushQues();

            List<T> list = GetObjectList<T>(typeof(T));

            return GetIterator<T>(list, predicate);
        }
        /// <summary>
        /// Iteroi jokaisen otuksen kokoelmassa läpi ehdolla tai ilman.
        /// </summary>
        private IEnumerable<T> GetIterator<T>(IList<T> collection, Predicate<T> predicate = null) where T : GameObject
        {
            if (predicate == null)
            {
                foreach (T gameObject in collection)
                {
                    yield return gameObject;
                }
            }
            else
            {
                foreach (T gameObject in collection.Where(o => predicate(o)))
                {
                    yield return gameObject;
                }
            }
        }
        #endregion

        public void Clear()
        {
            objectLists.Clear();
            drawableObjects.Clear();
            allObjects.Clear();

            safeAddQue.Clear();
            safeRemoveQue.Clear();

            if (CanTransferObjectsToBackground)
            {
                backgroundObjectManager.Clear();
            }
        }

        /// <summary>
        /// Flushaa remove ja add quen.
        /// </summary>
        public void FlushQues()
        {
            if (safeAddQue.Count > 0)
            {
                FlushAddQue();
            }
            if (safeRemoveQue.Count > 0)
            {
                FlushRemoveQue();
            }
        }

        /// <summary>
        /// Palauttaa aina objekti listan. Luo aina listan jos
        /// sitä ei ole jo olemassa.
        /// </summary>
        private IList GetObjectList(Type type)
        {
            IList list;

            try
            {
                list = objectLists[type];
            }
            catch
            {
                list = MakeObjectListOfType(type);
                objectLists.Add(type, list);
            }

            return list;
        }

        /// <summary>
        /// Palauttaa generisen version halutusta listasta.
        /// </summary>
        private List<T> GetObjectList<T>(Type type)
        {
            return GetObjectList(type) as List<T>;
        }

        /// <summary>
        /// Yrittää palauttaa objekti listan.
        /// </summary>
        private IList TryGetObjectList(Type type)
        {
            IList list;

            objectLists.TryGetValue(type, out list);

            return list;
        }

        /// <summary>
        /// Luo listan tyypin perusteella.
        /// </summary>
        private IList MakeObjectListOfType(Type type)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        }

        /// <summary>
        /// Poistaa listan jos se on tyhjä.
        /// </summary>
        private void RemoveIfEmpty(Type type)
        {
            IList list = objectLists[type];

            if (list.Count <= 0 && list != allObjects && list != drawableObjects)
            {
                objectLists.Remove(type);
            }
        }

        /// <summary>
        /// Poistaa olion listasta ja allobjects listasta jos sitä ei poisteta heti alussa.
        /// </summary>
        private void RemoveFromLists<T>(IList list, T gameObject) where T : GameObject
        {
            list.Remove(gameObject);

            // Koska ylempi listä voi olla allobjects lista, katsotaan ollaanko siitä jo poistettu.
            if (list != allObjects)
            {
                allObjects.Remove(gameObject);
            }
            if (list != drawableObjects)
            {
                DrawableGameObject drawableObject = gameObject as DrawableGameObject;
                if (drawableObject != null)
                {
                    drawableObjects.Remove(drawableObject);
                }
            }
            
        }

        /// <summary>
        /// Lisää olion listaan ja allobjects listaan jos sitä ei lisätä heti alussa.
        /// </summary>
        private void AddToLists<T>(IList list, T gameObject) where T : GameObject
        {
            // Ei lisätä samaa olio viitettä uudestaan.
            if (list.Contains(gameObject))
            {
                return;
            }

            list.Add(gameObject);

            // Koska ylempi listä voi olla allobjects lista, katsotaan ollaanko siihen jo lisätyy.
            if (list != allObjects)
            {
                allObjects.Add(gameObject);
            }
            if (list != drawableObjects)
            {
                DrawableGameObject drawableObject = gameObject as DrawableGameObject;
                if (drawableObject != null)
                {
                    drawableObjects.Add(drawableObject);
                }
            }
        }
    }
}
