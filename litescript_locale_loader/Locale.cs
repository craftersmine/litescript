using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json;

using craftersmine.LiteScript.Cfg.Console;

namespace craftersmine.LiteScript.Locale.Console
{
    public class Localization
    {
            public string StateRunning;
    }

    public class LocaleLoader
    {
        public static Localization Load()
        {
            Commons Settings = SettingsInitializer.Load();
            string path = Application.StartupPath + @"\localizedRes\" + Settings.Locale + @"\console\console.lang";
            return JsonConvert.DeserializeObject<Localization>(File.ReadAllText(path));
        }
    }
}
