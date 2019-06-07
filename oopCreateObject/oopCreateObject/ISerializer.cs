using System;
using System.IO;

namespace oopCreateObject
{
    public interface ISerializer
    {
        void Serialize(Object item, Stream streamFile);
        Object Deserialize(Stream streamFile);
        string FileExtension { get; }
    }
}
