using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.Services.Vault
{
    public class VaultStatus
    {
        public bool IsInitialized { get; set; }
        public bool IsUnlocked { get; set; }
        public string VaultFilePath { get; set; }
        public string VaultPassword { get; set; }
        public SecretVault Vault { get; set; } = new SecretVault();
        public string EncryptedVault { get; set; }
    }
}
