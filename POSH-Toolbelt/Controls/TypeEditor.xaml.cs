using Newtonsoft.Json;
using POSH_Toolbelt.FileFormats;
using POSH_Toolbelt.Services;
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
    /// Interaction logic for TypeEditor.xaml
    /// </summary>
    public partial class TypeEditor : FileEditor
    {
        private string _FilePath;
        private POSHToolbeltType _OriginalType;

        public TypeEditor(string filePath)
        {
            InitializeComponent();
            _FilePath = filePath;
            var fileContents = File.ReadAllText(filePath);
            _OriginalType = JsonConvert.DeserializeObject<POSHToolbeltType>(fileContents);
            SetUIValuesForType(_OriginalType);
        }

        private void SetUIValuesForType(POSHToolbeltType originalSnippet)
        {
            ListItems.Text = String.Join(Environment.NewLine, originalSnippet.ListValues);
        }

        public override string GetCurrentFileContents()
        {
            var newType = new POSHToolbeltType();
            newType.ID = _OriginalType.ID;
            newType.HasMultipleValues = _OriginalType.HasMultipleValues;
            newType.EmitQuotes = _OriginalType.EmitQuotes;
            newType.Name = _OriginalType.Name;
            newType.Regex = _OriginalType.Regex;
            var list = ListItems.Text.Split(Environment.NewLine).Where(x => !String.IsNullOrEmpty(x));
            newType.ListValues.Add(newType.Name, list.ToList());
            return JsonConvert.SerializeObject(newType, Formatting.Indented);
        }

        private void ListItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpenFileService.FileTextChanged(_FilePath);
        }
    }
}
