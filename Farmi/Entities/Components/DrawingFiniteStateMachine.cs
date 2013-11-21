using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    public sealed class DrawingFiniteStateMachine : IDrawableObjectComponent
    {
        #region Vars
        private readonly Stack<DrawingState> states;
        #endregion

        #region Properties
        public bool HasFinished
        {
            get
            {
                return states.Count == 0;
            }
        }
        public DrawingState CurrentState
        {
            get
            {
                return (HasFinished ? null : states.Peek());
            }
        }
        public int DrawOrder
        {
            get;
            set;
        }
        #endregion

        public DrawingFiniteStateMachine()
        {
            states = new Stack<DrawingState>();

            DrawOrder = 5;
        }

        public void PushState(DrawingState state)
        {
            state.InitializeAction();
            states.Push(state);
        }
        public DrawingState PopState()
        {
            if (!HasFinished)
            {
                return states.Pop();
            }
            else
            {
                return null;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!HasFinished)
            {
                CurrentState.UpdateAction(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!HasFinished)
            {
                CurrentState.DrawAction(spriteBatch);
            }
        }
    }

    public class DrawingState
    {
        #region Properties
        public Action InitializeAction
        {
            get;
            set;
        }
        public Action<GameTime> UpdateAction
        {
            get;
            set;
        }
        public Action<SpriteBatch> DrawAction
        {
            get;
            set;
        }
        #endregion

        public DrawingState()
        {
            // Alustaa tyhjät funkkarit jotta ei tarvitse tehä
            // null check.
            InitializeAction = () =>
                {
                };
            UpdateAction = (gametime) =>
                {
                };
            DrawAction = (spritebatch) =>
                {
                };
        }
    }
}
