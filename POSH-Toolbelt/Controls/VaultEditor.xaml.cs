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

            if(!_Vault.IsUnlocked || !_Vault.IsInitialized)
            {
                // If we have not unlocked the vault, it cannot be modified.
                NewSecretButton.IsEnabled = false;
            }
        }

        public VaultStatus GetCurrentVaultStatus()
        {
            return _Vault;
        }

        public override string GetCurrentFileContents()
        {
            var currentSecrets = GetSecretsFromUI();
            _Vault.Vault.Secrets = currentSecrets;
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
            var newSecretGrid = GetGridForSecret(new KeyValuePair<string, string>("MySecret", ""));
            SecretList.Children.Add(newSecretGrid);
            VaultModified();
        }

        public void VaultInitialized()
        {
            VaultModified();
            NewSecretButton.IsEnabled = true;
        }

        public void VaultUnlocked()
        {
            BuildUIFromVaultContents(_Vault.Vault);
            NewSecretButton.IsEnabled = true;
        }

        private Grid GetGridForSecret(KeyValuePair<string, string> secret)
        {
            var newSecretGrid = new Grid();
            newSecretGrid.Visibility = Visibility.Visible;
            newSecretGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newSecretGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newSecretGrid.RowDefinitions.Add(new RowDefinition());

            var variableName = new TextBox();
            variableName.Text = secret.Key;
            newSecretGrid.Children.Add(variableName);
            Grid.SetColumn(variableName, 0);

            var friendlyName = new PasswordBox();
            friendlyName.Password = secret.Value;
            newSecretGrid.Children.Add(friendlyName);
            Grid.SetColumn(friendlyName, 1);

            return newSecretGrid;
        }

        private void BuildUIFromVaultContents(SecretVault vault)
        {
            SecretList.Children.Clear();
            foreach (var kvp in vault.Secrets)
            {
                var gridForSecret = GetGridForSecret(kvp);
                SecretList.Children.Add(gridForSecret);
            }
        }

        private Dictionary<string, string> GetSecretsFromUI()
        {
            var secrets = new Dictionary<string, string>();
            foreach (var child in SecretList.Children)
            {
                var grid = child as Grid;
                if (grid != null)
                {
                    var secretName = ((TextBox)grid.Children[0]).Text;
                    var secretValue = ((PasswordBox)grid.Children[1]).Password;
                    secrets.Add(secretName, secretValue);
                }
            }
            return secrets;
        }

        private void VaultModified()
        {
            OpenFileService.FileTextChanged(_Vault.VaultFilePath);
        }
    }
}
