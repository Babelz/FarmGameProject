using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.SerializedDataTypes;
using Khv.Maps.MapClasses.Processors;
using Microsoft.Xna.Framework.Content;
using Khv.Engine;
using Khv.Engine.Args;
using System.IO;
using System.Xml.Linq;

namespace Khv.Maps.MapClasses.Managers
{
    public enum MapChangeAction
    {
        DisposeCurrent,
        MoveCurrentToBackground
    }

    /// <summary>
    /// Luokka joka sisältää kartat ja niihin liittyvät tiedot.
    /// Aktiivinen kartta asuu tässä oliossa.
    /// </summary>
    public class MapManager : Manager
    {
        #region Vars
        private KhvGame game;
        private ContentManager contentManager;
        private List<TileMap> mapsInBackground;
        private MapDepencyContainer mapDepencyContainer; 
        #endregion

        #region Events
        /// <summary>
        /// Kutsutaan kun karttaa aletaan vaihtamaan.
        /// </summary>
        public event MapEventHandler OnMapChanging;

        /// <summary>
        /// Kutsutaan kun kartta on vaihdettu.
        /// </summary>
        public event MapEventHandler OnMapChanged;
        #endregion

        #region Properties
        public TileMap ActiveMap
        {
            get;
            private set;
        }
        #endregion

        public MapManager(KhvGame game, string configurationFilePath)
            : base()
        {
            this.game = game;
            this.contentManager = game.Content;

            mapsInBackground = new List<TileMap>();
            mapDepencyContainer = new MapDepencyContainer(game, configurationFilePath);
        }

        /// <summary>
        /// Prosessoi kartan ja lataa sen. Jos
        /// kartta on jo taustalla olevissa kartoissa, palauttaa
        /// sen suorilta.
        /// </summary>
        private TileMap GetMap(string mapName)
        {
            TileMap map = mapsInBackground.Find(m => m.Name == mapName);

            return map ?? ProcessNewMap(mapName);
        }
        private TileMap ProcessNewMap(string mapName)
        {
            SerializedMap mapData = LoadMapData(mapName);

            MapDataProcessor mapDataProcessor = new MapDataProcessor(game, mapData, mapDepencyContainer);

            return mapDataProcessor.Process();
        }
        // DeSerialisoi kartan tiedot Xml tiedostosta SerializedMap olioksi.
        private SerializedMap LoadMapData(string mapName)
        {
            MapDataLoader mapDataLoader = new MapDataLoader(mapDepencyContainer.MapPaths);
            return mapDataLoader.Load(mapName);
        }
        private void RemoveFromBackground(TileMap map)
        {
            if (mapsInBackground.Contains(map))
            {
                mapsInBackground.Remove(map);
            }
        }
        private void MoveToBackground(TileMap map)
        {
            if (!mapsInBackground.Contains(map))
            {
                mapsInBackground.Add(map);
            }
        }
        /// <summary>
        /// Vaihtaa kartan halutuksi.
        /// </summary>
        public void ChangeMap(string mapName, MapChangeAction mapChangeAction = MapChangeAction.DisposeCurrent)
        {
            TileMap nextMap = GetMap(mapName);

            if (OnMapChanging != null)
            {
                OnMapChanging(this, new MapEventArgs(ActiveMap, nextMap));
            }
            switch (mapChangeAction)
            {
                case MapChangeAction.DisposeCurrent:
                    RemoveFromBackground(ActiveMap);
                    ActiveMap = null;
                    break;
                case MapChangeAction.MoveCurrentToBackground:
                    MoveToBackground(ActiveMap);
                    ActiveMap = null;
                    break;
            }

            ActiveMap = nextMap;

            if (OnMapChanged != null)
            {
                OnMapChanged(this, new MapEventArgs(ActiveMap, null));
            }
        }
        /// <summary>
        /// Lataa kartan ja laittaa sen backgroundille.
        /// </summary>
        public void LoadMap(string mapName)
        {
            MoveToBackground(ProcessNewMap(mapName));
        }
        /// <summary>
        /// Poistaa kartan mikä täyttää ehon. Tätä
        /// ei yleensä tarvitse käyttää jos mapit vaihdetaan dispose actionilla.
        /// </summary>
        public void RemoveMap(Predicate<TileMap> predicate)
        {
            if (predicate(ActiveMap))
            {
                ActiveMap = null;
            }
            else
            {
                mapsInBackground.Remove(
                    mapsInBackground.Find(m => predicate(m)));
            }
        }
        public IEnumerable<TileMap> MapsInBackground()
        {
            foreach (TileMap tileMap in mapsInBackground)
            {
                yield return tileMap;
            }
        }
    }
    public class MapEventArgs : GameEventArgs
    {
        #region Vars
        public TileMap Current
        {
            get;
            private set;
        }
        public TileMap Next
        {
            get;
            private set;
        }
        #endregion

        public MapEventArgs(TileMap current, TileMap next)
        {
            Current = current;
            Next = next;
        }
    }

    public delegate void MapEventHandler(object sender, MapEventArgs e);
}
