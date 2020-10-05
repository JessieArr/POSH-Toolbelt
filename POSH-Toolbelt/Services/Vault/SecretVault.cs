using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.Services.Vault
{
    public class SecretVault
    {
        public Dictionary<string, string> Secrets { get; set; } = new Dictionary<string, string>();
    }
}
