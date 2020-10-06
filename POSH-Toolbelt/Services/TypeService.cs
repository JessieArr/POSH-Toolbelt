using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Services.Vault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSH_Toolbelt.Services
{
    public static class TypeService
    {
        public static string SecretTypeID = "944f36d7-138f-4200-a021-b39f615064eb";

        private static List<POSHToolbeltType> DefaultTypes = new List<POSHToolbeltType>()
        {
            new POSHToolbeltType()
            {
                ID = Guid.Parse("a7467ec5-f159-4a39-8ff2-66cc3144d4c6"),
                Name = "String",
                EmitQuotes = true,
                Regex = ".+",
            },
            new SecretType()
            {
                ID = Guid.Parse(SecretTypeID),
                Name = "Secret",
                EmitQuotes = true,
                HasMultipleValues = true,
                Regex = ".+",
            },
            new POSHToolbeltType()
            {
                ID = Guid.Parse("ea6ecd6e-3628-4197-8453-02300c0cb086"),
                Name = "Integer",
                EmitQuotes = false,
                Regex = "[0-9]+",
            },
        };

        public static List<POSHToolbeltType> CustomTypes = new List<POSHToolbeltType>();

        public static List<POSHToolbeltType> GetAvailableTypes()
        {
            return DefaultTypes.Concat(CustomTypes).ToList();
        }
    }
}
