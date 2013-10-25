using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.MapComponents.Layers.Sheets;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// Luokka joka sisältää kaikki kartan layerit.
    /// Jokainen kartta sisältää yhden instanssin tästä luokasta.
    /// </summary>
    public class LayerManager
    {
        #region Vars
        private List<ILayer> layers;
        #endregion

        public LayerManager()
            : base()
        {
            layers = new List<ILayer>();
        }

        /// <summary>
        /// Lisää uuden layerin manageriin.
        /// </summary>
        public void AddLayer(ILayer layer)
        {
            layers.Add(layer);
        }
        /// <summary>
        /// Poistaa layerin joka täyttää annetut ehdot.
        /// </summary>
        public void RemoveLayer(Predicate<ILayer> predicate) 
        {
            layers.Remove(layers.Find(l => predicate(l)));
        }

        /// <summary>
        /// Palauttaa layerin joka täyttää annetut ehdot.
        /// </summary>
        public T GetLayer<T>(Predicate<T> predicate) where T : ILayer
        {
            return (T)layers.Find(l => l is T && predicate((T)l));
        }

        /// <summary>
        /// Palauttaa layerit jotka täyttävät annetut ehdot.
        /// </summary>
        public IEnumerable<T> GetLayers<T>(Predicate<T> predicate) where T : ILayer
        {
            return layers.Where(l => l is T && predicate((T)l)) as IEnumerable<T>;
        }

        /// <summary>
        /// Palauttaa kaikki layerit iterointia varten.
        /// </summary>
        public IEnumerable<ILayer> AllLayers(Predicate<ILayer> predicate = null)
        {
            if (predicate == null)
            {
                foreach (ILayer layer in layers)
                {
                    yield return layer;
                }
            }
            else
            {
                foreach (ILayer layer in layers.Where(l => predicate(l)))
                {
                    yield return layer;
                }
            }
        }
    }
}
