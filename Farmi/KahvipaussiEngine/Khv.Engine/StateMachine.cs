using Khv.Engine.State;
using Khv.Engine.Transition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Engine
{
    public class StateMachine<T> where T : IStateElement
    {
        #region Vars

        protected List<T> states;
        protected TransitionEffect enterTransition;
        protected TransitionEffect leaveTransition;

        protected T current;
        protected T next;

        #endregion

        #region Properties
        public T Current
        {
            get
            {
                return current;
            }
        }
        #endregion

        #region Constructor

        public StateMachine()
        {
            states = new List<T>();
        }

        #endregion

        public virtual void ChangeState(T state)
        {
            InitState(state);
            if (states.Count != 0)
            {
                current = states[states.Count - 1];
                next = state;
                leaveTransition = current.LeaveTransition;
                if (leaveTransition == null)
                    leaveTransition = new EmptyTransition();
                leaveTransition.Init(current, next);
                current.IsExiting = true;
            }
            current.State = ScreenState.TransitionOff; // ollaan häivyttämässä
            states.Add(state);
            current.HasFocus = false;
            next.HasFocus = false;
        }

        public virtual void PushState(T state)
        {
            InitState(state);
            if (states.Count != 0)
            {
                leaveTransition = (state.IsPopUp) ? null : states[states.Count - 1].LeaveTransition;
                if (leaveTransition != null)
                    leaveTransition.Init(states[states.Count - 1], state);
                else
                    leaveTransition = new EmptyTransition();
                current = states[states.Count - 1];
                current.State = ScreenState.TransitionOff; // häivytetään
            }
            next = state;
            next.HasFocus = false;
            current.HasFocus = false;
            states.Add(state);

        }

        public virtual void PopState()
        {
            if (states.Count > 0)
            {
                T current = states[states.Count - 1];
                T next = default(T);
                if (states.Count > 1)
                {
                    next = states[states.Count - 2];
                    next.State = ScreenState.Active;
                }
                leaveTransition = current.LeaveTransition;
                if (leaveTransition != null)
                    leaveTransition.Init(current, next);
                else
                    leaveTransition = new EmptyTransition();
                current.IsExiting = true;
                current.State = ScreenState.TransitionOff; // ollaan häivyttämässä
                this.current = current;
                this.next = next;

                current.HasFocus = false;
                // viiminen ruutu
                if (next != null)
                {
                    next.HasFocus = false;
                }
            }
        }

        protected virtual void InitState(T state)
        {
            
        }

        public virtual void Update(GameTime time)
        {
            // jos nykyisellä ruudulla on lopetus siirtymä
            if (leaveTransition != null)
            {
                if (!leaveTransition.IsStarted)
                    leaveTransition.Start();
                leaveTransition.Update(time);
                // jos siirtymä on valmis niin vaihdetaan ruutu seuraavaan 
                // ja aletaan suorittamaan aloitus siirtymää
                if (leaveTransition.IsFinished)
                {
                    leaveTransition = null;
                    // jos on viiminen ruutu, niin nextiä ei sillon ole
                    if (next != null)
                    {
                        if (next.IsPopUp)
                            current.State = ScreenState.Background;
                        else
                            current.State = ScreenState.Hidden; // ei ole enää näkyvissä
                        // vaihdetaan ruutujen paikat
                        T prev = current;
                        current = next;
                        // tyhjennetään nykyisen ruudun viite
                        next = default(T);
                        current.State = ScreenState.TransitionOn; // ollaan häivyttämässä kuvaa sisään
                        if (prev.IsPopUp)
                            enterTransition = new EmptyTransition();
                        else
                            enterTransition = current.EnterTransition;

                        if (enterTransition == null)
                        {
                            enterTransition = new EmptyTransition();
                        }
                        enterTransition.Init(current, prev);
                    }
                }
            }
            for (int i = states.Count - 1; i >= 0; i--)
            {
                T state = states[i];
                state.Update(time);
            }
            if (enterTransition != null)
            {
                if (!enterTransition.IsStarted)
                    enterTransition.Start();
                enterTransition.Update(time);
                if (enterTransition.IsFinished)
                {
                    enterTransition = null;
                    current.State = ScreenState.Active; // nyt ollaan näkyvissä 
                    current.HasFocus = true;


                    for (int i = states.Count - 1; i >= 0; i--)
                    {
                        T state = states[i];
                        if (state.IsExiting)
                            Remove(state);
                    }

                }
            }
        }

        public virtual void Remove(T state)
        {
            if (state.IsInitialized)
                state.Dispose();

            states.Remove(state);
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            foreach (IStateElement state in states)
            {
                
                if (state.State != ScreenState.Hidden)
                {
                    /*
                    * Piirretään juttuja ennen transitionia
                    */
                    state.PreRender();
                    // piirretään early transition
                    if (state.State == ScreenState.TransitionOn || state.State == ScreenState.TransitionOff)
                    {
                        if (leaveTransition != null)
                        {
                            leaveTransition.PreRender(spritebatch);
                        }
                        else if (enterTransition != null)
                        {
                            enterTransition.PreRender(spritebatch);
                        }
                    }
                    // piirretään itse ruutu 
                    state.Draw();
                    // piiretään late transition
                    if (state.State == ScreenState.TransitionOn || state.State == ScreenState.TransitionOff)
                    {
                        if (leaveTransition != null)
                        {
                            leaveTransition.PostRender(spritebatch);
                        }
                        else if (enterTransition != null)
                        {
                            enterTransition.PostRender(spritebatch);
                        }
                    }
                    // piirretään late ruutu
                    state.PostRender();
                }
            }
        }

    }
}
