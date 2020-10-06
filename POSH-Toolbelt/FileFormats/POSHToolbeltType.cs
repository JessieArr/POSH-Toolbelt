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
        public virtual Dictionary<string, List<string>> ListValues { get; set; } 
            = new Dictionary<string, List<string>>();
    }
}
