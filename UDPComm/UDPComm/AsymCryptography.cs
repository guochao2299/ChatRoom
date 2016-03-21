using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDPComm
{
    public class AsymCryptography
    {
        private RSACryptoServiceProvider m_CryptoService;
        private const int KEY_SIZE = 1024;

        public AsymCryptography()
        {
            m_CryptoService = new RSACryptoServiceProvider(KEY_SIZE);            
        }

        public string Encrypt(string plainText)
        {
            byte[] plainByte = ASCIIEncoding.Unicode.GetBytes(plainText);
            byte[] cryptoByte = m_CryptoService.Encrypt(plainByte, false);
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
        }

        public string Decrypt(string cryptoText)
        {
            byte[] cryptoByte = Convert.FromBase64String(cryptoText);

            try
            {
                byte[] plainByte = m_CryptoService.Decrypt(cryptoByte, false);
                return ASCIIEncoding.Unicode.GetString(plainByte);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string PublicKey
        {
            get
            {
                MemoryStream stream = null;

                try
                {
                    RSAParameters p = m_CryptoService.ExportParameters(false);
                    IFormatter formatter = new BinaryFormatter();
                    stream = new MemoryStream();
                    formatter.Serialize(stream, p);                    
                    return Convert.ToBase64String(stream.ToArray());
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }                
            }
            set
            {
                byte[] keys= Convert.FromBase64String(value);
                MemoryStream stream = null;
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    stream = new MemoryStream(keys);
                    RSAParameters p=(RSAParameters)formatter.Deserialize(stream);

                    m_CryptoService.ImportParameters(p);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }          
            }
        }
    }
}
