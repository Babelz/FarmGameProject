using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;
using Khv.Maps.MapClasses.Layers.Components;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.MapComponents.Layers.Sheets
{
    /// <summary>
    /// perus sheeti josta tilet piirtävät tekstuurinsa
    /// </summary>
    public class TileSheet : DrawableSheet
    {
        public TileSheet(string path, ContentManager contentManager, TileEngine tileEngine)
            : base(path, contentManager, tileEngine)
        {
            Initialize();
        }

        // alustaa sheetin sourcet
        protected override void Initialize()
        {
            // tallennetaan koko tilaipäiseen muuttujaan
            Size size = new Size(SheetWidth / tileEngine.TileSize.Width, texture.Height / tileEngine.TileSize.Height);
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
