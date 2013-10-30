using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using System.Xml.Linq;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;

namespace Farmi.Repositories
{
    /// <summary>
    /// Repo joka sisältää rakennusten data settejä.
    /// </summary>
    internal sealed class BuildingRepository : Repository<BuildingDataset>
    {
        public BuildingRepository(string name)
            : base(name)
        {
        }

        // Palauttaa kaikki rakennus elementit tiedostosta.
        private IEnumerable<XElement> GetBuildingDatas(XDocument repository)
        {
            var buildingDatas = from items in repository.Descendants("Items")
                                from item in items.Elements()
                                where item.Name == "Building"
                                select item;

            return buildingDatas;
        }

        public override void Load(XDocument repository)
        {
            var buildingDatas = GetBuildingDatas(repository);

            foreach (var buildingData in buildingDatas)
            {
                BuildingDataset buildingDataset = new BuildingDataset();
                buildingDataset.ParseValuesFrom(buildingData);

                items.Add(buildingDataset);
            }
        }
    }
}
