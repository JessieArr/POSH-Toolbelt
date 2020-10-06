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
using System.Text.RegularExpressions;
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
            type.DisplayMemberPath = "Value";
            type.SelectedValuePath = "Key";
            var availableTypes = TypeService.GetAvailableTypes().ToDictionary(x => x.ID, x => x.Name);
            type.ItemsSource = availableTypes;
            type.SelectedValue = input.TypeID;
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
            newInputGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newInputGrid.RowDefinitions.Add(new RowDefinition());

            var friendlyName = new Label();
            friendlyName.Content = input.FriendlyName;
            newInputGrid.Children.Add(friendlyName);
            Grid.SetColumn(friendlyName, 0);

            var type = TypeService.GetAvailableTypes().First(x => x.ID == input.TypeID);
            if (!type.HasMultipleValues)
            {
                var friendlyNameInput = new TextBox();
                newInputGrid.Children.Add(friendlyNameInput);
                Grid.SetColumn(friendlyNameInput, 1);
            }
            else
            {
                var listDropDown = new ComboBox();
                if(type.ListValues.Count == 1)
                {
                    listDropDown.ItemsSource = type.ListValues.First().Value;
                }
                else
                {
                    var items = new List<Item>();
                    foreach(var listSection in type.ListValues)
                    {
                        foreach(var element in listSection.Value)
                        {
                            items.Add(new Item(element, listSection.Key));
                        }
                    }

                    var listCollection = new ListCollectionView(items);
                    listCollection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                    listDropDown.ItemsSource = listCollection;
                }
                newInputGrid.Children.Add(listDropDown);
                Grid.SetColumn(listDropDown, 1);
            }

            var id = new Label();
            id.Content = input.TypeID;
            id.Visibility = Visibility.Collapsed;
            newInputGrid.Children.Add(id);
            Grid.SetColumn(id, 2);

            return newInputGrid;
        }

        public class Item
        {
            public string Name { get; set; }
            public string Category { get; set; }

            public Item(string name, string category)
            {
                Name = name;
                Category = category;
            }
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
                    var typeName = ((KeyValuePair<Guid, string>)((ComboBox)grid.Children[3]).SelectedItem).Value;
                    var typeId = ((KeyValuePair<Guid, string>)((ComboBox)grid.Children[3]).SelectedItem).Key;
                    var optional = ((CheckBox)grid.Children[4]).IsChecked;
                    inputs.Add(new SnippetInput()
                    {
                        VariableName = variableName,
                        FriendlyName = friendlyName,
                        Description = description,
                        TypeName = typeName,
                        TypeID = typeId,
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
            if (!AreRunInputsValid())
            {
                return;
            }
            var inputs = GetRunInputsFromUI();
            var command = "";
            foreach (var input in inputs)
            {
                command += $"{input.Key} = \"{input.Value}\"" + Environment.NewLine;
            }
            command += Environment.NewLine + ScriptEditor.Text;

            //var window = new ConsoleWindow(command);
            //window.Show();
            var psService = new PowershellService();
            psService.OpenPSWindowAndRunScript(command);
        }

        private bool AreRunInputsValid()
        {
            var availableTypes = TypeService.GetAvailableTypes();
            foreach (var child in RunInputs.Children)
            {
                var grid = child as Grid;
                if (grid != null)
                {
                    var typeID = ((Label)grid.Children[2]).Content.ToString();
                    var thisType = availableTypes.First(x => x.ID == Guid.Parse(typeID));
                    
                    if(!thisType.HasMultipleValues)
                    {
                        var value = ((TextBox)grid.Children[1]).Text;

                        if (!String.IsNullOrEmpty(thisType.Regex))
                        {
                            var regex = new Regex(thisType.Regex);
                            if (!regex.IsMatch(value))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }                    
                }
            }
            return true;
        }

        private Dictionary<string, string> GetRunInputsFromUI()
        {
            var output = new Dictionary<string, string>();
            var availableTypes = TypeService.GetAvailableTypes();
            var inputs = GetInputsFromUI();
            foreach (var child in RunInputs.Children)
            {
                var grid = child as Grid;
                if (grid != null)
                {
                    var typeID = ((Label)grid.Children[2]).Content.ToString();
                    var thisType = availableTypes.First(x => x.ID == Guid.Parse(typeID));

                    if(thisType.ID.ToString() == TypeService.SecretTypeID)
                    {
                        var variableFriendlyName = ((Label)grid.Children[0]).Content as String;
                        var value = ((ComboBox)grid.Children[1]).SelectedValue.ToString();
                        var variableName = inputs.First(x => x.FriendlyName == variableFriendlyName).VariableName;

                        var vaults = OpenFileService.GetOpenVaults();
                        foreach(var vault in vaults)
                        {
                            // This is a bug - we need to use the secret's ID to look it up later
                            // once support for multiple vaults is added.
                            if (vault.Vault.Secrets.Any(x => x.Key == value))
                            {
                                var secret = vault.Vault.Secrets.First(x => x.Key == value);
                                output.Add(variableName, secret.Value);
                            }
                        }

                        continue;
                    }

                    if(!thisType.HasMultipleValues)
                    {
                        var variableFriendlyName = ((Label)grid.Children[0]).Content as String;
                        var value = ((TextBox)grid.Children[1]).Text;
                        var variableName = inputs.First(x => x.FriendlyName == variableFriendlyName).VariableName;
                        output.Add(variableName, value);
                    }
                    else
                    {
                        var variableFriendlyName = ((Label)grid.Children[0]).Content as String;
                        var value = ((ComboBox)grid.Children[1]).SelectedValue.ToString();
                        var variableName = inputs.First(x => x.FriendlyName == variableFriendlyName).VariableName;
                        output.Add(variableName, value);
                    }                    
                }
            }
            return output;
        }
    }
}
