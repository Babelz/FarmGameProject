using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Transition;
using Khv.Engine.State;

namespace Khv.Engine
{
    /// <summary>
    /// Luokka joka huolehtii peliruutujen päivittämisestä, piirtämisestä, vaihtelemisesta 
    /// ja ruutujen välisten häivytyksien näyttämisestä.
    /// Perii DrawableGameComponentin joten se tulee lisätä Game.Components listaan
    /// </summary>
    public class GameStateManager : DrawableGameComponent
    {

        private SpriteBatch spriteBatch;
        private GameStateMachine stateMachine;

        public GameState Current
        {
            get
            {
                return stateMachine.Current;
            }
        }

        public GameStateManager(KhvGame game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            stateMachine = new GameStateMachine(this, game);
        }

        public override void Update(GameTime gameTime)
        {
            stateMachine.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            stateMachine.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public void ChangeState(GameState state)
        {
            stateMachine.ChangeState(state);
        }

        public void PopState()
        {
            stateMachine.PopState();
        }

        public void PushState(GameState state)
        {
            stateMachine.PushState(state);
        }

        /// <summary>
        /// Helpperi funktio piirtämään läpinäkyvä tausta
        /// </summary>
        /// <param name="alpha">Arvo väliltä 0.0 - 1.0f</param>
        public void DrawBackbuffer(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Khv.Engine.KhvGame.Temp, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            spriteBatch.End();
        }

        /// <summary>
        /// Lähtee helevettiin koko pelistä
        /// </summary>
        public void Exit()
        {
            Game.Exit();
        }
    }

}

