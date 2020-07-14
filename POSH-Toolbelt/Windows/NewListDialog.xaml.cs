using Newtonsoft.Json;
using POSH_Toolbelt.Constants;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace POSH_Toolbelt.Windows
{
    /// <summary>
    /// Interaction logic for NewListDialog.xaml
    /// </summary>
    public partial class NewListDialog : Window
    {
        private string _Path { get; set; }

        public NewListDialog(string path)
        {
            InitializeComponent();
            _Path = path;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = NewSnippetName.Text;
            if (!name.EndsWith(FileExtensions.TypeExtension))
            {
                name += FileExtensions.TypeExtension;
            }

            var newType = new POSHToolbeltType();
            newType.HasMultipleValues = true;
            newType.Name = name.Substring(0, name.LastIndexOf(FileExtensions.TypeExtension));

            var fileText = JsonConvert.SerializeObject(newType, Formatting.Indented);

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
