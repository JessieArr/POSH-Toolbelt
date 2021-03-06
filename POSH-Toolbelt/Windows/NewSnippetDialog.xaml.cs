﻿using Newtonsoft.Json;
using POSH_Toolbelt.Constants;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace POSH_Toolbelt.Windows
{
    /// <summary>
    /// Interaction logic for NewSnippetDialog.xaml
    /// </summary>
    public partial class NewSnippetDialog : Window
    {
        private string _Path { get; set; }
        public NewSnippetDialog(string path)
        {
            InitializeComponent();
            _Path = path;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = NewSnippetName.Text;
            if (!name.EndsWith(FileExtensions.SnippetExtension))
            {
                name = name + FileExtensions.SnippetExtension;
            }

            var newSnippet = new POSHToolbeltSnippet();
            newSnippet.Name = name.Substring(0, name.LastIndexOf(FileExtensions.SnippetExtension));

            var fileText = JsonConvert.SerializeObject(newSnippet, Formatting.Indented);

            var fileAttributes = File.GetAttributes(_Path);

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                File.WriteAllText(Path.Combine(_Path, name), fileText);
            }
            else
            {
                var directory = Path.GetDirectoryName(_Path);
                File.WriteAllText(Path.Combine(directory, name), fileText);
            }

            FileBrowserService.RefreshTreeView();
            Close();
        }
    }
}
