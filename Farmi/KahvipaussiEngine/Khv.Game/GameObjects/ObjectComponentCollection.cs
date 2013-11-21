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
        private void AddToLists<T>(T objectComponent) where T : IObjectComponent
        {
            // Suorittaa lisäys operaation listaan jos komponenttia ei ole siinä.
            PerformActionWith<T>((list, component) => 
                {
                    if (!list.Contains(component))
                    {
                        list.Add(component);
                    }
                }, objectComponent);
        }
        
        // Poistaa komponentin jokaisesta listasta johon se voisi kuulua.
        private void RemoveFromLists<T>(T objectComponent) where T : IObjectComponent
        {
            // Suorittaa poisto operaation listalle jos komponentti on listassa.
            PerformActionWith<T>((list, component) => 
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
        private void PerformActionWith<T>(Action<IList, T> action, T objectComponent) where T : IObjectComponent
        {
            IList list = null;
            Type[] types = typeof(T).GetInterfaces();
            
            // Jos tyyppi on rajapinta, suoritetaan operaatio heti sillä ja listalla johon se voi kuulua.
            if (typeof(T).IsInterface)
            {
                list = componentLists[typeof(T)];
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
                AddComponents<IObjectComponent>(safeAddQue);
                safeAddQue.Clear();
            }
        }
        private void FlushSafeRemoveQue()
        {
            if (safeRemoveQue.Count > 0)
            {
                RemoveComponents<IObjectComponent>(safeRemoveQue);
                safeRemoveQue.Clear();
            }
        }

        public void FlushQues()
        {
            FlushSafeAddQue();
            FlushSafeRemoveQue();
        }


        #region Safe add methods
        public void SafelyAddComponent<T>(T objectComponent) where T : IObjectComponent
        {
            safeAddQue.Add(objectComponent);
        }
        public void SafelyAddComponents<T>(IEnumerable<T> objectComponents) where T : IObjectComponent
        {
            foreach (T objectComponent in drawableComponents)
            {
                safeAddQue.Add(objectComponent);
            }
        }
        public void SafelyAddComponents<T>(params T[] objectComponents) where T : IObjectComponent
        {
            SafelyAddComponents<T>(objectComponents.ToList());
        }
        #endregion

        #region Safe remove methods
        public void SafelyRemoveComponent<T>(T objectComponent) where T : IObjectComponent
        {
            safeRemoveQue.Add(objectComponent);
        }
        public void SafelyRemoveComponents<T>(IEnumerable<T> objectComponents) where T : IObjectComponent
        {
            foreach (T objectComponent in drawableComponents)
            {
                safeRemoveQue.Add(objectComponent);
            }
        }
        public void SafelyRemoveComponents<T>(params T[] objectComponents) where T : IObjectComponent
        {
            SafelyRemoveComponents<T>(objectComponents.ToList());
        }
        #endregion

        #region Add methods
        /// <summary>
        /// Lisää annetun komponentin listoihin.
        /// </summary>
        public void AddComponent<T>(T objectComponent) where T : IObjectComponent
        {
            AddToLists(objectComponent);
        }
        /// <summary>
        /// Lisää annetut komponentit listoihin.
        /// </summary>
        public void AddComponents<T>(IEnumerable<T> objectComponents) where T : IObjectComponent
        {
            foreach (T component in objectComponents)
            {
                AddToLists(component);
            }
        }
        /// <summary>
        /// Lisää annetut komponentit listoihin.
        /// </summary>
        public void AddComponents<T>(params T[] objectComponents) where T : IObjectComponent
        {
            AddComponents<T>(objectComponents.ToList());
        }
        #endregion

        #region Remove methods
        /// <summary>
        /// Poistaa annetun komponentin listoista.
        /// </summary>
        public void RemoveComponent<T>(T objectComponent) where T : IObjectComponent
        {
            RemoveFromLists(objectComponent);
        }
        /// <summary>
        /// Poistaa annetut komponentit listoista.
        /// </summary>
        public void RemoveComponents<T>(IEnumerable<T> objectComponents) where T : IObjectComponent
        {
            foreach (T component in objectComponents)
            {
                RemoveFromLists(component);
            }
        }
        /// <summary>
        /// Poistaa annetut komponentit listoista.
        /// </summary>
        public void RemoveComponents<T>(params T[] objectComponents) where T : IObjectComponent
        {
            RemoveComponents<T>(objectComponents.ToList());
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
                foreach (IObjectComponent component in list)
                {
                    T result = component as T;
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                foreach (IObjectComponent component in list)
                {
                    T result = component as T;
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
        #endregion

        /// <summary>
        /// Palauttaa iterointia varten kokoelman halutun tyyppisistä komponenteista.
        /// Jos halutaan iteroida kaikki komponentit läpi, käytetään pohja rajapintaa (IObjectComponent)
        /// tyyppi argumenttina.
        /// </summary>
        public IEnumerable<T> ComponentsOfType<T>(Predicate<T> predicate = null) where T : IObjectComponent
        {
            FlushQues();

            List<T> list = GetComponentList(typeof(T)) as List<T>;

            if (predicate == null)
            {
                foreach (T component in list)
                {
                    yield return component;
                }
            }
            else
            {
                foreach (T component in list.Where(c => predicate(c)))
                {
                    yield return component;
                }
            } 
        }
    }
}
