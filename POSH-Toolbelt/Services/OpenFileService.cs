﻿using POSH_Toolbelt.Constants;
using POSH_Toolbelt.Controls;
using POSH_Toolbelt.Models;
using POSH_Toolbelt.Services.Vault;
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
                var editor = GetEditorForFile(filePathToOpen);
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
            foreach (var file in _OpenFiles)
            {
                if (file.Value.IsDirty)
                {
                    var currentValue = file.Value.Editor.GetCurrentFileContents();
                    file.Value.IsDirty = false;
                    (new FileInfo(file.Value.FilePath)).Directory.Create();
                    File.WriteAllText(file.Value.FilePath, currentValue);
                }
            }
        }

        public static List<VaultStatus> GetOpenVaults()
        {
            var vaults = new List<VaultStatus>();
            foreach(var file in _OpenFiles)
            {
                var vaultEditor = file.Value.Editor as VaultEditor;
                if (vaultEditor != null)
                {
                    vaults.Add(vaultEditor.GetCurrentVaultStatus());
                }
            }
            return vaults;
        }

        private static FileEditor GetEditorForFile(string path)
        {
            if (path.EndsWith(FileExtensions.PowershellScriptExtension, StringComparison.OrdinalIgnoreCase))
            {
                return new PowershellScriptEditor(path);
            }
            if (path.EndsWith(FileExtensions.SnippetExtension, StringComparison.OrdinalIgnoreCase))
            {
                return new SnippetEditor(path);
            }
            if (path.EndsWith(FileExtensions.TypeExtension, StringComparison.OrdinalIgnoreCase))
            {
                return new TypeEditor(path);
            }
            if (path.EndsWith(FileExtensions.VaultExtension, StringComparison.OrdinalIgnoreCase))
            {
                return new VaultEditor(path);
            }
            return new PowershellScriptEditor(path);
        }
    }
}
