using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Farmi.Datasets;

namespace Farmi.Repositories
{
    internal sealed class ToolRepository : Repository<ToolDataset>
    {
        public ToolRepository(string name) 
            : base(name)
        {
        }

        private IEnumerable<XElement> GetToolElements(XDocument repository)
        {
            var toolElements = from tools in repository.Descendants("Items")
                               from tool in tools.Descendants()
                               where tool.Name == "Tool"
                               select tool;

            return toolElements;
        }

        public override void Load(XDocument repository)
        {
            var toolElements = GetToolElements(repository);

            foreach (var toolElement in toolElements)
            {
                ToolDataset toolDataset = new ToolDataset();
                toolDataset.ParseValuesFrom(toolElement);

                items.Add(toolDataset);
            }
        }
    }
}
