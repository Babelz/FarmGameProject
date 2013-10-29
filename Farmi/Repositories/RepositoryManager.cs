using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Linq;

namespace Farmi.Repositories
{
    /// <summary>
    /// Luokka joka toimii tietokantana wrappaamalla repoja itseensä
    /// </summary>
    internal class RepositoryManager : IGameComponent
    {
        #region Vars
        private readonly string repositorysRootpath;
        private readonly string[] dataSetNamespaces;
        private readonly string[] repositoryNamespaces;
        private readonly Dictionary<Type, IRepository> repositories;
        #endregion

        /// <summary>
        /// Luo uuden instanssin managerista.
        /// </summary>
        /// <param name="repositorysRootpath">Kansio jossa repot sijaitsevat.</param>
        /// <param name="dataSetNamespaces">Nimiavaruudet joissa datasetit asuvat.</param>
        /// <param name="repositoryNamespaces">Nimiavaruudet joissa repo oliot asuvat.</param>
        public RepositoryManager(string repositorysRootpath, string[] dataSetNamespaces, string[] repositoryNamespaces)
        {
            this.repositorysRootpath = repositorysRootpath;
            this.dataSetNamespaces = dataSetNamespaces;
            this.repositoryNamespaces = repositoryNamespaces;

            repositories = new Dictionary<Type, IRepository>();
        }

        #region Error throwing methods
        /// <summary>
        /// Heittää errorin jos tyyppejä jää nulliksi viimeisessä vaiheessa 
        /// repon luontia.
        /// </summary>
        private void ThrowTypeError(Type datasetType, Type repositoryType, string repositoryName)
        {
            string missingTypes = string.Empty;

            if (datasetType == null)
            {
                missingTypes = "Dataset type, ";
            }

            if (repositoryType == null)
            {
                missingTypes += "Repository type.";
            }
            else
            {
                missingTypes = missingTypes.Substring(0, missingTypes.LastIndexOf(" ") - 2) + ".";
            }

            throw new ArgumentNullException("Given types were not found in assembly. Repository is " + repositoryName + 
                                            Environment.NewLine + "Types missing are " + missingTypes);
        }
        #endregion

        #region Loading methods
        // Hakee tyypin halutulla tyyppi nimellä ja annetuista nimiavaruuksista.
        private Type GetTypeFrom(string typename, string[] namespaces)
        {
            Type results = null;

            // Käydään jokainen nimiavaruus läpi ja katsotaan saadaanko tyyppiä ulos.
            foreach (string objectNamespace in namespaces)
            {
                if (results != null)
                {
                    break;
                }

                results = Type.GetType(objectNamespace + typename);
            }

            return results;
        }
        
        /// <summary>
        /// Lataa kaikki repot annetusta hakemistosta.
        /// </summary>
        private void LoadRepositories(string[] filenames)
        {
            // Hakee jokaisen repon XML tiedoston ja parsii ne.
            foreach (XDocument repositoryFile in GetRepositoryFiles(filenames))
            {
                // Hakee root atribuutit tiedostosta.
                XAttribute[] attributes = repositoryFile.Descendants("Repository")
                                          .Attributes().ToArray();

                // Hakee atribuuteista tarvittavat tiedot, heittää errorin jos jotain menee vikaan,
                // atribuutti voi olla null.
                string repositoryName = Array.Find(attributes, s => s.Name == "Name").Value;
                string dataSetTypeName = Array.Find(attributes, s => s.Name == "DatasetType").Value;
                string repositoryTypeName = Array.Find(attributes, s => s.Name == "RepositoryType").Value;

                // Lisää uuden repostoryn.
                AddNewRepository(repositoryName, repositoryTypeName, dataSetTypeName, repositoryFile);
            }
        }

        /// <summary>
        /// Lataa kaikki repostory filet muistiin.
        /// </summary>
        private XDocument[] GetRepositoryFiles(string[] filenames)
        {
            return filenames
                .Select(s => XDocument.Load(s))
                .ToArray<XDocument>();
        }

        /// <summary>
        /// Lisää uuden repostoryn listaan.
        /// </summary>
        private void AddNewRepository(string repositoryName, string repositoryTypeName, string dataSetTypeName, XDocument repositoryFile)
        {
            IRepository repository;

            // Hakee tyypit.
            Type repositoryType = GetTypeFrom(repositoryTypeName, repositoryNamespaces);
            Type dataSetType = GetTypeFrom(dataSetTypeName, dataSetNamespaces);

            // Jos jompikumpi tyypeistä on null, heittää poikkeuksen.
            if (dataSetType == null || repositoryType == null)
            {
                ThrowTypeError(dataSetType, repositoryType, repositoryName);
            }

            // Luo uuden instanssin reposta ja lataa sen tiedot.
            repository = (IRepository)Activator.CreateInstance(repositoryType, new object[] { repositoryName });
            repository.Load(repositoryFile);

            // Lisää uuden repon listaan.
            repositories.Add(dataSetType, repository);
        }
        #endregion

        public void Initialize()
        {
            if(repositories.Count > 0)
            {
                return;
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + repositorysRootpath;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path)
                                .Where(s => s.EndsWith(".db"))
                                .ToArray<string>();

                LoadRepositories(files);
            }
        }

        /// <summary>
        /// Palauttaa halutun tyyppisen datan 
        /// joka täyttää ensimmäisenä ehdon.
        /// Voi palauttaa tyhjän struktin tai nullin.
        /// </summary>
        public T GetDataSet<T>(Predicate<T> predicate)
        {
            Repository<T> repository = repositories[typeof(T)] as Repository<T>;

            return repository.GetItem(predicate);
        }
    }
}
