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
        private IEnumerable<XElement> GetBuildingElements(XDocument repository)
        {
            var buildingElements = from items in repository.Descendants("Items")
                                from item in items.Elements()
                                where item.Name == "Building"
                                select item;

            return buildingElements;
        }

        public override void Load(XDocument repository)
        {
            var buildingElements = GetBuildingElements(repository);

            foreach (var buildingElement in buildingElements)
            {
                BuildingDataset buildingDataset = new BuildingDataset();
                buildingDataset.ParseValuesFrom(buildingElement);

                items.Add(buildingDataset);
            }
        }
    }
}
