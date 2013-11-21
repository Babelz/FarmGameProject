using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Farmi.Entities.Scripts
{
    public interface ISavableBehaviour
    {
        List<XElement> SaveBehaviourState();
    }
    public interface ILoadableBehaviour
    {
        void LoadBehaviourState(List<XElement> xElements);
    }
}
