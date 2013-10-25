using System;

namespace Khv.Engine.Transition
{
    /// <summary>
    /// Tyhj‰ efekti, nopea k‰ytt‰‰ temppin‰
    /// </summary>
	public class EmptyTransition : TransitionEffect
	{
		public EmptyTransition () : base()
		{
		}

        /// <summary>
        /// Aina finished
        /// </summary>
		public override bool IsFinished {
			get {
				return true;
			}
		}
	}
}

