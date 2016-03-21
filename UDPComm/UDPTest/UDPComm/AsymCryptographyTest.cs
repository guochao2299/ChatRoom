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
    public class AsymCryptographyTest
    {
        [Test]
        public void EncryptAndDecryptTest()
        {
            string plainText = "hello,my name is jsjs-gchao";
            AsymCryptography cryptServer = new AsymCryptography();
            AsymCryptography cryptClient = new AsymCryptography();
            cryptClient.PublicKey = cryptServer.PublicKey;

            string cryptedText = cryptClient.Encrypt(plainText);

            string decryptedText = cryptServer.Decrypt(cryptedText);

            Assert.AreEqual(plainText, decryptedText, decryptedText);
        }
    }
}
