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
    /// Interaction logic for NewFolderDialog.xaml
    /// </summary>
    public partial class NewFolderDialog : Window
    {
        private string _Path;

        public NewFolderDialog(string path)
        {
            InitializeComponent();
            _Path = path;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var directoryName = NewFolderName.Text;
            Directory.CreateDirectory(Path.Join(_Path, directoryName));
            FileBrowserService.RefreshTreeView();
            Close();
        }
    }
}
