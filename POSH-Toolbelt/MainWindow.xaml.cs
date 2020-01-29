using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace POSH_Toolbelt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Runspace rs;
        private PSDataCollection<PSObject> PSData;
        public MainWindow()
        {
            InitializeComponent();
            rs = RunspaceFactory.CreateRunspace();
            rs.Open();

            var color = Color.FromRgb(0, 36, 86);
            Output.Background = new SolidColorBrush(color);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RunScript(Script.Text);
        }

        private void RunScript(string script)
        {
            var ps = PowerShell.Create();
            ps.AddScript(script);
            ps.Runspace = rs;
            var psdata = new PSDataCollection<PSObject>();
            psdata.DataAdding += Psdata_DataAdding;
            ps.Streams.Information.DataAdded += Information_DataAdded;
            ps.BeginInvoke((PSDataCollection<PSObject>)null, psdata);
        }

        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var data = sender as PSDataCollection<InformationRecord>;
            foreach(var datum in data)
            {
                var msg = datum.MessageData as HostInformationMessage;
                var x = msg.ForegroundColor;
            }
            throw new NotImplementedException();
        }

        private void Psdata_DataAdding(object sender, DataAddingEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var para = new Paragraph();
                para.Margin = new Thickness(0);
                var text = new TextRange(para.ContentEnd, para.ContentEnd);
                text.Text = e.ItemAdded.ToString();
                text.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                Output.Document.Blocks.Add(para);
            });
        }
    }
}
