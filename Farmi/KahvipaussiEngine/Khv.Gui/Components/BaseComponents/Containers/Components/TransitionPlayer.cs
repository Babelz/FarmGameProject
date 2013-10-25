using System;
using Khv.Engine.Transition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Args;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    /// <summary>
    /// Olio joka hoitaa siirtymien toistamisen.
    /// </summary>
    public class TransitionPlayer
    {
        #region Vars
        private TransitionEffect outTransition;
        private TransitionEffect inTransition;
        private TransitionEffect currentTransition;
        private bool isStarted;
        private bool isFinished;
        private bool isOutInvoked;
        private bool isInInvoked;
        #endregion

        #region Events
        public event TransitionEventHandler OnInTransitionFinished;
        public event TransitionEventHandler OnOutTransitionFinished;
        public event TransitionEventHandler OnFinished;
        #endregion

        #region Properties
        public bool IsPlaying
        {
            get
            {
                if (currentTransition == null)
                {
                    return false;
                }
                else
                {
                    return currentTransition.IsStarted;
                }
            }
        }
        public bool IsStarted
        {
            get
            {
                return isStarted;
            }
        }
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
        }
        #endregion

        /// <summary>
        /// Alustaa uuden siirtymä toistajan. Jompikumpi valueista voi olla null, mutta ei volemmat.
        /// </summary>
        public TransitionPlayer(TransitionEffect transitInEffect = null, TransitionEffect transitOutEffect = null)
        {
            this.outTransition = transitInEffect;
            this.inTransition = transitOutEffect;

            if (transitInEffect == null && transitOutEffect == null)
            {
                throw new ArgumentNullException("Both transitions cant be null.");
            }
        }

        // Tekee eventille null checkin ja laukaisee sen.
        private void LaunchOutFinished()
        {
            if (OnOutTransitionFinished != null && !isOutInvoked)
            {
                OnOutTransitionFinished(this, new GameEventArgs());
                isOutInvoked = true;
            }
        }
        // Tekee eventille null checkin ja laukaisee sen.
        private void LaunchInFinished()
        {
            if (OnInTransitionFinished != null && !isInInvoked)
            {
                OnInTransitionFinished(this, new GameEventArgs());
                isInInvoked = true;
            }
        }
        // Vaihtaa transitionin ja päivittää isFinishedin statea.
        private void ChangeTransition()
        {
            if (!isFinished)
            {
                // Otetaan tässä lohkossa eka mahdollinen transitioni.
                // Ottaa outTransitionin jossei se ole null ja vise versa.
                if (currentTransition == null)
                {
                    if (outTransition != null)
                    {
                        currentTransition = outTransition;
                    }
                    else
                    {
                        // Ammutaan out transitionin finished event suoraan.
                        LaunchOutFinished();

                        currentTransition = inTransition;
                    }
                }
                else
                {
                    if (currentTransition == outTransition)
                    {
                        LaunchOutFinished();

                        if (inTransition != null)
                        {
                            currentTransition = inTransition;
                        }
                        else
                        {
                            LaunchInFinished();
                        }
                    }
                    else
                    {
                        LaunchInFinished();
                        currentTransition = null;
                    }
                }

                // Alotetaan uusi transitoni jossei olla toistettu molempia.
                if (currentTransition != null && !currentTransition.IsStarted)
                {
                    currentTransition.Start();
                }
                else
                {
                    isFinished = true;

                    if (OnFinished != null)
                    {
                        OnFinished(this, new GameEventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Tulee kutsua kun halutaan aloittaa siirtymien toistaminen.
        /// </summary>
        public void Start()
        {
            if (!isStarted)
            {
                ChangeTransition();
                isStarted = true;
            }
        }
        /// <summary>
        /// Toistaa tämän hetkistä transitionia. Vaihtaa transitionin jos 
        /// tämän hetkinen on toistettu loppuun.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (isStarted)
            {
                if (currentTransition.IsFinished)
                {
                    ChangeTransition();
                }
                else
                {
                    currentTransition.Update(gameTime);
                }
            }
        }
        /// <summary>
        /// Pirtää tämän hetkisen siirtymän vaihee.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isFinished)
            {
                currentTransition.PostRender(spriteBatch);
            }
        }

        public delegate void TransitionEventHandler(object sender, GameEventArgs e);
    }
}
