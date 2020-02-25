using POSH_Toolbelt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.FileFormats
{
    public class POSHToolbeltSnippet
    {
        public POSHToolbeltSnippet()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
        public List<SnippetInput> Inputs { get; set; } = new List<SnippetInput>();
        public List<SnippetOutput> Outputs { get; set; } = new List<SnippetOutput>();
        public List<SnippetPreset> Presets { get; set; } = new List<SnippetPreset>();
    }
}
