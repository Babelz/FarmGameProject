using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Farmi.Entities.Items;

namespace Farmi.Entities.Components
{
    internal sealed class FeedDispenserComponent : InteractionComponent
    {
        #region Vars
        private readonly AnimalFeedDispenser owner;
        #endregion

        public FeedDispenserComponent(AnimalFeedDispenser owner)
            : base()
        {
            this.owner = owner;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            if (owner.HasFeed)
            {
                FarmPlayer player = with as FarmPlayer;

                if (!player.Inventory.HasItemInHands)
                {
                    player.Inventory.AddToInventory(owner.GetFeed());
                }
            }
            else
            {
                MessageBoxComponent interactorMessageComponent = with.Components.GetComponent(
                    c => c is MessageBoxComponent) as MessageBoxComponent;

                interactorMessageComponent.DrawMessage("Siilossa ei ole safkaa!", 100 * 25);
            }

            IsInteracting = false;
        }
        public override bool CanInteract(GameObject with)
        {
            return with is FarmPlayer;
        }
    }
}
