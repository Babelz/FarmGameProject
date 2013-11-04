using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Farmi.Entities.Items;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;
using Microsoft.Xna.Framework;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Scripts
{
    public abstract class ToolBehaviourScript : IScript
    {
        #region Vars
        protected readonly KhvGame game;
        protected readonly Tool owner;
        #endregion

        public ToolBehaviourScript(KhvGame game, Tool owner)
        {
            this.game = game;
            this.owner = owner;

            owner.OnDestroyed += new GameObjectEventHandler(owner_OnDestroyed);
        }

        /// <summary>
        /// Metodi jota kutsutaan initializessa, tässä
        /// metodissa on tarkoitus hokata tarvittavat eventit.
        /// </summary>
        protected virtual void HookEvents()
        {
        }
        /// <summary>
        /// Metodi jota kutsutaan kun peliobjekti tuhotaan,
        /// tässä metodissa on tarkoitus unhookata eventit.
        /// </summary>
        protected virtual void UnHookEvents()
        {
            owner.OnDestroyed -= owner_OnDestroyed;
        }

        #region Event handlers
        private void owner_OnDestroyed(object sender, Khv.Engine.Args.GameEventArgs e)
        {
            UnHookEvents();
        }
        #endregion

        public virtual void Initialize()
        {
            HookEvents();
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
