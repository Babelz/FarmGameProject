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

                // Rakennuksen perustietojen hakeminen.
                #region Building parsing
                buildingDataset.Name = buildingData.Attribute("Name").Value;

                buildingDataset.Size = new Size(int.Parse(buildingData.Attribute("Width").Value),
                                                int.Parse(buildingData.Attribute("Height").Value));

                buildingDataset.AssetName = buildingData.Attribute("AssetName").Value;
                #endregion

                // Colliderin valueiden hakeminen.
                #region Collider parsing
                var colliderData = buildingData.Element("Collider");

                buildingDataset.ColliderPositionOffSet = new Vector2(float.Parse(colliderData.Attribute("X").Value),
                                                                     float.Parse(colliderData.Attribute("Y").Value));

                buildingDataset.ColliderSizeOffSet = new Size(int.Parse(colliderData.Attribute("Width").Value),
                                                              int.Parse(colliderData.Attribute("Height").Value));
                #endregion

                // Scriptien valueiden hakeminen.
                #region Script parsing
                buildingDataset.Scripts = (from scriptNames in buildingData.Descendants("Scripts")
                                           from scriptName in scriptNames.Descendants()
                                           select scriptName.Attribute("Name").Value).ToArray<string>();
                #endregion

                // Ovien valueiden hakeminen.
                #region Door parsing
                buildingDataset.Doors = (from doors in buildingData.Descendants("Doors")
                                         from door in doors.Descendants()
                                         select
                                         new DoorDataset()
                                         {
                                             AssetName = door.Attribute("AssetName").Value,
                                             TeleportTo = door.Attribute("TeleportTo").Value,
                                             Position = new Vector2(float.Parse(door.Attribute("X").Value),
                                                                    float.Parse(door.Attribute("Y").Value)),
                                             Size = new Size(int.Parse(door.Attribute("Width").Value),
                                                             int.Parse(door.Attribute("Height").Value))
                                         }).ToArray<DoorDataset>();
                #endregion

                items.Add(buildingDataset);
            }
        }
    }
}
