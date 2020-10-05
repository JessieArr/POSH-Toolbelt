using POSH_Toolbelt.Services;
using POSH_Toolbelt.Services.Vault;
using POSH_Toolbelt.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSH_Toolbelt.Controls
{
    /// <summary>
    /// Interaction logic for VaultEditor.xaml
    /// </summary>
    public partial class VaultEditor : FileEditor
    {
        private VaultService _VaultService;
        private VaultStatus _Vault;
        public VaultEditor(string filePath)
        {
            InitializeComponent();
            _VaultService = new VaultService();
            _Vault = _VaultService.OpenVaultFile(filePath);
            if (_Vault.IsInitialized)
            {
                InitializeButton.Visibility = Visibility.Hidden;
            }
            if (_Vault.IsUnlocked || !_Vault.IsInitialized)
            {
                UnlockButton.Visibility = Visibility.Hidden;
            }
        }

        public override string GetCurrentFileContents()
        {
            _VaultService.EncryptVault(_Vault);
            return _Vault.EncryptedVault;
        }

        private void InitializeButton_Click(object sender, RoutedEventArgs e)
        {
            var initializeWindow = new InitializeVaultWindow(_Vault, this);
            initializeWindow.Show();
        }

        private void UnlockButton_Click(object sender, RoutedEventArgs e)
        {
            var initializeWindow = new UnlockVaultWindow(_Vault, this);
            initializeWindow.Show();
        }

        private void NewSecretButton_Click(object sender, RoutedEventArgs e)
        {
            var newSecretStack = new StackPanel();
            newSecretStack.Orientation = Orientation.Horizontal;
            newSecretStack.Children.Add(new Label() { Content = "Key" });
            newSecretStack.Children.Add(new Label() { Content = "Value" });
            SecretList.Children.Add(newSecretStack);
        }

        public void VaultInitialized()
        {
            OpenFileService.FileTextChanged(_Vault.VaultFilePath);
        }

        public void VaultUnlocked()
        {
        }
    }
}
