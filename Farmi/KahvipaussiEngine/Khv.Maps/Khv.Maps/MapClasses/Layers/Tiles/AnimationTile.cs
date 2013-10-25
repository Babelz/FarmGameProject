using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;
using Khv.Maps.MapClasses.Layers.Components;
using Microsoft.Xna.Framework.Graphics;
using Khv.Maps.MapClasses.Layers.Sheets;
using Khv.Maps.MapClasses.MapComponents.Layers.Sheets;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.Layers.Tiles
{
    /// <summary>
    /// tile joka on animoitu
    /// </summary>
    public class AnimationTile : Tile, IUpdatableTile
    {
        #region Properties
        private AnimationTileSheet AnimationSheet
        {
            get
            {
                return Sheet as AnimationTileSheet;
            }
        }
        #endregion

        public AnimationTile(Vector2 position, Index textureIndex)
            : base(position)
        {
            TextureIndex = textureIndex;
        }

        public AnimationTile(Vector2 position)
            : base(position)
        {
            TextureIndex = new Index(-1, -1);
        }

        // päivittää tilen texturea
        public void Update(GameTime gameTime)
        {
            TextureIndex = new Index(TextureIndex.X + AnimationSheet.AnimationManager.NextFrame, TextureIndex.Y);
        }
        // piirtää tilen
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = AnimationSheet.Sources[TextureIndex.Y, TextureIndex.X];

            spriteBatch.Draw(AnimationSheet.Texture, new Rectangle((int)Position.X, (int)Position.Y, source.Width, source.Height), source, Color.White);
        }
    }
}
