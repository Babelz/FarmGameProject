using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    public sealed class SeedInteractionComponent : InteractionComponent
    {
        #region Vars
        private readonly Seed owner;
        #endregion

        public SeedInteractionComponent(Seed owner)
        {
            this.owner = owner;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            CropSpot spot = with as CropSpot;
            if (spot == null)
                return;
            // onko spotilla maaperää tai kasvaako siinä joku
            if (spot.Ground == null || spot.Ground.IsOccupied)
            {
                IsInteracting = false;
                return;
            }
            // ei kasva joten istutetaan
            spot.Ground.Plant(owner);

            IsInteracting = false;
        }

        public override bool CanInteract(GameObject with)
        {
            return with is CropSpot;
        }
    }
}
