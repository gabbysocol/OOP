using System;
using System.IO;

namespace IRealiseCrypto
{
    public interface ICrypto
    {
        void EncryptStream(Stream sourcestream, Stream deststream, string key);
        void DecryptStream(Stream sourcestream, Stream deststream, string key);
        string Expansion { get; }
    }
}
