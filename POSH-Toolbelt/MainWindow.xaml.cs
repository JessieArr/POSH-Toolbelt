using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using POSH_Toolbelt.Services;
using POSH_Toolbelt.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace POSH_Toolbelt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowHelper.MainWindow = this;

            var historyService = new ApplicationHistoryService();
            var history = historyService.GetApplicationHistory();
            if(!String.IsNullOrEmpty(history.MostRecentOpenedProject))
            {
                if(File.Exists(history.MostRecentOpenedProject))
                {
                    FolderTree.Items.Add(FileBrowserService.Open(history.MostRecentOpenedProject));
                }
            }
        }

        public void SetEditorControl(FileEditor editorControl)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(editorControl);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Title = "Select Folder";
            dialog.IsFolderPicker = true;
            dialog.AddToMostRecentlyUsedList = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.EnsureFileExists = true;
            dialog.EnsurePathExists = true;
            dialog.EnsureReadOnly = false;
            dialog.EnsureValidNames = true;
            dialog.Multiselect = false;
            dialog.ShowPlacesList = true;

            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var folderPath = dialog.FileName;
                var dialogWindow = new NewProjectDialog(folderPath);
                dialogWindow.Show();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            OpenFileService.SaveAllOpenFiles();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Title = "My Title";
            dialog.Filters.Add(new CommonFileDialogFilter("POSH Toolbelt Project", ".ptproj"));
            dialog.AddToMostRecentlyUsedList = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.EnsureFileExists = true;
            dialog.EnsurePathExists = true;
            dialog.EnsureReadOnly = false;
            dialog.EnsureValidNames = true;
            dialog.Multiselect = false;
            dialog.ShowPlacesList = true;

            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var projectPath = dialog.FileName;
                FolderTree.Items.Add(FileBrowserService.Open(projectPath));

                var historyService = new ApplicationHistoryService();
                var history = historyService.GetApplicationHistory();
                history.MostRecentOpenedProject = projectPath;
                historyService.SaveApplicationHistory(history);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }        
    }
}
