using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Farmi.Entities.Components;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    internal class DoorInteractionComponent : BasicInteractionComponent
    {
        #region Vars
        private readonly Door owner;
        #endregion

        public DoorInteractionComponent(Door owner)
        {
            this.owner = owner;
        }
        protected override void DoInteract(GameObject source, GameTime gameTime)
        {
            owner.Teleport.Port();

            IsInteracting = false;
        }
        public override bool CanInteract(GameObject source)
        {
            return source as FarmPlayer != null;
        }
    }
}
