using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.State;
using Microsoft.Xna.Framework;
using Farmi.Entities;
using Khv.Engine;
using Khv.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components.BaseComponents.Containers.Components;
using Farmi.Calendar;
using Farmi.Entities.Components;

namespace Farmi.Screens
{
    internal sealed class GameplayScreen : GameState
    {
        #region Vars
        private Camera camera;

        private SpriteFont font;
        private DrawingFiniteStateMachine drawingFiniteStateMachine;
        #endregion

        #region Properties
        public FarmWorld World
        {
            get;
            private set;
        }
        public bool IsPlayingStateMachine
        {
            get
            {
                return drawingFiniteStateMachine != null;
            }
        }
        public Camera Camera
        {
            get
            {
                return camera;
            }
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            World = new FarmWorld(Game);
            World.Initialize();

            camera = new Camera(Vector2.Zero, Game.GraphicsDevice.Viewport);
            FarmPlayer player = World.WorldObjects.GetGameObject<FarmPlayer>(p => p is FarmPlayer);
            camera.Follow(player);
        }

        public void StartPlayingState(DrawingFiniteStateMachine drawingFiniteStateMachine)
        {
            this.drawingFiniteStateMachine = drawingFiniteStateMachine;
        }

        public override void LoadContent()
        {
            font = Content.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (drawingFiniteStateMachine != null)
            {
                if (drawingFiniteStateMachine.HasFinished)
                {
                    drawingFiniteStateMachine = null;
                }
                else
                {
                    drawingFiniteStateMachine.CurrentState.UpdateAction(gameTime);
                }
            } 

            World.Update(gameTime);

            camera.Update(World.MapManager.ActiveMap);
        }

        public override void Draw()
        {
            string text = (Game.Components.First(
                c => c is CalendarSystem) as CalendarSystem)
                .GetTimeDisplayString();
            string dateText = (Game.Components.First(
                c => c is CalendarSystem) as CalendarSystem)
                .GetDateDisplayString();

            Vector2 textSize = font.MeasureString(text);
            Vector2 dateSize = font.MeasureString(dateText);

            SpriteBatch.GraphicsDevice.Clear(Color.Black);
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                  null,
                  null,
                  null,
                  camera.TransFormation);

            World.Draw(SpriteBatch);
            base.Draw();

            SpriteBatch.DrawString(font, text, new Vector2(camera.Position.X + camera.Viewport.Width / 2 - textSize.X / 2, 
                                                           camera.Position.Y + camera.Viewport.Height - textSize.Y * 2), Color.White);

            SpriteBatch.DrawString(font, dateText, new Vector2(camera.Position.X + camera.Viewport.Width / 2 - dateSize.X / 2,
                                                               camera.Position.Y + camera.Viewport.Height - dateSize.Y * 3), Color.White);

            if (drawingFiniteStateMachine != null)
            {
                if (!drawingFiniteStateMachine.HasFinished)
                {
                    drawingFiniteStateMachine.CurrentState.DrawAction(SpriteBatch);
                }
            }

            SpriteBatch.End();
        }
    }
}
