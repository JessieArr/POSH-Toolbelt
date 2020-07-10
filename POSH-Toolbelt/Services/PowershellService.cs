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
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -Command {command}"
            });
        }
    }
}
