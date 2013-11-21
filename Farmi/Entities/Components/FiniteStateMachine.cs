using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    public class FiniteStateMachine : IUpdatableObjectComponent
    {
        #region Vars
        public Stack<Action> states;
        #endregion

        #region Properties
        public bool HasStates
        {
            get
            {
                return states.Count > 0;
            }
        }
        public Action CurrentState
        {
            get
            {
                if (HasStates)
                {
                    return states.Peek();
                }

                return null;
            }
        }
        #endregion

        public FiniteStateMachine()
        {
            states = new Stack<Action>();
        }

        public Action PopState()
        {
            if (HasStates)
            {
                return states.Pop();
            }

            return null;
        }
        public void PushState(Action action)
        {
            if (action != CurrentState)
            {
                states.Push(action);
            }
        }
        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState();
            }
        }
    }
}
