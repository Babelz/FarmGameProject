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
    class PushableComponent : BasicInteractionComponent
    {
        private readonly GameObject toMove;

        private Vector2 offset = Vector2.Zero;

        private MotionEngine engine;

        public PushableComponent(GameObject toMove)
        {
            this.toMove = toMove;
#warning motion engine pitäs olla komponentti
            engine = new MotionEngine(toMove);
            OnInteractionBegin += PushableComponent_OnInteractionBegin;
            OnInteractionFinished +=PushableComponent_OnInteractionFinished;
        }

        void PushableComponent_OnInteractionBegin(object sender, InteractionEventArgs e)
        {
            e.Interactor.Collider.OnCollision += OnHolderCollides;
            toMove.Collider.OnCollision += OnInteractWithCollides;
            Console.WriteLine("JEESUS");
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

        void PushableComponent_OnInteractionFinished(object sender, InteractionEventArgs e)
        {
            e.Interactor.Collider.OnCollision -= OnHolderCollides;
            toMove.Collider.OnCollision -= OnInteractWithCollides;
        }

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
