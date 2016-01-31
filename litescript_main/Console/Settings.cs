using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

//using craftersmine.Config;                    //That's namespace maybe deleted in future


namespace craftersmine.LiteScript.Cfg.Console
{
    public class SettingsInitializer
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\craftersmine\LiteScript\Commons\Settings.conf";
        //private static Configuration _cfg = new Configuration(path);

        public static Commons Load()
        {
            if (!File.Exists(path))
            {
                Commons _com = new Commons();
                _com.Locale = "ru-ru";
                string json = JsonConvert.SerializeObject(_com);
                File.WriteAllText(path, json);
                return _com;
            }
            else
            {
                string json = File.ReadAllText(path);
                Commons _com = JsonConvert.DeserializeObject<Commons>(json);
                return _com;
            }
        }
    }

    public class Commons
    {
        public string Locale { get; set; }
    }
}
