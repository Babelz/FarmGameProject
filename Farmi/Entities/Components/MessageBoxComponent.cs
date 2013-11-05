using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Khv.Gui.Components;

namespace Farmi.Entities.Components
{
    /// <summary>
    /// TODO: tosi varhainen implementaatio (proto)
    /// </summary>
    internal class MessageBoxComponent : IDrawableObjectComponent
    {
        #region Vars
        private ExclamationMarkDrawer exclamationMarkDrawer;
        private FarmPlayer player;
        private SpriteFont font;

        private int elapsed;

        private string currentMessage;
        private int timeToDraw;
        #endregion

        #region Properties
        public bool IsDrawingMessage
        {
            get
            {
                return currentMessage.Length > 0;
            }
        }
        public int DrawOrder
        {
            get;
            set;
        }
        #endregion

        public MessageBoxComponent(KhvGame game, FarmPlayer player)
        {
            this.player = player;

            DrawOrder = 1;
            font = game.Content.Load<SpriteFont>("arial");

            currentMessage = string.Empty;

            exclamationMarkDrawer = player.Components.GetComponent(
                c => c is ExclamationMarkDrawer) as ExclamationMarkDrawer;
        }

        public void DrawMessage(string message, int timeInMillis)
        {
            currentMessage = message;
            timeToDraw = timeInMillis;

            exclamationMarkDrawer.StopDrawing();
        }
        public void StopDraw()
        {
            currentMessage = string.Empty;
            timeToDraw = 0;
            elapsed = 0;

            exclamationMarkDrawer.ResumeDrawing();
        }

        public void Update(GameTime gametime)
        {
            if (IsDrawingMessage)
            {
                elapsed += gametime.ElapsedGameTime.Milliseconds;

                if (elapsed > timeToDraw)
                {
                    StopDraw();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDrawingMessage)
            {
                Vector2 size = font.MeasureString(currentMessage);
                Vector2 position = new Vector2(player.Position.X - size.X / 2,
                                                   player.Position.Y - size.Y);

                spriteBatch.DrawString(font, currentMessage, position, Color.White);
            }
        }
    }
}
