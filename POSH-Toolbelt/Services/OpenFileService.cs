using POSH_Toolbelt.Controls;
using POSH_Toolbelt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POSH_Toolbelt.Services
{
    public static class OpenFileService
    {
        private static Dictionary<string, OpenEditorFile> _OpenFiles = new Dictionary<string, OpenEditorFile>();

        public static void OpenFile(string filePathToOpen)
        {
            if (!_OpenFiles.ContainsKey(filePathToOpen))
            {
                var editor = new PowershellScriptEditor(filePathToOpen);
                MainWindowHelper.SetFileToBeEdited(editor);
                _OpenFiles[filePathToOpen] = new OpenEditorFile(filePathToOpen, editor);
            }
            else
            {
                // File is already open.
                MainWindowHelper.SetFileToBeEdited(_OpenFiles[filePathToOpen].Editor);

            }
        }

        public static void FileTextChanged(string path)
        {
            if (_OpenFiles.ContainsKey(path))
            {
                _OpenFiles[path].IsDirty = true;
            }
        }

        public static void SaveAllOpenFiles()
        {
            foreach(var file in  _OpenFiles)
            {
                if(file.Value.IsDirty)
                {
                    var currentValue = file.Value.Editor.GetCurrentFileContents();
                    file.Value.IsDirty = false;
                    File.WriteAllText(file.Value.FilePath, currentValue);
                }
            }
        }
    }
}
