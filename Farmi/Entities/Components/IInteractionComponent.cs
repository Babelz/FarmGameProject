using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    public delegate void InteractionDelegate(GameObject source);
    public interface IInteractionComponent : IObjectComponent
    {
        event InteractionDelegate OnInteractionBegin;
        event InteractionDelegate OnInteraction;
        event InteractionDelegate OnInteractionFinished;

        bool IsInteracting { get; }

        void Interact(GameObject with);
        bool CanInteract(GameObject with);

    }
}
