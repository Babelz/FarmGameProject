using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Processors;

namespace Khv.Maps.MapClasses.Layers
{
    /// <summary>
    /// rajapinta josta kaikki layeri tyypit johdetaan
    /// </summary>
    public interface ILayer
    {
        #region Properties
        /// <summary>
        /// layerin nimi
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// kertoo onko layeri transparentti
        /// </summary>
        bool Transparent
        {
            get;
            set;
        }
        /// <summary>
        /// kertoo onko layeri näkyvissä
        /// </summary>
        bool Visible
        {
            get;
            set;
        }
        /// <summary>
        /// layerin koko tileissä
        /// </summary>
        Size Size
        {
            get;
        }
        /// <summary>
        /// layerin koko pikseleissä
        /// </summary>
        Size SizeInPixels
        {
            get;
        }
        /// <summary>
        /// layerin sheetti
        /// </summary>
        Sheet Sheet
        {
            get;
            set;
        }
        /// <summary>
        /// layerin piirto orderi
        /// </summary>
        DrawOrder DrawOrder
        {
            get;
        }
        /// <summary>
        /// layerin component manageri
        /// </summary>
        LayerComponentManager Components
        {
            get;
        }
        #endregion

        // metodi jossa tulee alustaa layeri
        void Initialize(TileParameters tileParameters);
        // päivittää kaikki komponentit jos layeri on näkyvissä
        void Update(GameTime gameTime);
        // piirtää kaikilla komponenteilla jos layeri on näkyvissä
        void Draw(SpriteBatch spriteBatch);
    }
}
