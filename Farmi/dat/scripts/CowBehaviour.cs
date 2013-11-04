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
    internal sealed class CowBehaviour : AnimalBehaviourScript
    {
        #region Vars

        #endregion

        public CowBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
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
