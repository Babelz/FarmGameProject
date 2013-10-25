using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Khv.Maps.MapClasses.Layers.Components;

namespace Khv.Maps.MapClasses.Layers.Sheets.BaseClasses
{
    /// <summary>
    /// sheetti jonka tilet voidaan piirtää
    /// </summary>
    public abstract class DrawableSheet : Sheet
    {
        #region Vars
        protected const int SheetWidth = 320;

        protected TileEngine tileEngine;
        protected Rectangle[,] sources;
        protected Texture2D texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }
        public Rectangle[,] Sources
        {
            get
            {
                return sources;
            }
        }
        #endregion

        public DrawableSheet(string path, ContentManager contentManager, TileEngine tileEngine)
            : base(path)
        {
            texture = contentManager.Load<Texture2D>(path);
            this.tileEngine = tileEngine;
        }
    }
}
