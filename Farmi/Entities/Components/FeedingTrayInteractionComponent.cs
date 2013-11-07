using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Farmi.Entities.Items;
using Khv.Engine;

namespace Farmi.Entities.Components
{
    internal sealed class FeedingTrayInteractionComponent : InteractionComponent
    {
        #region Vars
        private readonly FeedingTray owner;
        #endregion

        public FeedingTrayInteractionComponent(FeedingTray owner)
        {
            this.owner = owner;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            FarmPlayer player = with as FarmPlayer;
            owner.InsertFeed(player.Inventory.ThrowItem() as AnimalFeedItem);

            IsInteracting = false;
        }
        public override bool CanInteract(GameObject with)
        {
            if (owner.ContainsFeed)
            {
                return false;
            }

            FarmPlayer player = with as FarmPlayer;

            if (player == null)
            {
                return false;
            }

            if (player.Inventory.HasItemInHands)
            {
                AnimalFeedItem feedItem = player.Inventory.ItemInHands as AnimalFeedItem;

                if (feedItem != null)
                {
                    if (feedItem.FeedType == owner.FeedType)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
