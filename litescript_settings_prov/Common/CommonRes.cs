using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.LiteScript.Cfg.Common
{
    public class CommonRes
    {
        public static string ApplicationData { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"craftersmine\LiteScript\"; } }
        public static string ApplicationsCommonData { get { return ApplicationData + @"Commons\"; } }
        public static string ApplicationCommonSettings { get { return ApplicationData + @"Commons\Settings.conf"; } }
    }
}
