using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    public interface IInteractionComponent : IObjectComponent
    {
        #region Events
        event InterActionEventHandler OnInteractionBegin;
        event InterActionEventHandler OnInteraction;
        event InterActionEventHandler OnInteractionFinished;
        #endregion

        #region Properties
        bool IsInteracting { get; }
        #endregion

        void Interact(GameObject with);
        bool CanInteract(GameObject with);

    }

    public delegate void InterActionEventHandler(object sender, InteractionEventArgs e);

    public class InteractionEventArgs
    {
        #region Properties
        public GameObject Interactor
        {
            get;
            set;
        }
        #endregion
    }
}
