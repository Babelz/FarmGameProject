using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.State;
using Khv.Engine.Args;

namespace Khv.Engine.Transition
{
    /// <summary>
    /// Kuvastaa efekti‰ jonka voi pist‰‰ GUI:n tai peliruutujen v‰lille
    /// </summary>
    public abstract class TransitionEffect
    {
        #region Vars

        protected Color color;
		protected TimeSpan time;
        protected bool isFinished;

        #endregion

        #region Events

        /// <summary>
        /// Laukeaa kun transition alkaa
        /// </summary>
        public event TransitionStatusEventHandler OnTransitionStart;
        /// <summary>
        /// Laukeaa kun transition on loppunut
        /// </summary>
        public event TransitionStatusEventHandler OnTransitionFinish;

        #endregion

        #region Properties

        /// <summary>
        /// Onko transition loppunut
        /// </summary>
        public virtual bool IsFinished
        {
            get { return isFinished; }
        }

        /// <summary>
        /// Onko transition alkanut
        /// </summary>
        public bool IsStarted
        {
            get;
            protected set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Luo uuden transitionin mustalla v‰rill‰ ja 1.5 sekunnin kestolla
        /// </summary>
        public TransitionEffect() : this(Color.Black, TimeSpan.FromSeconds(1.5))
        {
        }

        /// <summary>
        /// Luo uuden transitionin valitulla v‰rill‰ ja 1.5 sekunnin kestolla
        /// </summary>
        /// <param name="color">Mink‰ v‰rinen</param>
		public TransitionEffect(Color color) : this(color, TimeSpan.FromSeconds(1.5))
		{

		}

        /// <summary>
        /// Luo uuden transitionin valitulla v‰rill‰ ja ajalla
        /// </summary>
        /// <param name="color">V‰ri</param>
        /// <param name="delay">Kesto</param>
		public TransitionEffect(Color color, TimeSpan delay)
		{
			this.color = color;
			this.time = delay;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Kun halutaan aloittaa transition
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                IsStarted = true;
                if (OnTransitionStart != null)
                {
                    OnTransitionStart(this, new GameEventArgs());
                }
            }
        }

        /// <summary>
        /// P‰ivitt‰‰ transitionia
        /// </summary>
        /// <param name="time"></param>
        public virtual void Update(GameTime time)
        {
            if (IsStarted && IsFinished)
            {
                if (OnTransitionFinish != null)
                    OnTransitionFinish(this, new GameEventArgs());
                Reset();
            }
        }

 
        /// <summary>
        /// Asettaa aloitetun falseksi
        /// </summary>
        public virtual void Reset()
        {
            IsStarted = false;
        }
	
	
        /// <summary>
        /// Kutsutaan ennen ruudun piirtoa
        /// </summary>
        /// <param name="batch"></param>
		public virtual void PreRender(SpriteBatch batch)
		{

		}

        /// <summary>
        /// Kutsutaan ruudun piirron j‰lkeen
        /// </summary>
        /// <param name="batch"></param>
		public virtual void PostRender(SpriteBatch batch)
		{

		}

        /// <summary>
        /// Alustaa transitionin, nappaa nykyisest‰ vaikutus alueen
        /// </summary>
        /// <param name="current">Nykyinen</param>
        /// <param name="next">Seuraava</param>
		public virtual void Init(ITransition current, ITransition next) 
        {
            Area = current.Area;
        }

        public Rectangle Area
        {
            get;
            protected set;
        }
        #endregion
    }

    public delegate void TransitionStatusEventHandler(object sender, GameEventArgs e);
}

