using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.MapClasses.Factories;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Sheets.BaseClasses;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;

namespace Khv.Maps.MapClasses.Layers
{
    /// <summary>
    /// geneerinen pohjaluokka layereille
    /// </summary>
    /// <typeparam name="T">layerin tilejen tyyppi</typeparam>
    public class Layer<T> : ILayer where T : BaseTile
    {
        #region Vars
        protected readonly TileEngine tileEngine;
        protected LayerComponentManager components;
        protected T[][] tiles;
        protected string name;
        protected Size size;
        protected DrawOrder drawOrder;
        #endregion

        #region Properties
        public T[][] Tiles
        {
            get
            {
                return tiles;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public bool Transparent
        {
            get;
            set;
        }
        public bool Visible
        {
            get;
            set;
        }
        public Size Size
        {
            get
            {
                return size;
            }
        }
        public Size SizeInPixels
        {
            get
            {
                return new Size(size.Width * tileEngine.TileSize.Width, size.Height * tileEngine.TileSize.Height);
            }
        }
        public Sheet Sheet
        {
            get;
            set;
        }
        public DrawOrder DrawOrder
        {
            get
            {
                return drawOrder;
            }
        }
        public LayerComponentManager Components
        {
            get
            {
                return components;
            }
        }
        #endregion

        public Layer(KhvGame game, string name, bool transparent, bool visible, Size size, TileEngine tileEngine)
        {
            this.name = name;

            Transparent = transparent;
            Visible = visible;
            
            this.size = size;
            this.tileEngine = tileEngine;

            drawOrder = new DrawOrder(name);
            components = new LayerComponentManager();
        }

        // alustaa layerin tilet ja samalla huukkaa tileille viitteen sheetistä
        public virtual void Initialize(TileParameters tileParameters)
        {
            // luo uuden tile tehtaan geneerisen tyypin perusteella
            TileFactory factory = new TileFactory(typeof(T));

            tiles = new T[Size.Height][];
            for (int h = 0; h < Size.Height; h++)
            {
                tiles[h] = new T[Size.Width];
                for (int w = 0; w < Size.Width; w++)
                {
                    // hakee parametri listan indeksistä
                    object[] parameters = tileParameters.GetParameters(new Index
                    (
                        w,
                        h
                    ));

                    // jos parametri lista on null, tekee defaul muodostimelle oikeanlaisen listan joka
                    // käy kaikille tileille
                    if (parameters != null)
                    {
                        tiles[h][w] = (T)factory.MakeNew(parameters);
                    }
                }
            }
            // asettaa viitteen sheetistä vain jos layeri on animaatio tai 
            // tile, rule ja objekti sheetti eivät käytä sheettiä
            if (typeof(T).Equals(typeof(Tile)) || typeof(T).Equals(typeof(AnimationTile)))
            {
                HookSheetToTiles();
            }
        }
        /// <summary>
        /// palauttaa 3x3 matriisin positionin ympäriltä.
        /// </summary>
        public T[][] GetSurroundingTiles(Vector2 position, int addedWidth = 0, int addedHeight = 0)
        {
            // Kuinka monta tileä otetaan.
            int offsetX = 3 + addedWidth * 2, offsetY = 3 + addedHeight * 2;
            T[][] surroundingTiles = new T[offsetY][];

            for (int h = 0; h < surroundingTiles.Length; h++)
            {
                surroundingTiles[h] = new T[offsetX];
            }

            // Value joka lisätään positioniin ennen indeksin laskua jotta saadaan
            // position pysymään mahdollisimman keskellä tilejä.
            int addedX = tileEngine.TileSize.Width / 2;
            int addedY = tileEngine.TileSize.Height / 2;

            // Laskee oikean aloitus indeksin.
            // Miinustetaan alotus indeksistä jos halutaan ottaa enemmän tilejä
            // ja että position on tilejen keskellä.
            int startX, startY = 0;
            if (this is IMdiLayer)
            {
                startX = (int)((position.X - (this as IMdiLayer).Position.X + addedX) / tileEngine.TileSize.Width);
                startY = (int)((position.Y - (this as IMdiLayer).Position.Y + addedY) / tileEngine.TileSize.Height);
            }
            else
            {
                startX = (int)((position.X + addedX) / tileEngine.TileSize.Width) - addedWidth;
                startY = (int)((position.Y + addedY) / tileEngine.TileSize.Height) - addedHeight;
            }

            // Ei tarvitse koskea.
            int firstX = startX - 1;
            int firstY = startY - 1;

            // Hakee tilet.
            for (int h = startY - 1; h < startY + offsetY - 1; h++)
            {
                for (int w = startX - 1; w < startX + offsetX - 1; w++)
                {
                    if (w < 0 || w > Size.Width - 1)
                    {
                        continue;
                    }
                    else if (h < 0 || h > size.Height - 1)
                    {
                        continue;
                    }
                    else
                    {
                        surroundingTiles[h - firstY][w - firstX] = tiles[h][w];
                    }
                }
            }

            return surroundingTiles;
        }
        protected void HookSheetToTiles()
        {
            // antaa viitteen sheetistä tileille
            Array.ForEach(tiles, row =>
                Array.ForEach(row.Where(t => t != null).ToArray(), t =>
                    t.Sheet = Sheet));
        }
        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (LayerComponent layerComponent in components.AllComponents())
                {
                    layerComponent.Update(gameTime);
                }
            }
            else
            {
                foreach (IBackGroundUpdatableComponent layerComponent in components.AllComponents().Where(c => c as IBackGroundUpdatableComponent != null))
                {
                    layerComponent.DoBackgroundUpdates(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                foreach (DrawingLayerComponent layerComponent in components.DrawingComponents())
                {
                    layerComponent.Draw(spriteBatch);
                }
            }
        }
    }
}
