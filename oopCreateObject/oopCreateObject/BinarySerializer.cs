using System;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace oopCreateObject
{
    class BinarySerializer : ISerializer
    {
        public string FileExtension { get; } = ".dat";

        public BinarySerializer()
        {
        }

        // save in local file
        public void Serialize(Object itemList, Stream  fileName)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            // for file
            //using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            //{
            //    binFormatter.Serialize(fs, itemList);
            //}
            binFormatter.Serialize(fileName, itemList);
        }

        public Object Deserialize(Stream fileName)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            Object obj = null;
            //using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            //{
            //    obj = binFormatter.Deserialize(fs);
            //}
            obj = binFormatter.Deserialize(fileName);
            return obj;
        }
    }
}
