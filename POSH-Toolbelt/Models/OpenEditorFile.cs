using POSH_Toolbelt.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSH_Toolbelt.Models
{
    public class OpenEditorFile
    {
        public string FilePath { get; set; }
        public bool IsDirty { get; set; }
        public FileEditor Editor { get; set; }
        
        public OpenEditorFile(string path, FileEditor editor)
        {
            FilePath = path;
            Editor = editor;
        }
    }
}
