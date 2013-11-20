using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Game;
using Khv.Maps.MapClasses.Managers;
using Khv.Engine.Args;

namespace Farmi.Entities.Components
{
    /// <summary>
    /// Komponentti joka hoitaa objektin poistamisen worldin listasta jos se ei ole oikealla kartalla.
    /// </summary>
    public sealed class MapLocator : IObjectComponent
    {
        #region Vars
        private readonly GameObject owner;
        private readonly World world;
        #endregion

        #region Events
        /// <summary>
        /// Eventti joka laukaistaan kun kartta jossa omistaja asuu tulee aktiiviseksi.
        /// </summary>
        public event MapLocatorEventHandler ContainingMapActive;
        /// <summary>
        /// Eventti joka laukaistaan kun kartta jossa omistaja ei asu tulee aktiiviseksi.
        /// </summary>
        public event MapLocatorEventHandler ContainingMapChanged;
        #endregion

        #region Properties
        public string MapContainedIn
        {
            get;
            set;
        }
        #endregion

        public MapLocator(World world, GameObject owner, string mapContainedIn)
        {
            this.world = world;
            this.owner = owner;

            MapContainedIn = mapContainedIn;

            world.MapManager.OnMapChanged += new MapEventHandler(MapManager_OnMapChanged);
            owner.OnDestroyed += new GameObjectEventHandler(owner_OnDestroyed);
        }

        #region Event handlers
        private void owner_OnDestroyed(object sender, GameEventArgs e)
        {
            world.MapManager.OnMapChanged -= MapManager_OnMapChanged;
            owner.OnDestroyed -= owner_OnDestroyed;
        }
        private void MapManager_OnMapChanged(object sender, MapEventArgs e)
        {
            if (e.Current.Name == MapContainedIn)
            {
                if (ContainingMapActive != null)
                {
                    ContainingMapActive(this, new MapLocatorEventArgs(e, owner));
                }
            }
            else
            {
                if (ContainingMapChanged != null)
                {
                    ContainingMapChanged(this, new MapLocatorEventArgs(e, owner));
                }
            }
        }
        #endregion
    }

    public delegate void MapLocatorEventHandler(object sender, MapLocatorEventArgs e);

    public class MapLocatorEventArgs : MapEventArgs
    {
        #region Properties
        public GameObject Owner
        {
            get;
            private set;
        }
        #endregion

        public MapLocatorEventArgs(MapEventArgs e, GameObject owner)
            : base(e.Current, e.Next)
        {
            Owner = owner;
        }
    }
}
