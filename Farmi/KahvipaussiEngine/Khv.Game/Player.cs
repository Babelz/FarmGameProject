using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhvGame = Khv.Engine.KhvGame;
using Game = Microsoft.Xna.Framework.Game;

namespace Khv.Game
{
    public abstract class Player : CharacterEntity
    {

        #region Events

        // vois ehkä olla joku OnPlayerIndexChange?

        #endregion

        #region Properties

        public PlayerIndex PlayerIndex { get; set; }

        #endregion

        #region Ctor

        protected Player(KhvGame game, PlayerIndex index = PlayerIndex.One)
            : base(game)
        {
            PlayerIndex = index;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Päivittää komponentteja
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Components.ForEach(c => c.Update(gameTime));
        }

        #endregion

    }
}
