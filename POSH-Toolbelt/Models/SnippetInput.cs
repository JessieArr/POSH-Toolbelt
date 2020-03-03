using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.Models
{
    public class SnippetInput
    {
        public string FriendlyName { get; set; }
        public string VariableName { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public Guid TypeID { get; set; }
        public bool IsOptional { get; set; }
    }
}
