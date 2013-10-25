using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapComponents.Components;
using Khv.Engine;

namespace Khv.Maps.MapClasses.MapComponents
{
    /// <summary>
    /// abstrakti luokka jotka kartta komponentit perivät
    /// </summary>
    public abstract class MapComponent
    {
        #region Vars
        protected readonly KhvGame game;
        protected readonly TileMap map;
        protected readonly ILayer target;
        #endregion

        #region Properties
        public bool Enabled
        {
            get;
            set;
        }
        #endregion

        public MapComponent(KhvGame game, TileMap map, ILayer target)
        {
            this.game = game;
            this.map = map;
            this.target = target;
        }

        /// <summary>
        /// Metodi jossa tulee alustaa komponentin toiminnallisuus
        /// ja parsia tarvittavat tiedot MapComponentData oliosta.
        /// </summary>
        public abstract void Initialize(MapComponentData data);
        
        /// <summary>
        /// Päivittää komponenttia, kutsutaan Update metodissa.
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void UpdateComponent(GameTime gameTime);


        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                UpdateComponent(gameTime);
            }
        }
    }
}
