using POSH_Toolbelt.Services.Vault;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace POST_Toolbelt.Test.Unit
{
    public class AESThenHMACTests
    {
        [Fact]
        public void SimpleEncryptWithPassword_RightPassword_CanBeDecrypted()
        {
            var secret = "test";
            var password = "my12characterpass";
            var encryptedText = AESThenHMAC.SimpleEncryptWithPassword(secret, password);
            var decryptedText = AESThenHMAC.SimpleDecryptWithPassword(encryptedText, password);

            Assert.Equal(secret, decryptedText);
        }

        [Fact]
        public void SimpleEncryptWithPassword_WrongPassword_CannotBeDecrypted()
        {
            var secret = "test";
            var password = "my12characterpass";
            var wrongPassword = "thisisthewrongpass1";
            var encryptedText = AESThenHMAC.SimpleEncryptWithPassword(secret, password);
            var decryptedText = AESThenHMAC.SimpleDecryptWithPassword(encryptedText, wrongPassword);

            Assert.NotEqual(secret, decryptedText);
        }

        [Fact]
        public void PerformanceTest()
        {
            var reallyLongString = "";
            var password = "my12characterpass";

            for (var i = 0; i < 30000; i++)
            {
                reallyLongString += "Lorem ipsum dolor sit amet";
            }

            var encryptedText = AESThenHMAC.SimpleEncryptWithPassword(reallyLongString, password);
            var decryptedText = AESThenHMAC.SimpleDecryptWithPassword(encryptedText, password);

            Assert.Equal(reallyLongString, decryptedText);
        }

        [Fact]
        public void SimpleEncryptWithPassword_CiphertextModified_CannotBeDecrypted()
        {
            var secret = "test";
            var password = "my12characterpass";
            var encryptedText = AESThenHMAC.SimpleEncryptWithPassword(secret, password);
            var modifiedCharacterArray = encryptedText.ToCharArray();
            modifiedCharacterArray[5] = 'a';
            var modifiedText = new String(modifiedCharacterArray);
            var decryptedText = AESThenHMAC.SimpleDecryptWithPassword(modifiedText, password);

            Assert.NotEqual(secret, decryptedText);
            Assert.Null(decryptedText);
        }
    }
}
