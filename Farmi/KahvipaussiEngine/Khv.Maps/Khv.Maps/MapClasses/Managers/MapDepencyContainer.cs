using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using Khv.Engine;

namespace Khv.Maps.MapClasses.Managers
{
    /// <summary>
    /// Luokka joka wräppää kaikki depencyt, nimiavaruudet ja pathit
    /// joita tarvitaan karttojen lataamisessa.
    /// </summary>
    public class MapDepencyContainer
    {
        #region Vars
        private readonly string configurationFilePath;

        private List<string> errors;
        #endregion

        #region Properties
        public string[] MapPaths
        {
            get;
            private set;
        }
        public string[] MapObjectNamespaces
        {
            get;
            private set;
        }
        public string[] MapComponentNamespaces
        {
            get;
            private set;
        }
        #endregion

        public MapDepencyContainer(KhvGame game, string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
            if (File.Exists(configurationFilePath))
            {
                errors = new List<string>();
                XDocument configurationFile = XDocument.Load(configurationFilePath);

                ReadMapPaths(configurationFile);
                
                // Tyypit assemblystä 
                Type[] executingAssemblyTypes = Assembly.GetAssembly(game.GetType()).GetTypes();
                Type[] containingAssemblyTypes = Assembly.GetCallingAssembly().GetTypes();

                ReadMapObjectNamespaces(configurationFile, executingAssemblyTypes, containingAssemblyTypes);
                ReadMapComponentNamespaces(configurationFile, executingAssemblyTypes, containingAssemblyTypes);

            }
            else
            {
                throw new FileNotFoundException("Configurationfile for Maps was not found at path " + configurationFilePath);
            }

            errors.Clear();
            errors = null;
        }

        // Lukee karttojen polut tiedostosta.
        private void ReadMapPaths(XDocument configurationFile)
        {
            MapPaths = (from pathNodes in configurationFile.Descendants("MapPaths")
                        from pathNode in pathNodes.Descendants()
                        select pathNode.Value).ToArray<string>();
#if DEBUG
            Array.ForEach(MapPaths, s =>
            {
                if (!Directory.Exists(s))
                {
                    errors.Add(s + Environment.NewLine);
                }
            });

            if (errors.Count > 0)
            {
                throw new DirectoryNotFoundException("One or more paths in " + configurationFilePath + " do not exist!" +
                    ErrorsToString());
            }

            errors.Clear();
#endif
        }

        // Lukee karttaobjektien nimiavaruudet tiedostosta.
        private void ReadMapObjectNamespaces(XDocument configurationFile, Type[] executingAssemblyTypes, Type[] containingAssemblyTypes)
        {
            MapObjectNamespaces = (from namespaceNodes in configurationFile.Descendants("MapObjectNamespaces")
                                   from namespaceNode in namespaceNodes.Descendants()
                                   select namespaceNode.Value).ToArray<string>();
            
#if DEBUG
            foreach (string mapObjectNamespace in MapObjectNamespaces)
            {
                if (executingAssemblyTypes.Any(s => s.Namespace == mapObjectNamespace) || containingAssemblyTypes.Any(s => s.Namespace == mapObjectNamespace))
                {
                    break;
                }
                else
                {
                    errors.Add(MapComponentNamespaces + Environment.NewLine);
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception("One or more mapobject namespaces in " + configurationFilePath + " do not exist!" + Environment.NewLine +
                    ErrorsToString());
            }

            errors.Clear();
#endif
        }

        // Lukee karttakomponenttien nimiavaruudet tiedostosta.
        private void ReadMapComponentNamespaces(XDocument configurationFile, Type[] executingAssemblyTypes, Type[] containingAssemblyTypes)
        {
            MapComponentNamespaces = (from namespaceNodes in configurationFile.Descendants("MapComponentNamespaces")
                                      from namespaceNode in namespaceNodes.Descendants()
                                      select namespaceNode.Value).ToArray<string>();

#if DEBUG
            foreach (string componentNamespace in MapComponentNamespaces)
            {
                if (executingAssemblyTypes.Any(s => s.Namespace == componentNamespace) || containingAssemblyTypes.Any(s => s.Namespace == componentNamespace))
                {
                    break;
                }
                else 
                {
                    errors.Add(componentNamespace + Environment.NewLine);
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception("One or more mapcomponent namespaces in " + configurationFilePath + " do not exist!" +
                    ErrorsToString());
            }

            errors.Clear();
#endif
        }

        // Palauttaa stringinä kaikki errorit listasta.
        private string ErrorsToString()
        {
            string errorString = "";
            errors.ForEach(s => errorString += s);
            return errorString;
        }
    }
}
