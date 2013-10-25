using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Khv.Scripts.CSharpScriptEngine.Containers
{
    /// <summary>
    /// Luokka joka sisältää kaikki scriptien depencyt.
    /// </summary>
    public class ScriptDepencyContainer
    {
        #region Properties
        public string[] ScriptDepencies
        {
            get;
            private set;
        }
        #endregion

        public ScriptDepencyContainer(XDocument configurationFile)
        {
            ReadScriptDepencies(configurationFile);
            ReplaceKeywords();
        }

        #region Configurationfile parsing methods
        private void ReadScriptDepencies(XDocument configurationFile)
        {
            ScriptDepencies = (from scriptDenecyNodes in configurationFile.Descendants("ScriptDepencies")
                               from scriptDepencyNode in scriptDenecyNodes.Descendants()
                               select scriptDepencyNode.Value).ToArray<string>();
        }
        private void ReplaceKeywords()
        {
            if (ScriptDepencies.Contains("this"))
            {
                ScriptDepencies[Array.IndexOf<string>(ScriptDepencies, "this")] = Assembly.GetCallingAssembly().Location;
            }
        }
        private void RemoveDuplicates()
        {
            ScriptDepencies = ScriptDepencies.Distinct().ToArray();
        }
        #endregion
    }
}
