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
        private Dictionary<ConsoleColor, Color> _ColorMapping = new Dictionary<ConsoleColor, Color>()
        {
            { ConsoleColor.Black, Color.FromArgb(0xFF, 0x0, 0x0, 0x0) },
            { ConsoleColor.DarkBlue, Color.FromArgb(0xFF, 0x0, 0x0, 0x8B) },
            { ConsoleColor.DarkGreen, Color.FromArgb(0xFF, 0x0, 0x64, 0x0) },
            { ConsoleColor.DarkCyan, Color.FromArgb(0xFF, 0x0, 0x8B, 0x8B) },
            { ConsoleColor.DarkRed, Color.FromArgb(0xFF, 0x8B, 0x0, 0x0) },
            { ConsoleColor.DarkMagenta, Color.FromArgb(0xFF, 0x8B, 0x0, 0x8B) },
            { ConsoleColor.DarkYellow, Color.FromArgb(0xFF, 0x8B, 0x8B, 0x0) },
            { ConsoleColor.Gray, Color.FromArgb(0xFF, 0x80, 0x80, 0x80) },
            { ConsoleColor.DarkGray, Color.FromArgb(0xFF, 0xA9, 0xA9, 0xA9) },
            { ConsoleColor.Blue, Color.FromArgb(0xFF, 0x0, 0x0, 0xFF) },
            { ConsoleColor.Green, Color.FromArgb(0xFF, 0x00, 0x80, 0x0) },
            { ConsoleColor.Cyan, Color.FromArgb(0xFF, 0x0, 0xFF, 0xFF) },
            { ConsoleColor.Red, Color.FromArgb(0xFF, 0xFF, 0x0, 0x0) },
            { ConsoleColor.Magenta, Color.FromArgb(0xFF, 0xFF, 0x0, 0xFF) },
            { ConsoleColor.Yellow, Color.FromArgb(0xFF, 0xFF, 0xFF, 0x0) },
            { ConsoleColor.White, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF) },
        };

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
            ps.Streams.Verbose.DataAdded += Information_DataAdded;
            ps.Streams.Debug.DataAdded += Information_DataAdded;
            ps.Streams.Information.DataAdded += Information_DataAdded;
            ps.Streams.Error.DataAdded += Error_DataAdded;
            ps.Streams.Warning.DataAdded += Warning_DataAdded;
            ps.BeginInvoke((PSDataCollection<PSObject>)null, psdata);
        }

        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var data = sender as PSDataCollection<InformationRecord>;
            Dispatcher.Invoke(() =>
            {
                foreach (var datum in data)
                {
                    var msg = datum.MessageData as HostInformationMessage;
                    var foregroundColor = msg.ForegroundColor;
                    if (foregroundColor.HasValue)
                    {
                        var brush = new SolidColorBrush(_ColorMapping[foregroundColor.Value]);
                        Output.AddColoredText(msg.Message, brush);
                    }
                    else
                    {
                        Output.AddColoredText(msg.Message, Brushes.White);
                    }
                }
            });               
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var data = sender as PSDataCollection<InformationRecord>;
            Dispatcher.Invoke(() =>
            {
                foreach (var datum in data)
                {
                    var msg = datum.MessageData as HostInformationMessage;
                    var foregroundColor = msg.ForegroundColor;
                    if (foregroundColor.HasValue)
                    {
                        var brush = new SolidColorBrush(_ColorMapping[foregroundColor.Value]);
                        Output.AddColoredText(msg.Message, brush);
                    }
                    else
                    {
                        Output.AddColoredText(msg.Message, Brushes.Red);
                    }
                }
            });
        }

        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var data = sender as PSDataCollection<InformationRecord>;
            Dispatcher.Invoke(() =>
            {
                foreach (var datum in data)
                {
                    var msg = datum.MessageData as HostInformationMessage;
                    var foregroundColor = msg.ForegroundColor;
                    if (foregroundColor.HasValue)
                    {
                        var brush = new SolidColorBrush(_ColorMapping[foregroundColor.Value]);
                        Output.AddColoredText(msg.Message, brush);
                    }
                    else
                    {
                        Output.AddColoredText(msg.Message, Brushes.Yellow);
                    }
                }
            });
        }

        private void Psdata_DataAdding(object sender, DataAddingEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Output.AddColoredText(e.ItemAdded.ToString(), Brushes.White);
            });
        }
    }
}
