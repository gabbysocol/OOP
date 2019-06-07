using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;


namespace oopCreateObject
{
    class JSONSerializer : ISerializer
    {
        public string FileExtension { get; } = ".json";
        public JSONSerializer()
        {

        }

        public void Serialize(Object itemList, Stream fileName)
        {
            string obj = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            using (StreamWriter fs = new StreamWriter(fileName))
            {
                fs.Write(obj);
            }
        }

        public Object Deserialize(Stream fileName)
        {
            string obj = String.Empty;

            using (StreamReader fs = new StreamReader(fileName))
            {
                obj = fs.ReadToEnd();
            }

            object objD = JsonConvert.DeserializeObject<Object>(obj, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            });

            return objD;
        }
    }
}
