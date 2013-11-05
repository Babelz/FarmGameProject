using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    internal sealed class DispenserInformerComponent : InteractionComponent
    {
        #region Vars
        private readonly AnimalFeedDispenser owner;
        #endregion

        public DispenserInformerComponent(AnimalFeedDispenser owner)
        {
            this.owner = owner;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            FarmPlayer player = with as FarmPlayer;

            MessageBoxComponent messageComponent = player.Components.GetComponent(
                c => c is MessageBoxComponent) as MessageBoxComponent;

            messageComponent.DrawMessage(string.Format("Safkaa viela siilossa {0} yksikkoa.", owner.FeedContained), 100 * 10);

            IsInteracting = false;
        }
        public override bool CanInteract(GameObject with)
        {
            return with is FarmPlayer;
        }
    }
}
