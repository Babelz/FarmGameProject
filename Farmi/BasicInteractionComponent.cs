using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi
{
    abstract class BasicInteractionComponent : IInteractionComponent
    {
        private bool wasInteracting;
        private GameObject interactWith;

        private bool isInteracting;

        public virtual void Update(GameTime gametime)
        {

            if (IsInteracting)
            {
                if (OnInteraction != null)
                    OnInteraction();
                DoInteract(interactWith);
            }
            
        }

        public event InteractionDelegate OnInteractionBegin;
        public event InteractionDelegate OnInteraction;
        public event InteractionDelegate OnInteractionFinished;


        public virtual bool IsInteracting
        {
            get { return isInteracting;  }
            protected set
            {
                if (isInteracting && value == false)
                {
                    if (OnInteractionFinished != null)
                        OnInteractionFinished();
                }
                isInteracting = value;
            }
        }

        public virtual void Interact(GameObject source)
        {
            if (!CanInteract(source))
                return;
            
            if (!IsInteracting)
            {
                interactWith = source;
                IsInteracting = true;
                if (OnInteractionBegin != null)
                    OnInteractionBegin();
                DoInteract(source);
            }
        }

        protected abstract void DoInteract(GameObject source);

        public abstract bool CanInteract(GameObject source);

    }
}
