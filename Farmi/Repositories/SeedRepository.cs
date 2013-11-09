using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Farmi.Datasets;
using Farmi.Entities;

namespace Farmi.Repositories
{
    internal sealed class SeedRepository : Repository<SeedDataset>
    {
        public SeedRepository(string name) : base(name)
        {
        }

        private IEnumerable<XElement> GetSeedElements(XDocument repository)
        {
            var seedElems = from seeds in repository.Descendants("Items")
                        from item in seeds.Descendants()
                        where item.Name == "Seed"
                        select (item);
            Console.WriteLine(seedElems.Count());
            return seedElems;

        }

        public override void Load(XDocument repository)
        {
            var seeds = GetSeedElements(repository);

            foreach (var seedElement in seeds)
            {
                SeedDataset seedSet = new SeedDataset();
                seedSet.ParseValuesFrom(seedElement);
                items.Add(seedSet);
            }
        }
    }
}
