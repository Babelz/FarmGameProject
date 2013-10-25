using System.IO;
using System.Xml.Serialization;
using SerializedDataTypes.Components;

namespace SerializedDataTypes
{
    /// <summary>
    /// geneerinen luokka jolla voidaan deserialisoida olioja jotka perivät
    /// interfacen IDataCollection
    /// </summary>
    /// <typeparam name="T">IDataCollectionin perivä tyyppi</typeparam>
    public class CollectionLoader<T> where T : IDataCollection
    {
        /// <summary>
        /// lataa kokoelman ja palauttaa sen
        /// </summary>
        /// <param name="path">kokoelman sijainti</param>
        /// <returns></returns>
        public T Load(string path)
        {
            IDataCollection idataCollection;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StreamReader streamReader = new StreamReader(path))
            {
                idataCollection = (T)xmlSerializer.Deserialize(streamReader);
            }

            return (T)idataCollection;
        }
    }
}
