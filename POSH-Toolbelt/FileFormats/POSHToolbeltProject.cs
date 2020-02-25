using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.FileFormats
{
    public class POSHToolbeltProject
    {
        public POSHToolbeltProject()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
