using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

using UDPComm;

namespace UDPTest.UDPComm
{
    [TestFixture]
    public class SymCryptographyTest
    {
        [Test]
        public void EncryptAndDecryptTest()
        {
            string plainText = "hello,my name is jsjs-gchao";
            SymCryptography crypt = new SymCryptography();

            string cryptedText = crypt.Encrypt(plainText);

            string decryptedText=crypt.Decrypt(cryptedText);

            Assert.AreEqual(plainText, decryptedText, decryptedText);
        }
    }
}
