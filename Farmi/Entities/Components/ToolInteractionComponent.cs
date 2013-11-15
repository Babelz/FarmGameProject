using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    public class ToolInteractionComponent : IInteractionComponent
    {
        #region Events
        public event InterActionEventHandler OnInteractionBegin;
        public event InterActionEventHandler OnInteraction;
        public event InterActionEventHandler OnInteractionFinished;
        #endregion

        #region Properties
        public bool IsInteracting { get; private set; }
        #endregion

        public ToolInteractionComponent(Tool tool)
        {

        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }
        public void Interact(GameObject with)
        {
            throw new NotImplementedException();
        }
        public bool CanInteract(GameObject with)
        {
            throw new NotImplementedException();
        }
    }
}
