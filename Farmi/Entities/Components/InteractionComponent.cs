using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    public abstract class InteractionComponent : IInteractionComponent
    {
        #region Vars
        private GameObject interactWith;
        private bool isInteracting;
        #endregion

        #region Events
        public event InterActionEventHandler OnInteractionBegin;
        public event InterActionEventHandler OnInteraction;
        public event InterActionEventHandler OnInteractionFinished;
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
                        OnInteractionFinished(this, new InteractionEventArgs()
                                                    {
                                                        Interactor = interactWith
                                                    });
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

        public virtual void Update(GameTime gametime)
        {

            if (IsInteracting)
            {
                if (OnInteraction != null)
                {
                    OnInteraction(this, new InteractionEventArgs()
                                        {
                                            Interactor = interactWith
                                        });
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
                    OnInteractionBegin(this, new InteractionEventArgs()
                                             {
                                                 Interactor = with
                                             });
                }
            }
        }

        #region Abstract members
        protected abstract void DoInteract(GameObject with, GameTime gameTime);
        public abstract bool CanInteract(GameObject with);
        #endregion
    }
}
