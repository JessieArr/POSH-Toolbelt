using POSH_Toolbelt.FileFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSH_Toolbelt.Services.Vault
{
    public class SecretType : POSHToolbeltType
    {
        public override Dictionary<string, List<string>> ListValues
        {
            get
            {
                var vaultService = new VaultService();
                var vaultValues = new Dictionary<string, List<string>>();
                var vaults = OpenFileService.GetOpenVaults();
                var unlockedVaults = vaults.Where(x => x.IsUnlocked);
                var projectVaultCount = 1;
                foreach (var vault in unlockedVaults)
                {
                    if (vault.VaultFilePath == vaultService.GetUserVaultPath())
                    {
                        vaultValues.Add("User Vault", vault.Vault.Secrets.Select(x => x.Key).ToList());
                    }
                    else
                    {
                        vaultValues.Add("Project Vault " + projectVaultCount, vault.Vault.Secrets.Select(x => x.Key).ToList());
                        projectVaultCount++;
                    }
                }
                return vaultValues;
            }
            set
            {
            }
        }
    }
}
