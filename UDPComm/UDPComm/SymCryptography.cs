using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace UDPComm
{
    public class SymCryptography
    {
        private string m_IV = string.Empty;
        private SymmetricAlgorithm m_CryptoService;

        /// <summary>
        /// 获取合法初始向量
        /// </summary>
        /// <returns></returns>
        private byte[] GetLegalIV()
        {
            string iv = m_IV.Substring(0, m_IV.Length);
            int n = m_CryptoService.BlockSize / 8;

            if (iv.Length < n)
            {
                iv = iv.PadRight(n, '0');
            }

            return ASCIIEncoding.ASCII.GetBytes(iv);
        }

        public SymCryptography()
        {
            m_CryptoService = new RijndaelManaged();
            m_CryptoService.Mode = CipherMode.CBC;
            m_CryptoService.GenerateKey();
        }

        public string Encrypt(string plainText)
        {
            byte[] plainByte = ASCIIEncoding.ASCII.GetBytes(plainText);
            m_CryptoService.IV = GetLegalIV();
            ICryptoTransform cryptoTransform = m_CryptoService.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cs.Write(plainByte, 0, plainByte.Length);
            cs.FlushFinalBlock();
            byte[] cryptoByte = ms.ToArray();
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
        }

        public string Decrypt(string cryptoText)
        {
            byte[] cryptoByte = Convert.FromBase64String(cryptoText);
            m_CryptoService.IV = GetLegalIV();
            ICryptoTransform cryptoTransform = m_CryptoService.CreateDecryptor();

            try
            {
                MemoryStream ms = new MemoryStream(cryptoByte, 0, cryptoByte.Length);
                CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        public string Key
        {
            get
            {
                return Convert.ToBase64String(m_CryptoService.Key, 0, m_CryptoService.Key.GetLength(0));
            }
            set
            {
                m_CryptoService.Key = Convert.FromBase64String(value);
            }
        }
    }
}
