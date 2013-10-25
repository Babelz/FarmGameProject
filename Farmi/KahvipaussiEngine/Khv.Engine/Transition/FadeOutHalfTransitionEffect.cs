using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Engine.Transition
{
	public class FadeOutHalfTransitionEffect : TransitionEffect, IStandAloneTransition
	{
		protected float alpha = 0.0f;
		
		public FadeOutHalfTransitionEffect() : base(Color.Black, TimeSpan.FromSeconds(0.5)) 
		{
		}
		
		public FadeOutHalfTransitionEffect(Color color) : base(color, TimeSpan.FromSeconds(0.5)) 
		{
		}
		
		public FadeOutHalfTransitionEffect(Color color, TimeSpan time) : base(color, time) 
		{
		}
		
		public override void Update(GameTime time)
		{
			alpha +=  (float)(time.ElapsedGameTime.TotalMilliseconds * (1.0f / this.time.TotalMilliseconds));
			if (alpha > 0.5f)
				alpha = 0.5f;

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
				return alpha >= 0.5f;
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

