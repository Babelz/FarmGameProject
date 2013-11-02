using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using System.Xml.Linq;

namespace Farmi.Repositories
{
    internal sealed class AnimalRepository : Repository<AnimalDataset>
    {
        public AnimalRepository(string name)
            : base(name)
        {
        }

        private IEnumerable<XElement> GetAnimalElements(XDocument repository)
        {
            var animalElements = from items in repository.Descendants("Items")
                                 from item in items.Elements()
                                 where item.Name == "Animal"
                                 select item;

            return animalElements;
        }

        public override void Load(XDocument repository)
        {
            var animalElements = GetAnimalElements(repository);

            foreach (var animalElement in animalElements)
            {
                AnimalDataset animalDataset = new AnimalDataset();
                animalDataset.ParseValuesFrom(animalElement);

                items.Add(animalDataset);
            }
        }
    }
}
