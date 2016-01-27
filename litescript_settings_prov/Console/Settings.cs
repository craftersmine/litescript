using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using craftersmine.Configuration;
using craftersmine.LiteScript.Cfg.Common;

namespace craftersmine.LiteScript.Cfg.Console
{
    public class Settings
    {
        public static Configuration.Configuration _cfg = new Configuration.Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\craftersmine\LiteScript\Commons\Settings.conf");
        public static string Locale { get { return _cfg.GetString("common.lang"); } }
    }
}
