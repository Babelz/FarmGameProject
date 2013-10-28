using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhvGame = Khv.Engine.KhvGame;
using Game = Microsoft.Xna.Framework.Game;
using Khv.Game.GameObjects;

namespace Khv.Game
{
    public abstract class CharacterEntity : DrawableGameObject, IEntity
    {

        #region Vars


        #endregion


        #region Properties

        public MotionEngine MotionEngine
        {
            get;
            protected set;
        }

        #endregion


        protected CharacterEntity(KhvGame game)
            : base(game)
        {
            MotionEngine = new MotionEngine(this);
        }
    }
}
