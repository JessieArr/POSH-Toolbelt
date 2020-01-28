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
        public MainWindow()
        {
            InitializeComponent();
            var rs = RunspaceFactory.CreateRunspace();
            rs.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var scriptTask = RunScript(Script.Text);
            scriptTask.ContinueWith(x =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var line in x.Result)
                    {
                        Output.Content += line + Environment.NewLine;
                    }
                });                
            });
        }

        private async Task<PSDataCollection<PSObject>> RunScript(string script)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(script);
                ps.Runspace = rs;
                return await ps.InvokeAsync();
            }
        }
    }
}
