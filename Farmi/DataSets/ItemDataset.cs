using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Farmi.Datasets
{
    internal sealed class ItemDataset : IDataset
    {
        #region Vars
        private XElement xElement;
        #endregion

        #region Properties
        /// <summary>
        /// Itemin nimi.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }
        /// <summary>
        /// Itemin textuuri
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }
        /// <summary>
        /// Kuvaus itemistä joka näkyy tooltippinä
        /// </summary>
        public string Description
        {
            get;
            private set;
        }
        /// <summary>
        /// Palauttaa behaviourit
        /// </summary>
        public List<string> Behaviours
        {
            get; 
            private set;
        }
        

        #endregion

        public void ParseValuesFrom(XElement xElement)
        {
            this.xElement = xElement;
            GetBasicValues(xElement);
            ParseBehaviours(xElement);
            throw new NotImplementedException();
        }

        private void GetBasicValues(XElement xelement)
        {
            Name = xElement.Attribute("Name").Value;
            AssetName = xElement.Attribute("AssetName").Value;
            Description = xElement.Attribute("Description").Value;
        }

        private void ParseBehaviours(XElement element)
        {
            Behaviours = new List<string>();
#warning ItemDataset - Behaviours parsing = not implemented
        }

        public XElement AsXElement()
        {
            return xElement;
        }
    }
}
