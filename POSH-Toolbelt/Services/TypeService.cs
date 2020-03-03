using POSH_Toolbelt.FileFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.Services
{
    public static class TypeService
    {
        private static List<POSHType> DefaultTypes = new List<POSHType>()
        {
            new POSHType()
            {
                ID = Guid.Parse("a7467ec5-f159-4a39-8ff2-66cc3144d4c6"),
                Name = "String",
                EmitQuotes = true,
                Regex = ".+",
            },
            new POSHType()
            {
                ID = Guid.Parse("ea6ecd6e-3628-4197-8453-02300c0cb086"),
                Name = "Integer",
                EmitQuotes = false,
                Regex = "[0-9]+",
            },
        };

        public static List<POSHType> GetAvailableTypes()
        {
            return DefaultTypes;
        }
    }
}
