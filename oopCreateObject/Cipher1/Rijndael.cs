using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IRealiseCrypto;

namespace RijndaelCode
{
    public class RijndaelCode : ICrypto
    {
        //Work requires a vector of initialization (IV) and a key (Key)
        //Enciphering operations / deciphering have to use identical values IV and Key

        public void EncryptStream(Stream sourcestream, Stream deststream, string key)
        {
            byte[] _key = new byte[32];
            byte[] _IV = new byte[16];

            byte[] myKey = Encoding.ASCII.GetBytes(key);

            for (int i = 0; i < _key.Length; i++)
                _key[i] = 0;
            for (int i = 0; (i < _key.Length) && (i < myKey.Length); i++)
                _key[i] = myKey[i];
            for (int i = 0; (i < _key.Length) && (i < _IV.Length); i++)
                _IV[i] = _key[i];
            _IV.Reverse();

            Rijndael rijndael = RijndaelManaged.Create();
            //DES dES = DESCryptoServiceProvider.Create();
            rijndael.IV = _IV;
            rijndael.Key = _key;

            var decStream = new CryptoStream(sourcestream, rijndael.CreateEncryptor(), CryptoStreamMode.Read);
            deststream.SetLength(0);
            decStream.CopyTo(deststream);
        }

        public void DecryptStream(Stream sourcestream, Stream deststream, string key)
        {
            byte[] _key = new byte[32];
            byte[] _IV = new byte[16];

            byte[] myKey = Encoding.ASCII.GetBytes(key);

            for (int i = 0; i < _key.Length; i++) _key[i] = 0;
            for (int i = 0; (i < _key.Length) && (i < myKey.Length); i++) _key[i] = myKey[i];
            for (int i = 0; (i < _key.Length) && (i < _IV.Length); i++) _IV[i] = _key[i];
            _IV.Reverse();

            Rijndael rijndael = RijndaelManaged.Create();
            rijndael.IV = _IV;
            rijndael.Key = _key;

            var decStream = new CryptoStream(sourcestream, rijndael.CreateDecryptor(), CryptoStreamMode.Read);
            deststream.SetLength(0);
            decStream.CopyTo(deststream);

        }

        public string Expansion { get; } = ".rjndl";
    }
}
