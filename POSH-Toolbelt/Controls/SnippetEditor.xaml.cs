using Newtonsoft.Json;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Models;
using POSH_Toolbelt.Services;
using POSH_Toolbelt.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            SetUIValuesForSnippet(_OriginalSnippet);
        }

        private void SetUIValuesForSnippet(POSHToolbeltSnippet originalSnippet)
        {
            ScriptEditor.Text = originalSnippet.Script;
            foreach (var input in originalSnippet.Inputs)
            {
                var grid = GetGridForInput(input);
                InputStack.Children.Add(grid);
            }
        }

        public override string GetCurrentFileContents()
        {
            var newSnippet = new POSHToolbeltSnippet();
            newSnippet.ID = _OriginalSnippet.ID;
            newSnippet.Script = ScriptEditor.Text;
            newSnippet.Inputs = GetInputsFromUI();
            return JsonConvert.SerializeObject(newSnippet, Formatting.Indented);
        }

        private void ScriptEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpenFileService.FileTextChanged(_FilePath);
        }

        private void NewInput_Click(object sender, RoutedEventArgs e)
        {
            InputStack.Children.Add(GetGridForInput(new SnippetInput()
            {
                VariableName = "$MyVariable",
                FriendlyName = "Friendly Name",
                Description = "Description",
                Type = "",
                IsOptional = false,
            }));
        }

        private Grid GetGridForInput(SnippetInput input)
        {
            var newInputGrid = new Grid();
            newInputGrid.Visibility = Visibility.Visible;
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.RowDefinitions.Add(new RowDefinition());

            var variableName = new TextBox();
            variableName.Text = input.VariableName;
            newInputGrid.Children.Add(variableName);
            Grid.SetColumn(variableName, 0);

            var friendlyName = new TextBox();
            friendlyName.Text = input.FriendlyName;
            newInputGrid.Children.Add(friendlyName);
            Grid.SetColumn(friendlyName, 1);

            var description = new TextBox();
            description.Text = input.Description;
            newInputGrid.Children.Add(description);
            Grid.SetColumn(description, 2);

            var type = new ComboBox();
            type.ItemsSource = new List<string>()
            {
                "String",
                "Integer"
            };
            type.SelectedItem = input.Type;
            newInputGrid.Children.Add(type);
            Grid.SetColumn(type, 3);

            var optional = new CheckBox();
            optional.IsChecked = input.IsOptional;
            newInputGrid.Children.Add(optional);
            Grid.SetColumn(optional, 4);

            var remove = new Button();
            remove.Content = "Remove";
            remove.Click += (sender, args) =>
            {
                InputStack.Children.Remove(newInputGrid);
            };
            newInputGrid.Children.Add(remove);
            Grid.SetColumn(remove, 5);

            return newInputGrid;
        }

        private Grid GetRunInputGridForInput(SnippetInput input)
        {
            var newInputGrid = new Grid();
            newInputGrid.Visibility = Visibility.Visible;
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.RowDefinitions.Add(new RowDefinition());

            var friendlyName = new Label();
            friendlyName.Content = input.FriendlyName;
            newInputGrid.Children.Add(friendlyName);
            Grid.SetColumn(friendlyName, 0);

            var friendlyNameInput = new TextBox();
            newInputGrid.Children.Add(friendlyNameInput);
            Grid.SetColumn(friendlyNameInput, 1);

            return newInputGrid;
        }

        private List<SnippetInput> GetInputsFromUI()
        {
            var inputs = new List<SnippetInput>();
            foreach (var child in InputStack.Children)
            {
                var grid = child as Grid;
                if (grid != null)
                {
                    var variableName = ((TextBox)grid.Children[0]).Text;
                    var friendlyName = ((TextBox)grid.Children[1]).Text;
                    var description = ((TextBox)grid.Children[2]).Text;
                    var type = ((ComboBox)grid.Children[3]).Text;
                    var optional = ((CheckBox)grid.Children[4]).IsChecked;
                    inputs.Add(new SnippetInput()
                    {
                        VariableName = variableName,
                        FriendlyName = friendlyName,
                        Description = description,
                        Type = type,
                        IsOptional = optional.HasValue && optional.Value
                    });
                }
            }
            return inputs;
        }

        private void RunTab_Click(object sender, MouseButtonEventArgs e)
        {
            RunInputs.Children.Clear();
            var inputs = GetInputsFromUI();
            foreach (var input in inputs)
            {
                RunInputs.Children.Add(GetRunInputGridForInput(input));
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var inputs = GetRunInputsFromUI();
            var command = "";
            foreach(var input in inputs)
            {
                command += $"{input.Key} = \"{input.Value}\"" + Environment.NewLine;
            }
            command += Environment.NewLine + ScriptEditor.Text;
            var window = new ConsoleWindow(command);
            window.Show();
        }

        private Dictionary<string, string> GetRunInputsFromUI()
        {
            var output = new Dictionary<string, string>();
            var inputs = GetInputsFromUI();
            foreach (var child in RunInputs.Children)
            {
                var grid = child as Grid;
                if (grid != null)
                {
                    var variableFriendlyName = ((Label)grid.Children[0]).Content as String;
                    var value = ((TextBox)grid.Children[1]).Text;
                    var variableName = inputs.First(x => x.FriendlyName == variableFriendlyName).VariableName;
                    output.Add(variableName, value);
                }
            }
            return output;
        }
    }
}
