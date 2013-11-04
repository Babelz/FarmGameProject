using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using System.Xml.Linq;

namespace Farmi.Repositories
{
    internal sealed class AnimalFeedRepository : Repository<FeedDataset>
    {
        public AnimalFeedRepository(string name)
            : base(name)
        {
        }

        private IEnumerable<XElement> GetFeedElements(XDocument repository)
        {
            var feedElements = from items in repository.Descendants("Items")
                               from item in items.Elements()
                               where item.Name == "Feed"
                               select item;

            return feedElements;
        }

        public override void Load(XDocument repository)
        {
            var feedElements = GetFeedElements(repository);

            foreach (var feedElement in feedElements)
            {
                FeedDataset feedDataset = new FeedDataset();
                feedDataset.ParseValuesFrom(feedElement);

                items.Add(feedDataset);
            }
        }
    }
}
