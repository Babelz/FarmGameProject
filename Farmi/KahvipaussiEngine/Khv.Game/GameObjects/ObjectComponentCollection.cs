using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Khv.Game.GameObjects
{
    /// <summary>
    /// Luokka jonka tarkoitus on wrapata kaikki peliolion komponentit
    /// samaan luokkaa. Tukee foreach iterointia ja indexillä noutamista.
    /// Ei tue indexillä asettamista. Mäppää 3 komponenttien pää rajapintaa
    /// listoihin. Rajapinnat ovat IObjectComponent, IDrawableObjectComponent ja
    /// IUpdatableObjectComponent.
    /// </summary>
    public sealed class ObjectComponentCollection
    {
        #region Vars
        private readonly List<IObjectComponent> allComponents;
        private readonly List<IDrawableObjectComponent> drawableComponents;
        private readonly List<IUpdatableObjectComponent> updatableComponents;

        private readonly Dictionary<Type, IList> componentLists;

        private readonly List<IObjectComponent> safeRemoveQue;
        private readonly List<IObjectComponent> safeAddQue;
        #endregion

        #region Properties
        public IObjectComponent this[int index]
        {
            get
            {
                return allComponents[index];
            }
        }
        /// <summary>
        /// Palauttaa komponenttien määrän.
        /// </summary>
        public int ComponentCount
        {
            get
            {
                return allComponents.Count;
            }
        }

        /// <summary>
        /// Palauttaa kopion komponentti listasta
        /// </summary>
        public List<IObjectComponent> AllComponents
        {
            get
            {
                return allComponents.ToList();
            }
        }
        #endregion

        public ObjectComponentCollection()
        {
            allComponents = new List<IObjectComponent>();
            drawableComponents = new List<IDrawableObjectComponent>();
            updatableComponents = new List<IUpdatableObjectComponent>();

            componentLists = new Dictionary<Type, IList>();
            componentLists.Add(typeof(IObjectComponent), allComponents);
            componentLists.Add(typeof(IDrawableObjectComponent), drawableComponents);
            componentLists.Add(typeof(IUpdatableObjectComponent), updatableComponents);

            safeRemoveQue = new List<IObjectComponent>();
            safeAddQue = new List<IObjectComponent>();
        }

        // Tarkistaa tyypin ja heittää poikkeuksen jos tyyppi ei ole validi.
        private void CheckType(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException("Tyyppi parametrin (T) tulee olla rajapinta " +
                "IObjectComponent, IDrawableObjectComponent tai IUpdatableObjectComponent.");
            }
        }

        // Palauttaa komponentti listan komponentin tyypin perusteella.
        private IList GetComponentList(Type componentType)
        {
            IList results = null;
            Type[] types = componentType.GetInterfaces();

            // Jos tyyppi on rajapinta, haetaan lista suoriltaan.
            if (componentType.IsInterface)
            {
                results = componentLists[componentType];
            }
            else
            {
                // Jos tyyppi ei ole rajapinta, käydään sen jokainen rajapinta läpi 
                // ja katsotaan mikä niistä matchaa dictin avaimeen.
                foreach (Type type in types)
                {
                    componentLists.TryGetValue(type, out results);
                    if (results != null)
                    {
                        break;
                    }
                }
            }

            return results;
        }
       
        // Lisää komponentin jokaiseen listaan johon se voi kuulua.
        private void AddToLists(IObjectComponent objectComponent)
        {
            // Suorittaa lisäys operaation listaan jos komponenttia ei ole siinä.
            PerformActionWith((list, component) => 
                {
                    if (!list.Contains(component))
                    {
                        list.Add(component);
                    }
                }, objectComponent);
        }
        
        // Poistaa komponentin jokaisesta listasta johon se voisi kuulua.
        private void RemoveFromLists(IObjectComponent objectComponent)
        {
            // Suorittaa poisto operaation listalle jos komponentti on listassa.
            PerformActionWith((list, component) => 
                {
                    if (list.Contains(component))
                    {
                        list.Remove(component);
                    }
                }, objectComponent);
        }
       
        // Suorittaa jonkin operaation listalla ja annetulla komponentilla.
        // Suoritetaan poisto ja lisäys operaatiot tällä metodilla jotta säästytään
        // turhalta koodin duplikoinnilta.
        private void PerformActionWith(Action<IList, IObjectComponent> action, IObjectComponent objectComponent)
        {
            IList list = null;
            Type objectComponentType = objectComponent.GetType();
            Type[] types = objectComponentType.GetInterfaces();
            
            // Jos tyyppi on rajapinta, suoritetaan operaatio heti sillä ja listalla johon se voi kuulua.
            if (objectComponentType.IsInterface)
            {
                list = componentLists[objectComponentType];
                action(list, objectComponent);
                list = null;
            }

            // Käy tyypin jokaisen rajapinnan läpin ja samalla suorittaa jokaisella listalla johon 
            // se voi kuulua operaation.
            foreach (Type type in types)
            {
                componentLists.TryGetValue(type, out list);
                if (list != null)
                {
                    // Jos saatiin lista dictistä, suoritetaan operaatio ja asetetaan listan viite nulliksi.
                    action(list, objectComponent);
                    list = null;
                }
            }
        }

        private void FlushSafeAddQue()
        {
            if (safeAddQue.Count > 0)
            {
                AddComponents(safeAddQue);
                safeAddQue.Clear();
            }
        }
        private void FlushSafeRemoveQue()
        {
            if (safeRemoveQue.Count > 0)
            {
                RemoveComponents(safeRemoveQue);
                safeRemoveQue.Clear();
            }
        }

        public void FlushQues()
        {
            FlushSafeAddQue();
            FlushSafeRemoveQue();
        }

        #region Safe add methods
        public void SafelyAddComponent(IObjectComponent objectComponent)
        {
            safeAddQue.Add(objectComponent);
        }
        public void SafelyAddComponents(IEnumerable<IObjectComponent> objectComponents)
        {
            foreach (IObjectComponent objectComponent in objectComponents)
            {
                safeAddQue.Add(objectComponent);
            }
        }
        public void SafelyAddComponents(params IObjectComponent[] objectComponents)
        {
            SafelyAddComponents(objectComponents.ToList());
        }
        #endregion

        #region Safe remove methods
        public void SafelyRemoveComponent(IObjectComponent objectComponent)
        {
            safeRemoveQue.Add(objectComponent);
        }
        public void SafelyRemoveComponents(IEnumerable<IObjectComponent> objectComponents)
        {
            foreach (IObjectComponent objectComponent in allComponents)
            {
                safeRemoveQue.Add(objectComponent);
            }
        }
        public void SafelyRemoveComponents(params IObjectComponent[] objectComponents)
        {
            SafelyRemoveComponents(objectComponents.ToList());
        }
        #endregion

        #region Add methods
        /// <summary>
        /// Lisää annetun komponentin listoihin.
        /// </summary>
        public void AddComponent(IObjectComponent objectComponent)
        {
            AddToLists(objectComponent);
        }
        /// <summary>
        /// Lisää annetut komponentit listoihin.
        /// </summary>
        public void AddComponents(IEnumerable<IObjectComponent> objectComponents)
        {
            foreach (IObjectComponent objectComponent in objectComponents)
            {
                AddToLists(objectComponent);
            }
        }
        /// <summary>
        /// Lisää annetut komponentit listoihin.
        /// </summary>
        public void AddComponents(params IObjectComponent[] objectComponents)
        {
            AddComponents(objectComponents.ToList());
        }
        #endregion

        #region Remove methods
        /// <summary>
        /// Poistaa annetun komponentin listoista.
        /// </summary>
        public void RemoveComponent(IObjectComponent objectComponent)
        {
            RemoveFromLists(objectComponent);
        }
        /// <summary>
        /// Poistaa annetut komponentit listoista.
        /// </summary>
        public void RemoveComponents(IEnumerable<IObjectComponent> objectComponents)
        {
            foreach (IObjectComponent objectComponent in objectComponents)
            {
                RemoveFromLists(objectComponent);
            }
        }
        /// <summary>
        /// Poistaa annetut komponentit listoista.
        /// </summary>
        public void RemoveComponents(params IObjectComponent[] objectComponents)
        {
            RemoveComponents(objectComponents.ToList());
        }
        #endregion

        #region Query methods
        /// <summary>
        /// Palauttaa komponentin joka täyttää ehdon ensimmäisenä.
        /// Jos predikaatti on null, palauttaa ensimmäisen T:n tyyppiä
        /// olevan komponentin.
        /// </summary>
        public T GetComponent<T>(Predicate<T> predicate = null) where T : class, IObjectComponent
        {
            IList list = GetComponentList(typeof(T));

            if (predicate == null)
            {
                foreach (IObjectComponent objectComponent in list)
                {
                    T result = objectComponent as T;
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                foreach (IObjectComponent objectComponent in list)
                {
                    T result = objectComponent as T;
                    if (result != null && predicate(result))
                    {
                        return result;
                    }
                }
            }

            return default(T);
        }
        /// <summary>
        /// Palauttaa komponentin joka täyttää ehdon ensimmäisenä.
        /// </summary>
        public IObjectComponent GetComponent(Predicate<IObjectComponent> predicate)
        {
            return allComponents.Find(c => predicate(c));
        }
        /// <summary>
        /// Palauttaa truen jos jokin komponentti täyttää ehdon.
        /// </summary>
        public bool ContainsComponent(Predicate<IObjectComponent> predicate)
        {
            return allComponents.Find(c => predicate(c)) != null;
        }
        /// <summary>
        /// Palauttaa truen jos jokin komponentti täyttää ehdon.
        /// </summary>
        public bool ContainsComponent(IObjectComponent objectComponent)
        {
            return allComponents.Contains(objectComponent);
        }
        #endregion

        /// <summary>
        /// Palauttaa iterointia varten kokoelman halutun tyyppisistä komponenteista.
        /// Jos halutaan iteroida kaikki komponentit läpi, käytetään pohja rajapintaa (IObjectComponent)
        /// tyyppi argumenttina.
        /// </summary>
        public IEnumerable<T> ComponentsOfType<T>(Predicate<T> predicate = null) where T : IObjectComponent
        {
            CheckType(typeof(T));

            FlushQues();

            IList list = allComponents
                .Where(c => c.GetType().GetInterfaces().Contains(typeof(T)))
                .ToList();

            if (predicate == null)
            {
                foreach (T objectComponent in list)
                {
                    yield return objectComponent;
                }
            }
            else
            {
                foreach (T objectComponent in list)
                {
                    if (predicate(objectComponent))
                    {
                        yield return objectComponent;
                    }
                }
            } 
        }
    }
}
