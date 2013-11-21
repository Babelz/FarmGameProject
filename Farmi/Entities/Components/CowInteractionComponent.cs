using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Farmi.Entities.Animals;

namespace Farmi.Entities.Components
{
    public sealed class CowInteractionComponent : AnimalInteractionComponent
    {
        #region Vars
        #endregion

        public CowInteractionComponent(Animal owner, string[] acceptedTools)
            : base(owner, acceptedTools)
        {
        }
        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public override bool CanInteract(GameObject with)
        {
            throw new NotImplementedException();
        }
    }
}
