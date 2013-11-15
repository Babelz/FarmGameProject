using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Farmi.Entities.Animals;

namespace Farmi.dat.scripts
{
    public sealed class CowBehaviour : AnimalBehaviourScript
    {
        #region Vars

        #endregion

        public CowBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
            owner.OnDestroyed += new Khv.Game.GameObjects.GameObjectEventHandler(owner_OnDestroyed);
        }

        #region Event handlers
        private void owner_OnDestroyed(object sender, Khv.Engine.Args.GameEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
