using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UDPComm;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "hello,my name is jsjs-gchao";
            SymCryptography crypt = new SymCryptography();

            string cryptedText = crypt.Encrypt(plainText);

            string decryptedText = crypt.Decrypt(cryptedText);

            Console.WriteLine(string.Format("PlainText:{0}", plainText));
            Console.WriteLine(string.Format("cryptedText:{0}", cryptedText));

            Console.ReadKey();
        }
    }
}
