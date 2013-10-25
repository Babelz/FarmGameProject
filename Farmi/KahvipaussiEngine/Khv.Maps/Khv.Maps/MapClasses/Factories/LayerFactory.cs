using System;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.SerializedDataTypes.Layers;

namespace Khv.Maps.MapClasses.Factories
{
    /// <summary>
    /// Tehdas joka luo layereitä serialisoitujen datojen perusteella
    /// reflectionin avulla.
    /// </summary>
    public class LayerFactory
    {
        #region Vars
        private const string tileNamespace = "Khv.Maps.MapClasses.Layers.Tiles.";

        private readonly KhvGame game;
        private readonly TileEngine tileEngine;
        #endregion

        public LayerFactory(KhvGame game, TileEngine tileEngine)
        {
            this.game = game;
            this.tileEngine = tileEngine;
        }

        /// <summary>
        /// Luo uuden layerin parametrinä saatavasta datasta.
        /// </summary>
        /// <param name="layerData">Data josta halutaan luoda uusi layeri.</param>
        /// <param name="tileEngine">Kartan tilemoottori olio.</param>
        /// <returns>Datasta luotu layeri.</returns>
        public ILayer MakeNew(BaseSerializedLayer layerData)
        {
            object[] args = null;
            string tileTypeName = tileTypeName = layerData.GetType().Name.Replace("Serialized", "").Replace("Layer", "");

            if(!(tileTypeName.Contains("Tile")))
            {
                tileTypeName += "Tile";
            }

            Type tileType = Type.GetType(tileNamespace + tileTypeName);
            Type layerType = layerData.IsMdiLayer ? typeof(MdiLayer<>) : typeof(Layer<>);
            layerType = layerType.MakeGenericType(tileType);

            args = layerData.IsMdiLayer ? GetMdiLayerArguments(layerData) : GetLayerAruments(layerData);

            // Kutsuu layerin muodostinta parametrillä ja palauttaa sen.
            return (ILayer)Activator.CreateInstance(layerType, args);
        }

        private object[] GetLayerAruments(BaseSerializedLayer layerData)
        {
            object[] args;

            args = new object[] { game, layerData.Name, layerData.Transparent, layerData.Visible, new Size(layerData.Width, layerData.Height), tileEngine };
            
            return args;
        }
        private object[] GetMdiLayerArguments(BaseSerializedLayer layerData)
        {
            object[] args;

            args = new object[] { game, layerData.Name, layerData.Transparent, layerData.Visible, new Size(layerData.Width, layerData.Height), tileEngine, new Index( 
                    layerData.MdiData.PositionIndexX,
                    layerData.MdiData.PositionIndexY
                ) };

            return args;
        }
    }
}
