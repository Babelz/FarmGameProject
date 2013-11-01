using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Game
{
    /// <summary>
    /// Kuvastaa abstraktia pelimaailmaa
    /// jossa asuu karttamanageri ja peliobjekti säiliö
    /// </summary>
    public abstract class World : IGameComponent
    {
        #region Properties

        /// <summary>
        /// Palauttaa nykyisen karttamanagerin
        /// </summary>
        public MapManager MapManager
        {
            get;
            protected set;
        }

        /// <summary>
        /// Palauttaa nykyisen globaalin 
        /// </summary>
        public GameObjectManager WorldObjects
        {
            get;
            protected set;
        }

        protected KhvGame Game { get; private set; }

        #endregion

        #region Ctor

        protected World(KhvGame game)
        {
            Game = game;
        }

        #endregion

        #region Abstract members

        /// <summary>
        /// Kutsutaan kun peli alustetaan, ei ole järkeä ladata contenttia täällä
        /// </summary>
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
