using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi
{
    class BasicInteractionComponent : IInteractionComponent
    {
        public void Update(GameTime gametime)
        {
            
        }

        public event InteractionDelegate OnInteractionBegin;
        public event InteractionDelegate OnInteraction;
        public event InteractionDelegate OnInteractionFinished;
        public void Interact(GameObject source)
        {
            throw new NotImplementedException();
        }

        public bool CanInteract(GameObject source)
        {
            throw new NotImplementedException();
        }
    }
}
