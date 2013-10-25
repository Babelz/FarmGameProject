using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapComponents.Components;
using Khv.Maps.MapClasses.MapComponents;
using Khv.Maps.MapClasses.Factories;
using Khv.Maps.MapClasses.Managers;
using Khv.Engine;

namespace Khv.Maps.MapClasses.Processors
{
    /// <summary>
    /// Prosessori joka prosessoi kaikki kartan atribuutit 
    /// ja yrittää luoda tietojen perusteella olioja.
    /// </summary>
    public class MapComponentDataProcessor
    {
        #region Vars
        private readonly string[] componentNamespaces;
        private readonly KhvGame game;
        private readonly TileMap map;

        private readonly IEnumerable<string> componentDatas;
        #endregion

        public MapComponentDataProcessor(KhvGame game, TileMap map, string[] componentNamespaces, IEnumerable<string> componentDatas)
        {
            this.game = game;
            this.map = map;
            this.componentNamespaces = componentNamespaces;
            this.componentDatas = componentDatas;
        }

        /// <summary>
        /// Prosessoi kaikki komponentit, luo ne ja lisää karttaan.
        /// </summary>
        public void Process()
        {
            MapComponentFactory componentFactory = new MapComponentFactory(game, map, componentNamespaces);
            List<MapComponentData> mapComponentDatas = ProcessData();

            foreach (MapComponentData mapComponentData in mapComponentDatas)
            {
                MapComponent mapComponent = componentFactory.MakeNew(mapComponentData);

                if (mapComponent != null)
                {
                    map.ComponentManager.AddComponent(mapComponent);
                }
#if DEBUG
                else
                {
                    ThrowFailedToResolve(mapComponentData);
                }
#endif
            }
        }

        // Heittää poikkeuksen jos komponenttia ei pystytty luomaan.
        private void ThrowFailedToResolve(MapComponentData mapComponentData)
        {
            string namespaces = "";
            Array.ForEach(componentNamespaces, s => namespaces += s + Environment.NewLine);

            throw new Exception("Component " + mapComponentData.ComponentName + " could not be resolved! " +
                "Namespaces are " + namespaces + Environment.NewLine +
                "Please check syntax and namespaces.");
        }
        /// <summary>
        /// Prosessoi jokaisen atribuutin ja palauttaa component data olio
        /// listan tietojen perusteella.
        /// </summary>
        private List<MapComponentData> ProcessData()
        {
            List<MapComponentData> processedDatas = new List<MapComponentData>();

            foreach (string componentData in componentDatas)
            {
                processedDatas.Add(ProcessString(componentData));
            }

            return processedDatas;
        }
        // Parsii stringinä saadun datan ja palauttaa komponentti data olion.
        private MapComponentData ProcessString(string data)
        {
            // Splittaa datan.
            string[] datas = data.Split(new char[] { ' ', '(', ')' });
            string[] attribues = new string[datas.Length - 3];

            // Ottaa layerin ja komponentin nimen.
            string layerName = datas[0].Replace("-", "");
            string componentName = datas[1];

            // Kopioi osan tiedoista atributes taulukkoon.
            for (int i = 2; i < datas.Length - 1; i++)
            {
                attribues[i - 2] = datas[i];
            }

            return new MapComponentData(componentName, layerName, attribues);
        }
    }
}
