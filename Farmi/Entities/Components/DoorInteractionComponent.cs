using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Farmi.Entities.Components;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    internal class DoorInteractionComponent : BasicInteractionComponent
    {
        
        protected override void DoInteract(GameObject source)
        {
            //throw new NotImplementedException();
            IsInteracting = false;
        }

        public override bool CanInteract(GameObject source)
        {
            return source as FarmPlayer != null;
        }
    }
}
