using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.MapClasses.Layers.Sheets;
using Khv.Maps.MapClasses.MapComponents.Layers.Sheets;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.Layers.Tiles
{
    /// <summary>
    /// perus tile johon piirretään tekstuureja
    /// </summary>
    public class Tile : BaseTile, IDrawableTile
    {
        #region Properties
        public Index TextureIndex
        {
            get;
            set;
        }
        private TileSheet TileSheet
        {
            get
            {
                return Sheet as TileSheet;
            }
        }
        #endregion

        public Tile(Vector2 position, Index textureIndex)
            : base(position)
        {
            TextureIndex = textureIndex;
        }
        public Tile(Vector2 position)
            : base(position)
        {
            TextureIndex = new Index(-1, -1);
        }

        public override bool IsEmpty()
        {
            return TextureIndex.X == -1 &&
                   TextureIndex.Y == -1;
        }
        public override void Clear()
        {
            TextureIndex = new Index(-1, -1);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = TileSheet.Sources[TextureIndex.Y, TextureIndex.X];

            spriteBatch.Draw(TileSheet.Texture, new Rectangle((int)Position.X, (int)Position.Y, source.Width, source.Height), source, Color.White);
        }
    }
}
