using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POSH_Toolbelt.Services.Vault
{
    public class VaultService
    {
        private const string UserVaultName = "secrets.ptvault";

        public string GetUserVaultPath()
        {
            var pathToUserDocuments = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var userAppDataDirectory = Path.Combine(pathToUserDocuments, "Documents\\POSHToolbelt");
            var userVaultPath = Path.Combine(userAppDataDirectory, UserVaultName);
            return userVaultPath;
        }

        public VaultStatus OpenVaultFile(string path)
        {

            var status = new VaultStatus();
            if(File.Exists(path))
            {
                var encryptedVault = File.ReadAllText(path);
                status.EncryptedVault = encryptedVault;
                status.IsInitialized = true;
            }
            status.VaultFilePath = path;

            return status;
        }

        public VaultStatus InitializeVault(VaultStatus vaultToInitialize, string password)
        {
            vaultToInitialize.VaultPassword = password;
            EncryptVault(vaultToInitialize);
            File.WriteAllText(vaultToInitialize.VaultFilePath, vaultToInitialize.EncryptedVault);
            vaultToInitialize.IsInitialized = true;
            return vaultToInitialize;
        }

        public VaultStatus DecryptVault(VaultStatus vaultToDecrypt, string password)
        {
            var decryptedVaultContents = AESThenHMAC.SimpleDecryptWithPassword(vaultToDecrypt.EncryptedVault, password);
            vaultToDecrypt.Vault = JsonConvert.DeserializeObject<SecretVault>(decryptedVaultContents);
            vaultToDecrypt.VaultPassword = password;
            vaultToDecrypt.IsUnlocked = true;
            return vaultToDecrypt;
        }

        public void EncryptVault(VaultStatus vaultToSave)
        {
            var contentToEncrypt = JsonConvert.SerializeObject(vaultToSave.Vault);
            vaultToSave.EncryptedVault = AESThenHMAC.SimpleEncryptWithPassword(contentToEncrypt, vaultToSave.VaultPassword);
        }
    }
}
