using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Screens;
using Khv.Engine;
using Khv.Game.GameObjects;
using Khv.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmi.Entities.Components
{
    public class HoeInteractionComponent : InteractionComponent
    {
        private readonly KhvGame game;
        private FarmPlayer player;

        public HoeInteractionComponent(KhvGame game)
        {
            this.game = game;
            var gameplay = game.GameStateManager.States.FirstOrDefault(c => c is GameplayScreen) as GameplayScreen;
            player = gameplay.World.Player;
            OnInteractionBegin += HoeInteractionComponent_OnInteractionBegin;
            OnInteractionFinished += HoeInteractionComponent_OnInteractionFinished;
        }

        void HoeInteractionComponent_OnInteractionFinished(object sender, InteractionEventArgs e)
        {
            player.Animator.AnimationEnded -= Animator_AnimationEnded;
        }

        void HoeInteractionComponent_OnInteractionBegin(object sender, InteractionEventArgs e)
        {
            player.Animator.AnimationEnded += Animator_AnimationEnded;
        }

        void Animator_AnimationEnded(BrashMonkeySpriter.Spriter.Animation sender)
        {
            IsInteracting = false;
            // with = cropspot
            CropSpot spot = InteractWith as CropSpot;
            // siinä on jo maaperä
            if (spot.Ground != null)
            {
                IsInteracting = false;
                return;
            }

            spot.SetGround(new Ground());
            IsInteracting = false;
        }

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {

        }

        public override bool CanInteract(GameObject with)
        {
            return with is CropSpot;
        }
    }
}
