using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDPComm
{
    public class UDPComm
    {
        public static byte[] EncodingASCII(string buf, SymCryptography cryptoService)
        {
            return Encoding.Unicode.GetBytes(cryptoService.Encrypt(buf));
        }

        public static string DecodingASCII(byte[] buf, SymCryptography cryptoService)
        {
            return cryptoService.Decrypt(Encoding.ASCII.GetString(buf));
        }

        public static byte[] EncodingASCII(string buf, AsymCryptography cryptoService)
        {
            return Encoding.Unicode.GetBytes(cryptoService.Encrypt(buf));
        }

        public static string DecodingASCII(byte[] buf, AsymCryptography cryptoService)
        {
            return cryptoService.Decrypt(Encoding.ASCII.GetString(buf));
        }

        public static byte[] EncodingASCII(string buf)
        {
            return Encoding.Unicode.GetBytes(buf + "\r\n");
        }

        public static byte[] EncodingASCIIPlus(string buf)
        {
            return Encoding.Unicode.GetBytes(buf);
        }

        public static string DecodingASCII(byte[] buf)
        {
            return Encoding.Unicode.GetString(buf);
        }
    }
}
