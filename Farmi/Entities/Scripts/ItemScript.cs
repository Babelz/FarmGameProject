using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Farmi.Entities.Items;

namespace Farmi.Entities.Scripts
{
    public abstract class ItemScript : IScript
    {
        #region Vars
        protected readonly KhvGame game;
        protected readonly ConsumableItem owner;
        #endregion

        public ItemScript(KhvGame game, ConsumableItem owner)
        {
            this.game = game;
            this.owner = owner;
        }

        #region Abstract members
        public abstract void Update(GameTime gameTime);
        #endregion

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
