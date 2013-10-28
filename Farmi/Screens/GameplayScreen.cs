using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.World;
using Khv.Engine.State;
using Microsoft.Xna.Framework;
using Farmi.Entities;
using Khv.Engine;
using Khv.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Screens
{
    internal sealed class GameplayScreen : GameState
    {
        #region Vars
        private Camera camera;

        private SpriteFont font;
        #endregion

        #region Properties
        public FarmWorld World
        {
            get;
            private set;
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            World = new FarmWorld(Game);
            World.Initialize();

            camera = new Camera(Vector2.Zero, Game.GraphicsDevice.Viewport);

            Game.InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("camera move down", Keys.Down), (s, a) =>
                    {
                        camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 1.25f);
                    });
            Game.InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("camera move up", Keys.Up), (s, a) =>
                    {
                        camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 1.25f);
                    });
            Game.InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("camera move right", Keys.Right), (s, a) =>
                    {
                        camera.Position = new Vector2(camera.Position.X + 1.25f, camera.Position.Y);
                    });
            Game.InputManager.Mapper.GetInputBindProvider<KeyInputBindProvider>().Map(
                new KeyTrigger("camera move left", Keys.Left), (s, a) =>
                    {
                        camera.Position = new Vector2(camera.Position.X - 1.25f, camera.Position.Y);
                    });
        }

        public override void LoadContent()
        {
            font = Content.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            World.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                  null,
                  null,
                  null,
                  null,
                  camera.TransFormation);

            World.Draw(SpriteBatch);

            base.Draw();

            SpriteBatch.DrawString(font, "Alpha build 0.1 - SaniEngine v1.0",
                new Vector2(camera.Position.X, camera.Position.Y + 25), Color.White);

            SpriteBatch.End();

            Console.WriteLine(camera.Position);
        }
    }
}
