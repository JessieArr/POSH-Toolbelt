using Newtonsoft.Json;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Services;
using POSH_Toolbelt.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSH_Toolbelt.Controls
{
    /// <summary>
    /// Interaction logic for SnippetEditor.xaml
    /// </summary>
    public partial class SnippetEditor : FileEditor
    {
        private string _FilePath;
        private POSHToolbeltSnippet _OriginalSnippet;

        public SnippetEditor(string filePath)
        {
            InitializeComponent();
            _FilePath = filePath;
            var fileContents = File.ReadAllText(filePath);
            _OriginalSnippet = JsonConvert.DeserializeObject<POSHToolbeltSnippet>(fileContents);

            ScriptEditor.Text = _OriginalSnippet.Script;
            AddHeaderGrid();
        }

        public override string GetCurrentFileContents()
        {
            _OriginalSnippet.Script = ScriptEditor.Text;
            return JsonConvert.SerializeObject(_OriginalSnippet, Formatting.Indented);
        }

        private void ScriptEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpenFileService.FileTextChanged(_FilePath);
        }

        private void NewInput_Click(object sender, RoutedEventArgs e)
        {
            AddVariableGrid();
        }

        private void AddHeaderGrid()
        {
            var newHeaderGrid = new Grid();
            newHeaderGrid.Visibility = Visibility.Visible;
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.RowDefinitions.Add(new RowDefinition());

            var variableNameLabel = new Label();
            variableNameLabel.Content = "Variable Name";
            newHeaderGrid.Children.Add(variableNameLabel);
            Grid.SetColumn(variableNameLabel, 0);

            var nameLabel = new Label();
            nameLabel.Content = "Friendly Name";
            newHeaderGrid.Children.Add(nameLabel);
            Grid.SetColumn(nameLabel, 1);

            var descriptionLabel = new Label();
            descriptionLabel.Content = "Description";
            newHeaderGrid.Children.Add(descriptionLabel);
            Grid.SetColumn(descriptionLabel, 2);

            var typeLabel = new Label();
            typeLabel.Content = "Type";
            newHeaderGrid.Children.Add(typeLabel);
            Grid.SetColumn(typeLabel, 3);

            var optionalLabel = new Label();
            optionalLabel.Content = "Optional?";
            newHeaderGrid.Children.Add(optionalLabel);
            Grid.SetColumn(optionalLabel, 4);

            InputStack.Children.Add(newHeaderGrid);
        }

        private void AddVariableGrid()
        {
            var newHeaderGrid = new Grid();
            newHeaderGrid.Visibility = Visibility.Visible;
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newHeaderGrid.RowDefinitions.Add(new RowDefinition());

            var variableName = new TextBox();
            variableName.Text = "$MyVariable";
            newHeaderGrid.Children.Add(variableName);
            Grid.SetColumn(variableName, 0);

            var friendlyName = new TextBox();
            friendlyName.Text = "Friendly Name";
            newHeaderGrid.Children.Add(friendlyName);
            Grid.SetColumn(friendlyName, 1);

            var description = new TextBox();
            description.Text = "Description";
            newHeaderGrid.Children.Add(description);
            Grid.SetColumn(description, 2);

            var type = new ComboBox();
            type.ItemsSource = new List<string>()
            {
                "String",
                "Integer"
            };
            newHeaderGrid.Children.Add(type);
            Grid.SetColumn(type, 3);

            var optional = new CheckBox();
            optional.IsChecked = false;
            newHeaderGrid.Children.Add(optional);
            Grid.SetColumn(optional, 4);

            InputStack.Children.Add(newHeaderGrid);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConsoleWindow(ScriptEditor.Text);
            window.Show();
        }
    }
}
