using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Tiles;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Engine.Structs;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;

namespace Khv.Maps.MapClasses.Layers
{
    /// <summary>
    /// layeri jonka koko on 0:llan ja kartan koon väliltä, layeriä 
    /// vois myös liikutella
    /// </summary>
    public class MdiLayer<T> : Layer<T>, IMdiLayer where T : BaseTile
    {
        #region Vars
        private readonly Vector2 originalPosition;
        private Vector2 position;
        private Rectangle rectangle;
        #endregion

        #region Properties
        public Vector2 OriginalPosition
        {
            get
            {
                return originalPosition;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa uuden sijainnin
        /// layerille. 
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                MoveTo(position.X, position.Y);
            }
        }
        public Point Index
        {
            get
            {
                return new Point((int)Position.X / tileEngine.TileSize.Width, (int)Position.Y / tileEngine.TileSize.Height);
            }
        }
        public Size RectangleSize
        {
            get
            {
                return new Size(rectangle.Width, rectangle.Height);
            }
        }
        #endregion

        public MdiLayer(KhvGame game, string name, bool transparent, bool visible, Size size, TileEngine tileEngine, Index startIndex)
            : base(game, name, transparent, visible, size, tileEngine)
        {
            rectangle = new Rectangle(startIndex.X * tileEngine.TileSize.Width, 
                                      startIndex.Y * tileEngine.TileSize.Height,
                                      size.Width * tileEngine.TileSize.Width, 
                                      size.Height * tileEngine.TileSize.Height);

            originalPosition = new Vector2(startIndex.X * tileEngine.TileSize.Width, 
                                                    startIndex.Y * tileEngine.TileSize.Height);

            position = new Vector2(OriginalPosition.X, OriginalPosition.Y);
        }

        public override void Initialize(TileParameters tileParameters)
        {
            base.Initialize(tileParameters);
            RePositionRectangle();
            RePositionTiles();
        }
        /// <summary>
        /// liikuttaa layerin indeksiin
        /// </summary>
        /// <param name="index">kartta indeksi mihin halutaan liikkua</param>
        public void MoveToIndex(Index index)
        {
            position = new Vector2(index.X * tileEngine.TileSize.Width, index.Y * tileEngine.TileSize.Height);
            RePositionRectangle();
            RePositionTiles();
        }
        /// <summary>
        /// liikuttaa layerin koordinaatteihin
        /// </summary>
        public void MoveTo(float x, float y)
        {
            position = new Vector2(x, y);
            RePositionRectangle();
            RePositionTiles();
        }
        /// <summary>
        /// liikuttaa layeriä halutun summan verran
        /// </summary>
        /// <param name="x">x arvo jolla liikutetaan x suunnassa</param>
        /// <param name="y">y arvo jolla liikutetaan y suunnassa</param>
        public void MoveBy(float x, float y)
        {
            position = new Vector2(Position.X + x, Position.Y + y);
            RePositionRectangle();
            RePositionTiles();
        }
        /// asettaa tilejen sijainnin uudelleen
        private void RePositionTiles()
        {
            Point index = Index;
            for (int h = 0; h < Size.Height; h++)
            {
                for (int w = 0; w < Size.Width; w++)
                {
                    if (tiles[h][w] != null)
                    {
                        (Tiles[h][w] as T).Move(new Vector2(position.X + (w * tileEngine.TileSize.Width),
                                                            position.Y + (h * tileEngine.TileSize.Height)));
                    }
                }
            }
        }
        /// uudelleen alustaa rectanglen
        private void RePositionRectangle()
        {
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, Size.Width * tileEngine.TileSize.Width,
                                                              Size.Height * tileEngine.TileSize.Height);
        }
    }
}
