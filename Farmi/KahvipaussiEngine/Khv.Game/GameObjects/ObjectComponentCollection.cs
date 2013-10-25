using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Game.GameObjects
{
    /// <summary>
    /// Luokka jonka tarkoitus on wrapata kaikki peliolion komponentit
    /// samaan luokkaa. Tukee foreach iterointia ja indexillä noutamista.
    /// Ei tue indexillä asettamista.
    /// </summary>
    public class ObjectComponentCollection
    {
        #region Vars
        private readonly List<IObjectComponent> allComponents;
        private readonly List<IDrawableObjectComponent> drawableComponents;
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return allComponents.Count;
            }
        }
        public IObjectComponent this[int index]
        {
            get
            {
                return allComponents[index];
            }
        }
        #endregion

        public ObjectComponentCollection()
        {
            allComponents = new List<IObjectComponent>();
            drawableComponents = new List<IDrawableObjectComponent>();
        }

        // Poistaa komponentin oikeista listoista.
        private void CheckAndRemove(IObjectComponent objectComponent)
        {
            if (objectComponent != null)
            {
                allComponents.Remove(objectComponent);

                // Jos komponentti on drawable, pitää se poistaa myös drawable listasta.
                IDrawableObjectComponent drawableComponent;
                if ((drawableComponent = objectComponent as IDrawableObjectComponent) != null)
                {
                    drawableComponents.Remove(drawableComponent);
                }
            }
        }
        // Lisää komponentin oikeisiin listoihin.
        private void CheckAndAdd(IObjectComponent objectComponent)
        {
            if (objectComponent != null)
            {
                if (allComponents.Find(c => c.GetType() == objectComponent.GetType()) == null)
                {
                    allComponents.Add(objectComponent);

                    // Jos komponentti on drawable, lisätään se myös drawable listaan.
                    IDrawableObjectComponent drawableComponent;
                    if ((drawableComponent = objectComponent as IDrawableObjectComponent) != null)
                    {
                        drawableComponents.Add(drawableComponent);
                    }
                }
            }
        }
        // Palauttaa iteraattorin halutulle listalle ja halutulla predicaatilla.
        private IEnumerable<T> GetIterator<T>(List<T> collection, Predicate<T> predicate = null) where T : IObjectComponent
        {
            if (predicate == null)
            {
                foreach (T item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (T item in collection.Where(i => predicate(i)))
                {
                    yield return item;
                }
            }
        }

        #region Add methods
        /// <summary>
        /// Lisää uuden komponentin managerille. 
        /// Jos manageri omistaa jo tätä tyyppiä olevan komponentin, sitä ei lisätä.
        /// </summary>
        public void Add(IObjectComponent objectComponent)
        {
            CheckAndAdd(objectComponent);
        }
        /// <summary>
        /// Lisää monta komponenttia managerille.
        /// Jos manageri omistaa jonkin näistä komponentti tyypeistä, sitä ei lisätä.
        /// </summary>
        public void AddMany(IEnumerable<IObjectComponent> objectComponents)
        {
            foreach (IObjectComponent objectComponent in objectComponents)
            {
                CheckAndAdd(objectComponent);
            }
        }
        /// <summary>
        /// Lisää monta komponenttia managerille.
        /// Jos manageri omistaa jonkin näistä komponentti tyypeistä, sitä ei lisätä.
        /// </summary>
        /// <param name="objectComponents"></param>
        public void AddMany(params IObjectComponent[] objectComponents)
        {
            Array.ForEach<IObjectComponent>(objectComponents, c => CheckAndAdd(c));
        }
        #endregion

        #region Remove methods
        /// <summary>
        /// Poistaa halutun komponentin managerista.
        /// </summary>
        /// <param name="objectComponent"></param>
        public void Remove(IObjectComponent objectComponent)
        {
            CheckAndRemove(objectComponent);
        }
        /// <summary>
        /// Poistaa komponentin managerista joka
        /// ensimmäisenä täyttää ehdon.
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(Predicate<IObjectComponent> predicate)
        {
            CheckAndRemove(allComponents.Find(c => predicate(c)));
        }
        /// <summary>
        /// Poistaa kaikki komponentit managerista jotka 
        /// täyttävät ehdon.
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveAll(Predicate<IObjectComponent> predicate)
        {
            allComponents.FindAll(c => predicate(c)).ForEach(c => CheckAndRemove(c));
        }
        /// <summary>
        /// Poistaa kaikki komponentit managerista.
        /// </summary>
        public void Clear()
        {
            allComponents.Clear();
            drawableComponents.Clear();
        }
        #endregion

        #region Query methods
        /// <summary>
        /// Palauttaa booleanin siitä omistaako
        /// manageri ehdon täyttävän komponentin.
        /// </summary>
        public bool ContainsComponent(Predicate<IObjectComponent> predicate)
        {
            return allComponents.Find(c => predicate(c)) != null;
        }
        /// <summary>
        /// Palauttaa ensimmäisen komponentin managerista
        /// joka täyttää annetun ehdon.
        /// </summary>
        public IObjectComponent GetComponent(Predicate<IObjectComponent> predicate)
        {
            return allComponents.Find(c => predicate(c));
        }
        /// <summary>
        /// Palauttaa kaikki piirrettävät komponentit managerista.
        /// </summary>
        /// <param name="predicate">Selection ehto.</param>
        public IEnumerable<IDrawableObjectComponent> DrawableComponents(Predicate<IDrawableObjectComponent> predicate = null)
        {
            return GetIterator<IDrawableObjectComponent>(drawableComponents, predicate);
        }
        /// <summary>
        /// Palauttaa kaikki komponentit managerista.
        /// </summary>
        /// <param name="predicate">Selection ehto.</param>
        public IEnumerable<IObjectComponent> AllComponents(Predicate<IObjectComponent> predicate = null)
        {
            return GetIterator<IObjectComponent>(allComponents, predicate);
        }
        #endregion
    }
}
