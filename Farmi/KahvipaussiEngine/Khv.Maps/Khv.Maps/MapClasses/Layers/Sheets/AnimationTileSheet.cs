using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;
using Microsoft.Xna.Framework.Content;
using Khv.Maps.MapClasses.Layers.Components;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.MapComponents.Layers.Sheets
{
    public class AnimationTileSheet : DrawableSheet
    {
        #region Properties
        public AnimationManager AnimationManager
        {
            get;
            private set;
        }
        #endregion

        public AnimationTileSheet(string path, ContentManager contentManager, TileEngine tileEngine, AnimationManager animationManager)
            : base(path, contentManager, tileEngine)
        {
            AnimationManager = animationManager;
            Initialize();
        }

        // alustaa animaatiot ja sourcet
        protected override void Initialize()
        {
            // tallennetaan koko tilapäiseen muuttujaan
            Size size = new Size((SheetWidth / tileEngine.TileSize.Width) * AnimationManager.FrameCount, Texture.Height / tileEngine.TileSize.Height);
            sources = new Rectangle[size.Height, size.Width];
            
            // luo sourcet kuvasta
            for (int h = 0; h < size.Height; h++)
            {
                for (int w = 0; w < size.Width; w++)
                {
                    sources[h, w] = new Rectangle(tileEngine.TileSize.Width * w, tileEngine.TileSize.Height * h,
                                                  tileEngine.TileSize.Width, tileEngine.TileSize.Height);
                }
            }
        }
    }
}
