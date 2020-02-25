using POSH_Toolbelt.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace POSH_Toolbelt.Services
{
    public static class FileBrowserService
    {
        public static TreeViewItem RootNode { get; set; }
        public static string ProjectPath { get; set; }

        public static void RefreshTreeView()
        {
            RootNode.Items.Clear();

            var projectDirectory = Path.GetDirectoryName(ProjectPath);
            RecursivelyBuildFolderTree(RootNode.Items, projectDirectory);
        }

        public static TreeViewItem Open(string projectPath)
        {
            ProjectPath = projectPath;
            var projectName = Path.GetFileName(projectPath);

            var rootTreeItem = new TreeViewItem();
            rootTreeItem.Header = projectName;
            rootTreeItem.IsExpanded = true;
            RootNode = rootTreeItem;
            RefreshTreeView();
            return rootTreeItem;
        }

        private static void RecursivelyBuildFolderTree(ItemCollection collection, string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            var directories = directoryInfo.GetDirectories();
            foreach (var directory in directories)
            {
                var directoryItem = new TreeViewItem();
                directoryItem.Header = directory.Name;
                directoryItem.ContextMenu = new ContextMenu();
                var createItem = new MenuItem();
                createItem.Header = "Create Snippet";
                createItem.Click += (sender, args) =>
                {
                    var dialog = new NewSnippetDialog(directory.FullName);
                    dialog.Show();
                };
                directoryItem.ContextMenu.Items.Add(createItem);
                collection.Add(directoryItem);
                RecursivelyBuildFolderTree(directoryItem.Items, directory.FullName);
            }

            var powershellScriptFiles = directoryInfo.GetFiles("*.ps1");
            foreach (var file in powershellScriptFiles)
            {
                var newItem = new TreeViewItem();
                newItem.Header = file.Name;
                newItem.MouseDoubleClick += (sender, args) =>
                {
                    var fileText = File.ReadAllText(file.FullName);
                    MainWindowHelper.SetScriptText(fileText);
                };
                collection.Add(newItem);
            }

            var poshToolbeltSnippetFiles = directoryInfo.GetFiles("*.ptsnip");
            foreach (var file in poshToolbeltSnippetFiles)
            {
                var newItem = new TreeViewItem();
                newItem.Header = file.Name;
                newItem.MouseDoubleClick += (sender, args) =>
                {
                    var fileText = File.ReadAllText(file.FullName);
                    MainWindowHelper.SetScriptText(fileText);
                };
                collection.Add(newItem);
            }
        }
    }
}
