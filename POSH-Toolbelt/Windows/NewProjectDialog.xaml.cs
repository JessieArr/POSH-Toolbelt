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
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        private string _Path;
        public NewProjectDialog(string path)
        {
            InitializeComponent();
            _Path = path;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = NewProjectName.Text;
            if(!name.EndsWith(".ptproj"))
            {
                name = name + ".ptproj";
            }

            var newProject = new POSHToolbeltProject();
            newProject.Name = name.Substring(0, name.LastIndexOf(".ptproj"));

            var newFilePath = Path.Combine(_Path, name);
            File.WriteAllText(newFilePath, "");
            
            var rootNode = FileBrowserService.Open(newFilePath);
            MainWindowHelper.SetRootTreeNode(rootNode);

            Close();
        }
    }
}
