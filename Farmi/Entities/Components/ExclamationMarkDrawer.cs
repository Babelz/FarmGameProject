using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Particles;
using System.IO;

namespace Farmi.Entities.Components
{
    internal sealed class ExclamationMarkDrawer : IDrawableObjectComponent
    {
        #region Vars
        private readonly FarmPlayer farmPlayer;
        private Texture2D texture;
        private Size size;
        private Rectangle rectangle;

        private bool isDrawing;
        #endregion

        public ExclamationMarkDrawer(KhvGame game, FarmPlayer farmPlayer)
        {
            this.farmPlayer = farmPlayer;

            texture = game.Content.Load<Texture2D>(Path.Combine("Entities", "exclamation"));
            size = new Size(texture.Width, texture.Height);
        }

        private void InitializeRect()
        {
            rectangle = new Rectangle((int)farmPlayer.Position.X - (farmPlayer.Size.Width - size.Width / 2),
                                      (int)farmPlayer.Position.Y - size.Height, size.Width, size.Height);
        }

        public void Update(GameTime gametime)
        {
            isDrawing = farmPlayer.CouldInteract;

            // Vois laskee animaatiota ja efektejä täällä.
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDrawing)
            {
                InitializeRect();
                spriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
    }
}
