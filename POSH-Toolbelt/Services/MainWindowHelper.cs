using POSH_Toolbelt.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace POSH_Toolbelt.Services
{
    public static class MainWindowHelper
    {
        public static MainWindow MainWindow { get; set; }
        public static void SetFileToBeEdited(FileEditor editor)
        {
            MainWindow.SetEditorControl(editor);
        }

        public static void SetRootTreeNode(TreeViewItem item)
        {
            MainWindow.FolderTree.Items.Clear();
            MainWindow.FolderTree.Items.Add(item);
        }
    }
}
