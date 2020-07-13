using Newtonsoft.Json;
using POSH_Toolbelt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POSH_Toolbelt.Services
{
    public class ApplicationHistoryService
    {
        private static string _FileName = "autosave.json";
        public ApplicationHistory GetApplicationHistory()
        {
            if(!File.Exists(_FileName))
            {
                return new ApplicationHistory();
            }
            var fileContents = File.ReadAllText(_FileName);
            var history = JsonConvert.DeserializeObject<ApplicationHistory>(fileContents);
            return history;
        }

        public void SaveApplicationHistory(ApplicationHistory history)
        {
            var text = JsonConvert.SerializeObject(history);
            File.WriteAllText(_FileName, text);
        }
    }
}
