using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    abstract class BasicInteractionComponent : IInteractionComponent
    {
        #region Vars
        private bool wasInteracting;
        private GameObject interactWith;
        private bool isInteracting;
        #endregion

        #region Properties
        public virtual bool IsInteracting
        {
            get
            {
                return isInteracting;
            }
            protected set
            {
                if (isInteracting && value == false)
                {
                    if (OnInteractionFinished != null)
                    {
                        OnInteractionFinished(interactWith);
                    }
                }
                isInteracting = value;
            }
        }

        protected GameObject InteractWith
        {
            get { return interactWith;  }
        }
        #endregion

        #region Events
        public event InteractionDelegate OnInteractionBegin;
        public event InteractionDelegate OnInteraction;
        public event InteractionDelegate OnInteractionFinished;
        #endregion

        public virtual void Update(GameTime gametime)
        {

            if (IsInteracting)
            {
                if (OnInteraction != null)
                {
                    OnInteraction(interactWith);
                }
                DoInteract(interactWith, gametime);
            }
            
        }

        public virtual void Interact(GameObject with)
        {
            if (!CanInteract(with))
            {
                return;
            }
            
            if (!IsInteracting)
            {
                interactWith = with;
                IsInteracting = true;
                if (OnInteractionBegin != null)
                {
                    OnInteractionBegin(with);
                }
            }
        }

        #region Abstract members
        protected abstract void DoInteract(GameObject with, GameTime gameTime);
        public abstract bool CanInteract(GameObject with);
        #endregion
    }
}
