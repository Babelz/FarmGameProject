using Khv.Engine.State;
using Khv.Engine.Transition;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Args;

namespace Khv.Engine
{
    public class GameStateMachine : StateMachine<GameState>
    {
        #region Vars
        private readonly GameStateManager manager;
        private readonly KhvGame game;
        #endregion

        #region Properties

        public List<GameState> States
        {
            get { return states.ToList(); }
        }

        #endregion

        public GameStateMachine(GameStateManager manager, KhvGame game)
        {
            this.manager = manager;
            this.game = game;
            states.Add(new EmptyScreen());
        }

        public override void ChangeState(GameState state)
        {
            base.ChangeState(state);
            HookTransitionListener(state);
            current.CoveredByOtherScreen = false;
            next.CoveredByOtherScreen = false;
        }

        public override void PushState(GameState state)
        {
            base.PushState(state);
            HookTransitionListener(state);
            current.CoveredByOtherScreen = next.IsPopUp;
        }

        private void HookTransitionListener(GameState state)
        {
            if (state.LeaveTransition != null)
            {
                TransitionEffect transition = state.LeaveTransition;
                transition.OnTransitionFinish -= OnLeaveTransitionFinish;
                transition.OnTransitionFinish += OnLeaveTransitionFinish;
                
            }
        }

        void OnLeaveTransitionFinish(object sender, GameEventArgs e)
        {
            if (next == null)
            {
                manager.Exit();
            }
        }

        protected override void InitState(GameState state)
        {
            base.InitState(state);
            if (!state.IsInitialized)
            {
                state.Manager = manager;
                state.Game = game;
                state.Initialize();
            }
        }

        public override void PopState()
        {
            base.PopState();
            if (next != null)
            {
                next.CoveredByOtherScreen = false;
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            base.Draw(spritebatch);
            spritebatch.End();
        }
    }
}
