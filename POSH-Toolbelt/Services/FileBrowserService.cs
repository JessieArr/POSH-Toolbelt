using Newtonsoft.Json;
using POSH_Toolbelt.Controls;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Models;
using POSH_Toolbelt.Services.Vault;
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
            var vaultService = new VaultService();
            var userVaultItem = new TreeViewItem();
            userVaultItem.Header = "User Vault";
            userVaultItem.MouseDoubleClick += (sender, args) =>
            {
                OpenFileService.OpenFile(vaultService.GetUserVaultPath());
            };
            RootNode.Items.Add(userVaultItem);
            RecursivelyBuildFolderTree(RootNode.Items, projectDirectory);
        }

        public static TreeViewItem Open(string projectPath)
        {
            ProjectPath = projectPath;
            var projectName = Path.GetFileName(projectPath);

            var rootTreeItem = new TreeViewItem();
            rootTreeItem.Header = projectName;
            rootTreeItem.IsExpanded = true;
            AddCreateSnippetToTreeViewItem(rootTreeItem, projectPath);
            AddCreateListToTreeViewItem(rootTreeItem, projectPath);
            AddCreateFolderToTreeViewItem(rootTreeItem, Path.GetDirectoryName(projectPath));
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
                AddCreateSnippetToTreeViewItem(directoryItem, directory.FullName);
                AddCreateListToTreeViewItem(directoryItem, directory.FullName);
                AddCreateFolderToTreeViewItem(directoryItem, directory.FullName);
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
                    OpenFileService.OpenFile(file.FullName);
                };
                AddDeleteToTreeViewItem(newItem, file.FullName);
                collection.Add(newItem);
            }

            var poshToolbeltSnippetFiles = directoryInfo.GetFiles("*.ptsnip");
            foreach (var file in poshToolbeltSnippetFiles)
            {
                var newItem = new TreeViewItem();
                newItem.Header = file.Name;                
                newItem.MouseDoubleClick += (sender, args) =>
                {
                    OpenFileService.OpenFile(file.FullName);
                };
                AddDeleteToTreeViewItem(newItem, file.FullName);
                collection.Add(newItem);
            }

            var poshToolbeltTypeFiles = directoryInfo.GetFiles("*.pttype");
            foreach (var file in poshToolbeltTypeFiles)
            {
                var newItem = new TreeViewItem();
                newItem.Header = file.Name;
                newItem.MouseDoubleClick += (sender, args) =>
                {
                    OpenFileService.OpenFile(file.FullName);
                };
                AddDeleteToTreeViewItem(newItem, file.FullName);

                var text = File.ReadAllText(file.FullName);
                var type = JsonConvert.DeserializeObject<POSHToolbeltType>(text);
                TypeService.CustomTypes.Add(type);

                collection.Add(newItem);
            }
        }

        private static void AddCreateSnippetToTreeViewItem(TreeViewItem treeViewItem, string path)
        {
            if(treeViewItem.ContextMenu == null)
            {
                treeViewItem.ContextMenu = new ContextMenu();
            }
            var createItem = new MenuItem();
            createItem.Header = "Create Snippet";
            createItem.Click += (sender, args) =>
            {
                var dialog = new NewSnippetDialog(path);
                dialog.Show();
            };
            treeViewItem.ContextMenu.Items.Add(createItem);
        }

        private static void AddCreateListToTreeViewItem(TreeViewItem treeViewItem, string path)
        {
            if (treeViewItem.ContextMenu == null)
            {
                treeViewItem.ContextMenu = new ContextMenu();
            }
            var createItem = new MenuItem();
            createItem.Header = "Create List";
            createItem.Click += (sender, args) =>
            {
                var dialog = new NewListDialog(path);
                dialog.Show();
            };
            treeViewItem.ContextMenu.Items.Add(createItem);
        }

        private static void AddCreateFolderToTreeViewItem(TreeViewItem treeViewItem, string path)
        {
            if (treeViewItem.ContextMenu == null)
            {
                treeViewItem.ContextMenu = new ContextMenu();
            }
            var createItem = new MenuItem();
            createItem.Header = "New Folder";
            createItem.Click += (sender, args) =>
            {
                var dialog = new NewFolderDialog(path);
                dialog.Show();
            };
            treeViewItem.ContextMenu.Items.Add(createItem);
        }

        private static void AddDeleteToTreeViewItem(TreeViewItem treeViewItem, string path)
        {
            if (treeViewItem.ContextMenu == null)
            {
                treeViewItem.ContextMenu = new ContextMenu();
            }
            var createItem = new MenuItem();
            createItem.Header = "Delete";
            createItem.Click += (sender, args) =>
            {
                File.Delete(path);
                RefreshTreeView();
            };
            treeViewItem.ContextMenu.Items.Add(createItem);
        }
    }
}
