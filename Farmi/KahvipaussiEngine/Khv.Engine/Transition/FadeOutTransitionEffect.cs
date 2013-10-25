using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Engine.Transition
{
    public class FadeOutTransitionEffect : TransitionEffect, IStandAloneTransition
    {
		protected float alpha = 0.0f;

		public FadeOutTransitionEffect() : base(Color.Black, TimeSpan.FromSeconds(0.5)) 
		{
		}

		public FadeOutTransitionEffect(Color color) : base(color, TimeSpan.FromSeconds(0.5)) 
		{
		}

		public FadeOutTransitionEffect(Color color, TimeSpan time) : base(color, time) 
		{
		}

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
			alpha +=  (float)(time.ElapsedGameTime.TotalMilliseconds * (1.0f / this.time.TotalMilliseconds));
			if (alpha > 1f)
				alpha = 1f;

            base.Update(time);
        }

		public override void PostRender (SpriteBatch batch)
		{
			batch.Draw (Khv.Engine.KhvGame.Temp, Area, color * alpha);
		}
        
        
        public override bool IsFinished
        {
            get
            {
                return alpha >= 1f;
            }
        }

        /// <summary>
        /// Stand alone init, haluaa vaikutusalueen (Rectangle)
        /// </summary>
        /// <param name="args">Rectangle area, mihin piirret‰‰n</param>
        public void Init(params object[] args)
        {
            Area = (Rectangle)args[0];
        }
    }
}

