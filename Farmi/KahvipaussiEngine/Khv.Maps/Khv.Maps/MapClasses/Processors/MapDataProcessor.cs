using System;
using System.Collections.Generic;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Maps.MapClasses.Factories;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.SerializedDataTypes;
using Khv.Maps.SerializedDataTypes.Layers;
using Microsoft.Xna.Framework.Content;
using Khv.Maps.SerializedDataTypes.Tiles;

namespace Khv.Maps.MapClasses.Processors
{
    /// <summary>
    /// luokka joka prosessoi deserialisodun kartan tiedot
    /// ja luo siitä kartan
    /// </summary>
    public class MapDataProcessor
    {
        #region Vars
        private readonly KhvGame game;
        private readonly SerializedMap serializedMap;
        private readonly ContentManager contentManager;
        private readonly MapDepencyContainer mapDenecyContainer;
        
        private TileMap map;
        #endregion

        public MapDataProcessor(KhvGame game, SerializedMap serializedMap, MapDepencyContainer mapDenecyContainer)
        {
            this.game = game;
            this.serializedMap = serializedMap;
            this.mapDenecyContainer = mapDenecyContainer;

            contentManager = game.Content;
        }

        /// <summary>
        /// prosessoi kartta tiedot ja luo sen perusteella toimivia karttoja
        /// </summary>
        public TileMap Process()
        {
            // alustaa tile mottorin
            TileEngine tileEngine = new TileEngine(new Size(serializedMap.TileEngine.TileWidth, serializedMap.TileEngine.TileHeight),
                                                   new Size(serializedMap.TileEngine.MapWidth, serializedMap.TileEngine.MapHeight));

            // alustaa kartta olion ja tehtaat
            map = new TileMap(tileEngine, serializedMap.Name);
            
            // alustaa listat joissa on draworder parit sekä serialisoidut layeri
            var drawOrderPairs = new List<Tuple<string, int>>();
            var serializedLayers = SerializedMapLayersToList(serializedMap);

            // prosessoi jokaisen serialisoidun layerin datan
            // ja luo siitä layerin 
            ProcessLayers(drawOrderPairs, serializedLayers);

            // asettaa layereille oikeat draworderit ja lisää
            // viitteen niitten ordereista ordermanagerille
            ProcessDrawOrders(drawOrderPairs);

            // lohko jossa prosessoidaan kaikki kartan
            // komponentit
            if (serializedMap.Attributes.Count > 0)
            {
                ProcessMapComponents(serializedMap);
            }

            return map;
        }

        // Prosessoi ja luo kaikki kartta objektit.
        private void ProcessMapObjects(BaseSerializedLayer serializedLayer, ILayer layer)
        {
            // luo uuden instanssin objekti prosessorista ja prosessoi datan
            // sekä luo kartta objektit
            MapObjectProcessor mapObjectProcessor = new MapObjectProcessor(game, map.TileEngine, mapDenecyContainer.MapObjectNamespaces);

            GameObjectManager gameObjectManager = mapObjectProcessor.Process(serializedLayer as SerializedObjectLayer,
                                                                             layer as Layer<ObjectTile>);

            // lisää kartan objekti managerille uuden kartta objektin
            map.ObjectManagers.AddManager(gameObjectManager);

            layer.Components.AddComponent(new GameObjectComponent(game, layer, gameObjectManager));
        }

        // Prosessoi kartan komponentit.
        private void ProcessMapComponents(SerializedMap serializedMap)
        {
            MapComponentDataProcessor mapComponentDataProcessor = new MapComponentDataProcessor(game, map, mapDenecyContainer.MapComponentNamespaces, serializedMap.Attributes);
            mapComponentDataProcessor.Process();
        }

        // Prosessoi piirto orderit.
        private void ProcessDrawOrders(List<Tuple<string, int>> drawOrderPairs)
        {
            foreach (ILayer layer in map.LayerManager.AllLayers())
            {
                drawOrderPairs.ForEach(o =>
                    {
                        if (layer.Name == o.Item1)
                        {
                            layer.DrawOrder.Value = o.Item2;
                            map.DrawOrderManager.AddOrder(layer.DrawOrder);
                        }
                    });
            }
        }

        // Prosessoi kaikki layerit.
        private void ProcessLayers(List<Tuple<string, int>> drawOrderPairs, IEnumerable<BaseSerializedLayer> serializedLayers)
        {
            LayerFactory layerFactory = new LayerFactory(game, map.TileEngine);
            SheetFactory sheetFactory = new SheetFactory();

            foreach (BaseSerializedLayer serializedLayer in serializedLayers)
            {
                // alustaa layerin 
                Size size = new Size(serializedLayer.Width, serializedLayer.Height);
                ILayer layer = layerFactory.MakeNew(serializedLayer);

                // alustaa tilejen parametrit ja indeksoi ne
                TileParameters tileParameters = GetRightTileParameters(serializedLayer, map.TileEngine);

                // jos layeri on tilayeri, tehdään sille tile sheetti
                if (serializedLayer is SerializedTileLayer)
                {
                    SerializedTileLayer tileLayer = serializedLayer as SerializedTileLayer;
                    layer.Sheet = sheetFactory.MakeNew(layer.GetType(), new object[] 
                    { 
                        tileLayer.SheetPath, contentManager, map.TileEngine 
                    });
                }
                // jos layeri on animaatio layeri, tehdään sille animaatio sheetti
                else if (serializedLayer is SerializedAnimationLayer)
                {
                    SerializedAnimationLayer animationLayer = serializedLayer as SerializedAnimationLayer;
                    layer.Sheet = sheetFactory.MakeNew(layer.GetType(), new object[] 
                    { 
                        animationLayer.SheetPath, contentManager, map.TileEngine, 
                        new AnimationManager(animationLayer.AnimationManager.FrameCount, animationLayer.AnimationManager.FrameTime)
                    });
                }

                // alustaa layerin ja sen tilet
                layer.Initialize(tileParameters);
                // lisää layerin managerille
                map.LayerManager.AddLayer(layer);

                // jos layeri ei ole rule, otetaan sen draworder huomioon
                if (!(layer is Layer<RuleTile>))
                {
                    AddDrawOrder(drawOrderPairs, serializedLayer);
                }
                // prosessoidaan objectit jos layeri sisältää objecti tilejä
                if (layer is Layer<ObjectTile>)
                {
                    ProcessMapObjects(serializedLayer, layer);
                }

                // rakennetaan componentit layerille lopuksi
                ComponentBuilder componentBuilder = new ComponentBuilder(game);
                layer.Components.AddComponents(componentBuilder.BuildComponents(layer));
            }
        }

        // Lisää orderin listaan pairs listaan. Lisätään se vain jos layeri ei ole rule layer.
        private void AddDrawOrder(List<Tuple<string, int>> drawOrderPairs, BaseSerializedLayer serializedLayer)
        {
            drawOrderPairs.Add(new Tuple<string, int>(serializedLayer.Name, serializedLayer.DrawOrder));
        }

        // hakee kaikki layerit listaan
        private IEnumerable<BaseSerializedLayer> SerializedMapLayersToList(SerializedMap serializedMap)
        {
            var serializedLayers = new List<BaseSerializedLayer>();

            serializedLayers.AddRange(serializedMap.TileLayers);
            serializedLayers.AddRange(serializedMap.AnimationLayers);
            serializedLayers.AddRange(serializedMap.RuleLayers);
            serializedLayers.AddRange(serializedMap.ObjectLayers);

            return serializedLayers;
        }

        // asettaa oikeat parametrit tileparameters oliolle ja tekee niistä keypairit
        private TileParameters GetRightTileParameters(BaseSerializedLayer serializedLayer, TileEngine tileEngine)
        {
            dynamic layer = serializedLayer;

            TileParameters tileParameters = new TileParameters(serializedLayer.MdiData, new List<BaseSerializedTile>(layer.Tiles), tileEngine);

            return tileParameters;
        }
    }
}
