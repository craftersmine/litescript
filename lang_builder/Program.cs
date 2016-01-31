using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace lang_builder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = Application.StartupPath + @"\console.lang";
                Locale _locale = new Locale();
                _locale.StateRunning = "Выполняется...";
                _locale.StateStopped = "Остановленно!";
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(_locale));
                Console.WriteLine("Success!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }

    public class Locale
    {
        public string StateRunning;
        public string StateStopped;
    }
}
