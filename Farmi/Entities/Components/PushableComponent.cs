using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Khv.Game;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    class PushableComponent : InteractionComponent
    {
        #region Vars
        private readonly GameObject toMove;
        private Vector2 offset = Vector2.Zero;
        private MotionEngine engine;
        #endregion

        public PushableComponent(GameObject toMove)
        {
            this.toMove = toMove;
            
            engine = new MotionEngine(toMove);
            OnInteractionBegin += PushableComponent_OnInteractionBegin;
            OnInteractionFinished +=PushableComponent_OnInteractionFinished;
        }

        #region Event handlers
        private void PushableComponent_OnInteractionBegin(object sender, InteractionEventArgs e)
        {
            e.Interactor.Collider.OnCollision += OnHolderCollides;
            toMove.Collider.OnCollision += OnInteractWithCollides;
        }
        private void OnHolderCollides(object sender, CollisionEventArgs result)
        {
            if (ReferenceEquals(result.CollidingObject, toMove))
                result.IsCanceled = true;
        }
        private void OnInteractWithCollides(object sender, CollisionEventArgs result)
        {
            if (ReferenceEquals(result.CollidingObject, InteractWith))
                result.IsCanceled = true;
        }
        private void PushableComponent_OnInteractionFinished(object sender, InteractionEventArgs e)
        {
            e.Interactor.Collider.OnCollision -= OnHolderCollides;
            toMove.Collider.OnCollision -= OnInteractWithCollides;
        }
        #endregion

        protected override void DoInteract(GameObject with, GameTime gameTime)
        {
            Vector2 v = with.Velocity;
            v.Normalize();

            engine.GoalVelocity = with.Velocity;
            //toMove.Position += v * 0.1f;
            engine.Update(gameTime);
            //IsInteracting = false;
        }

        public override bool CanInteract(GameObject source)
        {
            return source as FarmPlayer != null;
        }
    }
}
