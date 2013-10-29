using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.SerializedDataTypes;
using System.Xml.Serialization;
using System.IO;
using Khv.Maps.MapClasses.Managers;

namespace Khv.Maps.MapClasses.Processors
{
    /// <summary>
    /// käytetään serialisoitujen kartta tietojen lataamiseen
    /// </summary>
    public class MapDataLoader
    {
        #region Vars
        private readonly string[] paths;
        #endregion

        public MapDataLoader(string[] paths)
        {
            this.paths = paths;
        }

        /// <summary>
        /// deserialisoi kartan ja palauttaa sen
        /// </summary>
        /// <param name="mapname">kartan polku ja nimi</param>
        /// <returns>deserialisoitu kartta</returns>
        public SerializedMap Load(string mapname)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializedMap));
            SerializedMap serializedMap = null;

            string fullName = ResolveFullName(mapname + ".mp");
            using (StreamReader streamReader = new StreamReader(fullName))
            {
                serializedMap = (SerializedMap)xmlSerializer.Deserialize(streamReader);
            }

            return serializedMap;
        }
        private string ResolveFullName(string mapname)
        {
            string fullName = null;

            foreach (string path in paths)
            {
                if (File.Exists(path + mapname))
                {
                    fullName = path + mapname;
                    break;
                }
            }
            if (string.IsNullOrEmpty(fullName))
            {
                throw new FileNotFoundException("Map file named " + mapname + " was not found! Paths are: " + Environment.NewLine +
                    (new Func<string>(() => { string str = ""; Array.ForEach(paths, s => str += s); return str; }))());
            }

            return fullName;
        }
    }
}
