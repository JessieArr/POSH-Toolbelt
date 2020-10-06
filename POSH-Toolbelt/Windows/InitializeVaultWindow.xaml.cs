using POSH_Toolbelt.Controls;
using POSH_Toolbelt.Services.Vault;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POSH_Toolbelt.Windows
{
    /// <summary>
    /// Interaction logic for InitializeVaultWindow.xaml
    /// </summary>
    public partial class InitializeVaultWindow : Window
    {
        private VaultStatus _VaultStatus;
        private VaultEditor _Editor;
        public InitializeVaultWindow(VaultStatus vaultStatus, VaultEditor editor)
        {
            InitializeComponent();
            _VaultStatus = vaultStatus;
            _Editor = editor;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var firstPassword = FirstPassword.Password;
            var secondPassword = SecondPassword.Password;
            if(firstPassword == secondPassword)
            {
                _VaultStatus.VaultPassword = firstPassword;
                _Editor.VaultInitialized();
                Close();
            }
            else
            {
                ErrorText.Content = "Passwords did not match!";
            }            
        }
    }
}
