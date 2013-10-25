using System;
using System.Collections.Generic;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.SerializedDataTypes.Layers;
using Microsoft.Xna.Framework;
using SerializedDataTypes.MapObjects;

namespace Khv.Maps.MapClasses.Processors
{
    /// <summary>
    /// Prosessori joka prosessoi serialisoidun layerin oliot ja luo niistä 
    /// instanssit.
    /// </summary>
    public class MapObjectProcessor
    {
        #region Vars
        private readonly string[] objectNamespaces;
        private readonly KhvGame game;
        private readonly TileEngine tileEngine;
        #endregion

        public MapObjectProcessor(KhvGame game, TileEngine tileEngine, string[] objectNamespaces)
        {
            this.game = game;
            this.tileEngine = tileEngine;
            this.objectNamespaces = objectNamespaces;
        }

        public GameObjectManager Process(SerializedObjectLayer serializedObjectLayer, Layer<ObjectTile> layer)
        {
            #region Object parsing and creation
            GameObjectManager objectManager = new GameObjectManager(layer);
            List<MapObjectParameters> parameters = GetContainingObjectsParameters(serializedObjectLayer);

            // Loopataan jokainen parametri läpi.
            foreach (MapObjectParameters parameter in parameters)
            {
                Type objectType = null;
                GameObject mapObject = null;

                
                // Loopataan jokainen userin syöttämä nimiavaruus.
                foreach (string objectNamespace in objectNamespaces)
                {
                    // Koitetaan saada tyyppiä nimiavaruudesta.
                    objectType = Type.GetType(objectNamespace + "." + parameter.SerializedData.Name);

                    // Jos tyyppi löytyi, yrittää luoda uuden kartta objectin, jos tämä ei onnistu ja 
                    // debugataan, heittää poikkeuksen.
                    if (objectType != null)
                    {
                        parameter.Origin = layer.Tiles[parameter.OriginTileIndex.Y][parameter.OriginTileIndex.X].Position;
                        mapObject = CreateObject(game, objectType, parameter);
                        objectManager.AddGameObject(mapObject);
                        break;
                    }
                }

                // Heittää poikkeuksen jos debugataan ja 
                // olio jää nulliksi.
                #region Debug exception
#if DEBUG
                if (mapObject == null)
                {
                    ThrowFailedToResolve(parameter);
                }
#endif
                #endregion
            }
            #endregion

            return objectManager;
        }

        // Heittää poikkeuksen kun olion luominen epäonnistuu.
        private void ThrowFailedToResolve(MapObjectParameters parameter)
        {
            string namespaces = "";
            Array.ForEach(objectNamespaces, s => namespaces += s);

            throw new Exception("Map object type could not be resolved, object data is " + parameter.SerializedData.ToString() + Environment.NewLine +
                "Namespaces are " + Environment.NewLine + namespaces);
        }

        // Luo aktivaattorilla uuden instanssin halutusta oliosta.
        private GameObject CreateObject(KhvGame game, Type type, MapObjectParameters data)
        {
            return (GameObject)Activator.CreateInstance(type, game, data);
        }

        // Hakee kaikki tiedot layeristä luomista varten.
        private List<MapObjectParameters> GetContainingObjectsParameters(SerializedObjectLayer serializedObjectLayer)
        {
            List<MapObjectParameters> parameters = new List<MapObjectParameters>();

            serializedObjectLayer.Tiles.ForEach(t =>
                {
                    parameters.Add(new MapObjectParameters(new Index(t.PositionIndexX, t.PositionIndexY), t.MapObject));
                });

            return parameters;
        }
    }
    /// <summary>
    /// Olio joka luodaan wrappaamaan tietoja ennen kun serialisoidusta oliosta
    /// luodaan instanssi.
    /// </summary>
    public class MapObjectParameters
    {
        #region Vars
        private readonly Index index;
        private readonly SerializedMapObject serializedData;
        #endregion

        #region Properties
        /// <summary>
        /// Indexi minkä tilen kohdalla olio sijaitsee.
        /// </summary>
        public Index OriginTileIndex
        {
            get
            {
                return index;
            }
        }
        /// <summary>
        /// Olion serialisoitu data.
        /// </summary>
        public SerializedMapObject SerializedData
        {
            get
            {
                return serializedData;
            }
        }
        /// <summary>
        /// Position näytöllä.
        /// </summary>
        public Vector2 Origin
        {
            get;
            set;
        }
        #endregion

        public MapObjectParameters(Index index, SerializedMapObject serializedData)
        {
            this.index = index;
            this.serializedData = serializedData;
        }
    }
}
