using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    public class HoeInteractionComponent : InteractionComponent
    {
        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            // with = cropspot
            CropSpot spot = with as CropSpot;
            // siinä on jo maaperä
            if (spot.Ground != null)
            {
                IsInteracting = false;
                return;
            }

            spot.SetGround(new Ground());
            IsInteracting = false;
        }

        public override bool CanInteract(GameObject with)
        {
            return with is CropSpot;
        }
    }
}
