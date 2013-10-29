using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Farmi.Repositories
{
    /// <summary>
    /// Readonly repo tiettyjä oliota varten jotka
    /// luetaan xml tiedostosta.
    /// </summary>
    internal abstract class Repository<T> : IRepository
    {
        #region Vars
        protected readonly List<T> items;
        #endregion

        #region Properties
        /// <summary>
        /// Repon nimi xml tiedostossa.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }
        #endregion

        public Repository(string name)
        {
            Name = name;

            items = new List<T>();
        }

        #region Abstract memebers
        /// <summary>
        /// Lataa kaikki itemit repositorystä 
        /// jotka kuuluvat tälle repostorylle.
        /// </summary>
        public abstract void Load(XDocument repository);
        #endregion

        /// <summary>
        /// Palauttaa ensimmäisen itemin joka täyttää ehdon.
        /// </summary>
        public T GetItem(Predicate<T> predicate)
        {
            return items.Find(i => predicate(i));
        }
    }
    /// <summary>
    /// Kaikkien repojen perus rajapinta.
    /// </summary>
    internal interface IRepository
    {
        string Name
        {
            get;
        }

        // Lataa tiedostosta itemit. 
        // Tässä metodissa tulisi lisätä ne item listaan
        // ja parsia tiedostossa.
        void Load(XDocument repository);
    }
    /// <summary>
    /// Rajapinta repolle jota voi muokata ajon aikana.
    /// </summary>
    internal interface IModifyableRepository<T> : IRepository
    {
        void AddItem(T item);
        void RemoveItem(T item);
    }
}
