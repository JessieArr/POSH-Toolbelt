using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.FileFormats
{
    public class POSHType
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Regex { get; set; }
        public bool EmitQuotes { get; set; }
    }

    public static class POSHTypeList
    {
        public static string Regex = "Regex";
        public static string List = "List";
    }
}
