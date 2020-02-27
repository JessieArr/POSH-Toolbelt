using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace POSH_Toolbelt.Services
{
    public abstract class FileEditor : UserControl
    {
        public abstract string GetCurrentFileContents();
    }
}
