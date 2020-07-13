using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace POSH_Toolbelt.Services
{
    public class PowershellService
    {
        public void OpenPSWindowAndRunScript(string command)
        {
            var length = command.Length;
            var commandBytes = Encoding.Unicode.GetBytes(command);
            var base64Command = Convert.ToBase64String(commandBytes);
            var base64Length = base64Command.Length;
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -EncodedCommand {base64Command}"
            });
        }
    }
}
