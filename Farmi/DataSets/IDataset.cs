using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Farmi.Datasets
{
    public interface IDataset
    {
        void ParseValuesFrom(XElement xElement);
        XElement AsXElement();
    }
}
