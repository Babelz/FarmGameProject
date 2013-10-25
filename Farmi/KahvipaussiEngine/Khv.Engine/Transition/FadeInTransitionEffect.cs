using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Engine.Transition
{
    public class FadeInTransitionEffect : TransitionEffect, IStandAloneTransition
    {
		protected float alpha = 1f;

        /// <summary>
        /// Luo uuden siirtym‰n mustalla v‰rill‰ ja 0.5 sekunnin kestolla
        /// </summary>
		public FadeInTransitionEffect() : base(Color.Black, TimeSpan.FromSeconds(0.5)) 
		{
		}
        /// <summary>
        /// Luo uuden transition valitulla v‰rill‰ ja 0.5 sekunnin kestolla
        /// </summary>
        /// <param name="color">V‰ri</param>
		public FadeInTransitionEffect(Color color) : base(color, TimeSpan.FromSeconds(0.5)) 
		{
		}
		
		public FadeInTransitionEffect(Color color, TimeSpan time) : base(color, time) 
		{
		}


        public override void Update(GameTime time)
        {

			alpha -=  (float)(time.ElapsedGameTime.TotalMilliseconds * (1.0f / this.time.TotalMilliseconds));
			if (alpha < 0)
				alpha = 0;

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
                return alpha <= 0f;
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

