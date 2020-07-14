using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.FileFormats
{
    public class POSHToolbeltType
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Regex { get; set; }
        public bool EmitQuotes { get; set; }
        public bool HasMultipleValues { get; set; }
        public List<string> ListValues { get; set; } = new List<string>();
    }

    public static class POSHTypeList
    {
        public static string Regex = "Regex";
        public static string List = "List";
    }
}
