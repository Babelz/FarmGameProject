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
        public ToolRepository(string name) : base(name)
        {
        }

        public override void Load(XDocument repository)
        {
            //throw new NotImplementedException();
        }
    }
}
