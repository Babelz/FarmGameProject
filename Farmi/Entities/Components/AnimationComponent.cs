using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Components
{
    /// <summary>
    /// Hoitaa animointilogiikan perus liikkumiseen
    /// </summary>
    class AnimationComponent : IDrawableObjectComponent
    {
        private readonly FarmPlayer player;
        private readonly CharaterAnimator animator;

        public AnimationComponent(FarmPlayer player, CharaterAnimator animator, string animation)
        {
            this.player = player;
            this.animator = animator;
            animator.ChangeAnimation(animation);
            DrawOrder = 1;
        }

        public void Update(GameTime gametime)
        {
            /*
            if (player.Velocity == Vector2.Zero && animator.CurrentAnimation.Name != "idle")
            {
                animator.ChangeAnimation("idle");
            }
             */
            animator.Update(gametime);
        }

        public int DrawOrder { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            animator.Draw(spriteBatch);
        }
    }
}
