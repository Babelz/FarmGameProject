using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    public class SeedInteractionComponent : InteractionComponent
    {
        private readonly DrawableGameObject owner;

        public SeedInteractionComponent(DrawableGameObject owner)
        {
            this.owner = owner;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            CropSpot spot = with as CropSpot;
            if (spot == null)
                return;
            // mahtuuko spottiin kasvia?
            if (spot.IsOccupied)
            {
                IsInteracting = false;
                return;
            }
            // mahtuu kasvi joten pläntätään se
            spot.Plant(owner);

            IsInteracting = false;
        }

        public override bool CanInteract(GameObject with)
        {
            return with is CropSpot;
        }
    }
}
