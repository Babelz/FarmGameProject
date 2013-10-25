using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.SerializedDataTypes.Tiles;
using Microsoft.Xna.Framework;
using Khv.Maps.SerializedDataTypes.Layers.SerializedComponents;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.Processors
{
    /// <summary>
    /// luo ja indeksoi parametri listat tilejä varten, sisältää listan
    /// jossa on tuple tietorakenne index + object[] sisällä, 
    /// kun layerin tilejä alustetaan, haetaan indeksistä tilelle
    /// parametrit
    /// </summary>
    public class TileParameters
    {
        #region Vars
        private readonly SerializedMdiData mdiData;
        private readonly List<BaseSerializedTile> serializedTiles;
        private readonly TileEngine tileEngine;

        private List<TileParameter> tileParams;
        #endregion

        public TileParameters(SerializedMdiData mdiData, List<BaseSerializedTile> serializedTiles, TileEngine tileEngine)
        {
            this.mdiData = mdiData;
            this.serializedTiles = serializedTiles;
            this.tileEngine = tileEngine;

            tileParams = new List<TileParameter>();
            MakeArguments();
        }

        /// <summary>
        /// luo keypairit indeksistä ja parametristä
        /// </summary>
        /// <param name="serializedTiles"></param>
        /// <param name="tileEngine"></param>
        private void MakeArguments()
        {
            // vakiona nolla mutta jos käytetään mdi layeri,
            // niin valuet muuttuvat
            int indexOffSetX = 0;
            int indexOffSetY = 0;

            if (mdiData != null)
            {
                indexOffSetX = mdiData.PositionIndexX;
                indexOffSetY = mdiData.PositionIndexY;
            }

            // tile layerin valuepairien ja indeksien luonti
            if (serializedTiles[0].GetType() == typeof(SerializedBaseTile))
            {
                MakeTileArguments(indexOffSetX, indexOffSetY);
            }
            // rule layerin valuepairien ja indeksien luonti
            if(serializedTiles[0].GetType() == typeof(SerializedRuleTile))
            {
                MakeRuleArguments(indexOffSetX, indexOffSetY);
            }
            // objekti layerin valuepairien ja indeksien luonti
            if (serializedTiles[0].GetType() == typeof(SerializedObjectTile))
            {
                MakeObjectArguments(indexOffSetX, indexOffSetY);
            }
        }
        private void MakeObjectArguments(int indexOffSetX, int indexOffSetY)
        {
            foreach (SerializedObjectTile tileData in serializedTiles)
            {
                tileParams.Add(new TileParameter(new Index
                (
                    tileData.PositionIndexX - indexOffSetX,
                    tileData.PositionIndexY - indexOffSetY
                ),
                new object[]
                    {
                        new Vector2(tileData.PositionIndexX * tileEngine.TileSize.Width, tileData.PositionIndexY * tileEngine.TileSize.Height)
                    }));
            }
        }
        private void MakeRuleArguments(int indexOffSetX, int indexOffSetY)
        {
            RuleHelper ruleHelper = new RuleHelper();
            foreach (SerializedRuleTile tileData in serializedTiles)
            {
                tileParams.Add(new TileParameter(new Index
                (
                    tileData.PositionIndexX - indexOffSetX,
                    tileData.PositionIndexY - indexOffSetY
                ),
                new object[]
                    {
                        new Vector2(tileData.PositionIndexX * tileEngine.TileSize.Width, tileData.PositionIndexY * tileEngine.TileSize.Height),
                        ruleHelper.GetRuleByName(tileData.RuleName)
                    }));
            }
        }
        private void MakeTileArguments(int indexOffSetX, int indexOffSetY)
        {
            foreach (SerializedBaseTile tileData in serializedTiles)
            {
                tileParams.Add(new TileParameter(new Index
                (
                    tileData.PositionIndexX - indexOffSetX,
                    tileData.PositionIndexY - indexOffSetY
                ),
                new object[] 
                    {
                        new Vector2(tileData.PositionIndexX * tileEngine.TileSize.Width, tileData.PositionIndexY * tileEngine.TileSize.Height),
                        new Index(tileData.TextureIndexX, tileData.TextureIndexY)
                    }));
            }
        }

        /// <summary>
        /// palauttaa parametri listan indeksin perusteella
        /// </summary>
        /// <param name="index">indeksi jota haetaan</param>
        /// <returns>parametri lista jos se löytyy indeksistä</returns>
        public object[] GetParameters(Index index)
        {
            TileParameter args = tileParams.Find(a =>
                a.Index.X == index.X &&
                a.Index.Y == index.Y);

            tileParams.Remove(args);

            return args == null ? null : args.Parameters;
        }
    }
    public class TileParameter
    {
        #region Vars
        private readonly object[] parameters;
        private readonly Index index;
        #endregion

        #region Properites
        public object[] Parameters
        {
            get
            {
                return parameters;
            }
        }
        public Index Index
        {
            get
            {
                return index;
            }
        }
        #endregion

        public TileParameter(Index index, object[] arguments)
        {
            this.index = index;
            this.parameters = arguments;
        }
    }
}
